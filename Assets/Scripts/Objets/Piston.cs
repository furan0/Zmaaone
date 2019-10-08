/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Piston : Cable
{
    private PistonEvent pistonEvent;
    private PistonBloqueEvent pistonBloqueEvent;
    private CartonEvent cartonBloqueEvent;

    private Animator animator;
    public bool estSortie = false;
    private bool estBloque = false;
    public Vector2 posPiston;

    public AudioClip bruitPistonSortant;
    public AudioClip bruitPistonBloque;
    private AudioSource source;

    public GameObject effetBloquage;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        cartonBloqueEvent = manager.eventCartonsBloque;
        pistonEvent = manager.eventPiston;
        pistonEvent.AddListener(cartonPousse);
        pistonBloqueEvent = manager.eventPistonBloque;
        pistonBloqueEvent.AddListener(eventBloque);

        animator = GetComponent<Animator>();
        animator.SetBool("Sortie", estSortie);

        Vector2 pos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        posPiston = pos + Direction.dir2Vec(direction);

        source = GetComponent<AudioSource>();
    }

    public override void timerEvent()
    {  
        if(charge) { //Le cable est alimenté, on le signal
            if(elcetriciteActuelle != null)
                Destroy(elcetriciteActuelle);
            

            if(estSortie) {
                estSortie = false;
                /*if (bruitPistonRentrant != null)
                    source.PlayOneShot(bruitPistonRentrant);*/
                
            } else {
                
                if (!estSortie && !estBloque) {
                    estSortie = true;
                    if(bruitPistonSortant != null)
                        source.PlayOneShot(bruitPistonSortant);

                } else if (!estSortie) {
                    //Veux sortir mais bloqué : mettre une anim ici
                    if(effetBloquage != null) {
                        GameObject obj = Instantiate(effetBloquage, posPiston, Quaternion.identity);
                        Destroy(obj, 0.2f);
                    }

                    if(bruitPistonBloque != null)
                        source.PlayOneShot(bruitPistonBloque);
                }
            }

            animator.SetBool("Sortie", estSortie);
        }

        estBloque = false;
        charge = false;

        if(estSortie) 
            cartonBloqueEvent.Invoke(posPiston); //On préviens les cubes que la position du piston est bloqué
    }

    protected void eventBloque(Vector2 pos) { //Recu lorsqu'un carton ne peux pas se déplacer
        // Debug.Log("event piston Bloque " + pos);    
        if (charge && pos.Equals(posPiston) && !estBloque) { //Si on devait pousser cette case, on annule
            Debug.Log("Annulation piston bloque");
            estBloque = true;
        }
    }

    public override void notifierCharge(bool [] robert) { //Appelé lorsqu'on le charge
        elcetriciteActuelle = Instantiate(effetElec, transform.position, transform.rotation);
        charge = true;
        Invoke("notifierDuDeplacement", 0.1f); //On préviens les cartons qu'ils vont bouger
    }

    private void notifierDuDeplacement() {
        pistonEvent.Invoke(posPiston, direction);
    }

    public override void detruireCourant() {
        base.detruireCourant();
        estSortie = false;
        estBloque = false;
        animator.SetBool("Sortie", false);
    }

    private void cartonPousse(Vector2 pos, EDirection dir) { //Appelé lorsqu'un carton est poussé par un piston
        Vector2 maPos = posPiston - Direction.dir2Vec(direction);
        if(pos.Equals(maPos) || (pos.Equals(posPiston) && estSortie))
            pistonBloqueEvent.Invoke(pos);
    }
}
