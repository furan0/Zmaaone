/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Carton))]
public class CartonAPiston : TimerListener
{
    
    public float dureeDeplacement;
    
    private Level.LevelData niveau;
    private Carton cartonATapis;
    private PistonEvent eventPiston;
    private PistonBloqueEvent eventPistonBloque;

    private float tempsDeplacementRestant;
    private Vector2 oldPos = new Vector2();
    private Vector2 nextPos;

    [SerializeField]
    private bool enCoursDeDeplacement = false;
    [SerializeField] private bool peutSeDeplacer = false;

    void Start() {
        LevelManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        niveau = manager.niveau;
        cartonATapis = GetComponent<Carton>();
        eventPiston = manager.eventPiston;
        eventPiston.AddListener(calculNextPos);
        eventPistonBloque = manager.eventPistonBloque;
        eventPistonBloque.AddListener(eventBloque);


        tempsDeplacementRestant = -1;
        oldPos.x = Mathf.Round(transform.position.x);
        oldPos.y = Mathf.Round(transform.position.y);
        nextPos = oldPos;
    }

    void Update() {
        if(enCoursDeDeplacement && tempsDeplacementRestant > 0) {
            tempsDeplacementRestant -= Time.deltaTime;
            float facteur = Mathf.Min(1 - (tempsDeplacementRestant / dureeDeplacement), 1);
            Vector2 stepPos = Vector2.Lerp(oldPos, nextPos, facteur);
            transform.position = new Vector3(stepPos.x, stepPos.y, 0);

        } else if (enCoursDeDeplacement && tempsDeplacementRestant <= 0) { //Position finale
            transform.position = new Vector3(nextPos.x, nextPos.y, 0);
            oldPos = nextPos;
            enCoursDeDeplacement = false;
            peutSeDeplacer = false;
            tempsDeplacementRestant = 0;
            Invoke("calculCartonClassique", 0.0001f);
        }

        /*if (niveau.estDansNiveau(oldPos)) {
            int index = niveau.vector2index(oldPos);
            GameObject block = niveau.niveau[index].block;
            Receptacle receptacle = (block != null)? block.GetComponent<Receptacle>() : null;
            if(receptacle != null && !receptacle.estModeEdition) {
                receptacle.cubeQuiTombe(cartonATapis.couleur);
                //Mettre une Zouli petite anim ici please
                Destroy(gameObject);
            }
        }*/
    }

    public override void timerEvent() {
        //Debug.Log("Deplacement carton");
        if(peutSeDeplacer) {
            tempsDeplacementRestant = dureeDeplacement;
            enCoursDeDeplacement = true;
        }
    }

    //Calcul le prochain déplacement du carton
    private void calculNextPos(Vector2 pos, EDirection dir) {
        oldPos.x = Mathf.Round(transform.position.x);
        oldPos.y = Mathf.Round(transform.position.y);
        
        Debug.Log("Déplacement piston : " + pos + " : " + dir + " avec pos " + oldPos);
        if (pos.Equals(oldPos)) { //Le carton doit se déplacer
            peutSeDeplacer = false;
            Vector2 newPos = pos + Direction.dir2Vec(dir);

            if (niveau.estDansNiveau(newPos)) { 
                int index = niveau.vector2index(pos);
                peutSeDeplacer = true;
                nextPos = newPos;
                eventPiston.Invoke(newPos, dir); //On propage l'ordre de déplacement
            
            } else { //On prévient si on ne peut pas se déplacer
                Debug.Log("Bloque : " + pos);
                eventPistonBloque.Invoke(pos);
            }
        }  
    }

    protected void eventBloque(Vector2 pos) { //Recu lorsqu'un carton ne peux pas se déplacer
        Debug.Log("event Bloque " + pos + " avec next pos : " + nextPos);
        //calculNextPos();
        
        if (peutSeDeplacer && pos.Equals(nextPos)) { //Si on devait se déplcer sur cette case, on annule
            Debug.Log("Annulation dep from " + nextPos + " to " + oldPos);
            nextPos = oldPos;
            peutSeDeplacer = false;
            eventPistonBloque.Invoke(oldPos); //On prévient les autres
        }
    }

    private void calculCartonClassique() {
        cartonATapis.aEteDeplace();
    }
}
