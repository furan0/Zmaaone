/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Receptacle : MonoBehaviour, Level.IBlockType
{
    public EDirection direction;
    public Carton.ECouleur couleur = Carton.ECouleur.Jaune;
    private GestionnaireJeu manager;

    public bool estModeEdition = false;

    //Gestion Audio
    public AudioClip bruitBonCarton;
    public AudioClip bruitMauvaisCarton;
    private AudioSource source;

    public EDirection getDirection()
    {
        return direction;
    }

    public void modeEdition(bool etat) { 
        estModeEdition = etat;
    } //Pas de mode édition 

    public void nextDirection()
    {
        if(++direction == EDirection.Nope) {
            direction = EDirection.Haut;
        }

        ActualiserRotation actu = GetComponent<ActualiserRotation>();
        if (actu != null)
            actu.rafraichir();
    }

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GestionnaireJeu>();
        source = GetComponent<AudioSource>();
    }

    public void cubeQuiTombe(Carton.ECouleur c) //Appelé par un cube qui va vers le réceptacle
    {
        if(c == couleur) { //Bon carton
            manager.cubeRecupere();
            if(bruitBonCarton != null)
                source.PlayOneShot(bruitBonCarton);

        } else { //Mauvais carton
            manager.mauvaisCube();
            if(bruitMauvaisCarton != null)
                source.PlayOneShot(bruitMauvaisCarton);
        }
    }
}
