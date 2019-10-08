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

public class Carton : TimerListener
{
    public enum ECouleur {Jaune, Bleu}
    public float dureeDeplacement;

    [SerializeField]
    public ECouleur couleur; //Couleur du carton
    
    private Level.LevelData niveau;
    private CartonEvent eventCartonsBloque;

    private float tempsDeplacementRestant;
    private Vector2 oldPos = new Vector2();
    private Vector2 nextPos;

    [SerializeField]
    private bool enCoursDeDeplacement = false;
    private bool peutSeDeplacer = false;
    [SerializeField]
    private bool calculFait = false;
    private bool enTrainDeMourrir = false;

    void Start() {
        LevelManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        niveau = manager.niveau;
        eventCartonsBloque = manager.eventCartonsBloque;
        eventCartonsBloque.AddListener(eventBloque);

        tempsDeplacementRestant = -1;
        oldPos.x = Mathf.Floor(transform.position.x);
        oldPos.y = Mathf.Floor(transform.position.y);
        nextPos = oldPos;
        enTrainDeMourrir = false;
    }

    void Update() {
        if(enTrainDeMourrir)
            return;

        if(enCoursDeDeplacement && tempsDeplacementRestant > 0) {
            tempsDeplacementRestant -= Time.deltaTime;
            float facteur = Mathf.Min(1 - (tempsDeplacementRestant / dureeDeplacement), 1);
            Vector2 stepPos = Vector2.Lerp(oldPos, nextPos, facteur);
            transform.position = new Vector3(stepPos.x, stepPos.y, 0);

        } else if (enCoursDeDeplacement && tempsDeplacementRestant <= 0) { //Position finale
            transform.position = new Vector3(nextPos.x, nextPos.y, 0);
            oldPos = nextPos;
            enCoursDeDeplacement = false;
            tempsDeplacementRestant = 0;
            Invoke("calculNextPos", 0.1f);
        } else if (tempsDeplacementRestant <= 0) {
            tempsDeplacementRestant = 0;
            Invoke("calculNextPos", 0.1f);
        }

        //Check si on va dans un tuyau
        checkTuyau();
    }

    public override void timerEvent() {
        //Debug.Log("Deplacement carton");
        calculFait = false;

        peutSeDeplacer = true;
        tempsDeplacementRestant = dureeDeplacement;

        if(peutSeDeplacer) {
            enCoursDeDeplacement = true;
        }
        
    }

    //Calcul le prochain déplacement du carton
    private void calculNextPos() {
        if (calculFait || enTrainDeMourrir)
            return;

        peutSeDeplacer = false;
        Vector2 pos = new Vector2();
        pos.x = Mathf.Round(transform.position.x);
        pos.y = Mathf.Round(transform.position.y);

        if (niveau.estDansNiveau(pos)) {
            int index = niveau.vector2index(pos);
            Tapis tapis = (niveau.niveau[index].block != null)? niveau.niveau[index].block.GetComponent<Tapis>() : null;

            if(tapis != null && !tapis.estModeEdition) { //il y a un tapis, on avance selon la direction
                EDirection dir = tapis.getDirection();
                nextPos = pos + Direction.dir2Vec(dir);
                peutSeDeplacer = true;
            }
        }
        
        calculFait = true;

        if(!peutSeDeplacer){ //On prévient si on ne peut pas se déplacer
            //Debug.Log("Bloque : " + pos);
            eventCartonsBloque.Invoke(pos);
        }

        
    }

    protected void eventBloque(Vector2 pos) { //Recu lorsqu'un carton ne peux pas se déplacer
        //Debug.Log("event Bloque " + pos + " avec next pos : " + nextPos + " status " + calculFait);
        calculNextPos();
        
        if (peutSeDeplacer && pos.Equals(nextPos)) { //Si on devait se déplcer sur cette case, on annule
            //Debug.Log("Annulation dep from " + nextPos + " to " + oldPos);
            nextPos = oldPos;
            peutSeDeplacer = false;
            eventCartonsBloque.Invoke(oldPos); //On prévient les autres
        }
    }

    public void aEteDeplace() {
        calculFait = false;
        oldPos.x = Mathf.Floor(transform.position.x);
        oldPos.y = Mathf.Floor(transform.position.y);
        nextPos = oldPos;
        checkTuyau();
        calculNextPos();
    }

    private void checkTuyau() { //Vérifie si on est dans un tuyau
        if (niveau.estDansNiveau(oldPos) && !enTrainDeMourrir) {
            int index = niveau.vector2index(oldPos);
            GameObject block = niveau.niveau[index].block;
            Receptacle receptacle = (block != null)? block.GetComponent<Receptacle>() : null;
            if(receptacle != null && !receptacle.estModeEdition) {
                GetComponent<Animator>().SetTrigger("finDeVie");
                Debug.Log("finDeVie");
                receptacle.cubeQuiTombe(couleur);
                //Mettre Zouli petite anim
                enTrainDeMourrir = true;
                peutSeDeplacer = true;
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
