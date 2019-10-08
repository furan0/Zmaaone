/* #################################################################
 * ####     Written by A.Guillemain for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuileRender : MonoBehaviour
{

    public enum Forme
    {
        seul,
        droit,
        enT,
        croix,
        coin,
        fin,
    }

    Level.LevelData levelData;
    CableEvent rafraichissementEvent;
    public Forme forme;
    [SerializeField] Sprite [] image;
    public EDirection direction;
    public bool [] voisin;

    [SerializeField] private bool modifieDirection = true;

    void Start()
    {
        forme=Forme.seul;
        direction=EDirection.Nope;
        // Obtenir, le level manager
        LevelManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        levelData = manager.niveau;
        rafraichissementEvent = manager.eventCableRafraichissement;
        rafraichissementEvent.AddListener(updateForme);

        Invoke("updateForme", 0.0001f); //On l'exècute à la prochaine frame
    }

    /*public void updateCable(){ //On dit merci qui ? Merci Guillemaiiiiiinnn
        updateCable();  //Kaboom :p
    }*/

    void updateForme(){
        int nombreVoisin=0;
        Vector2 pos=new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        bool [] situation = {false,false,false,false};
        //Haut
        if(levelData.estDansNiveau(pos+new Vector2(0f,1f))){
            int index = levelData.vector2index(pos+new Vector2(0f,1f));
            Level.Cell cell = levelData.niveau[index];
            if(cell.block!=null){
                if(cell.block.CompareTag("Cable")){
                    nombreVoisin++;
                    situation[0]=true;
                }
            }
        }
        //bas
        if(levelData.estDansNiveau(pos+new Vector2(0f,-1f))){
            int index = levelData.vector2index(pos+new Vector2(0f,-1f));
            Level.Cell cell = levelData.niveau[index];
            if(cell.block!=null){
                if(cell.block.CompareTag("Cable")){
                    nombreVoisin++;
                    situation[1]=true;
                }
            }
        }
        //gauche
        if(levelData.estDansNiveau(pos+new Vector2(-1f,0f))){
            int index = levelData.vector2index(pos+new Vector2(-1f,0f));
            Level.Cell cell = levelData.niveau[index];
            if(cell.block!=null){
                if(cell.block.CompareTag("Cable")){
                    nombreVoisin++;
                    situation[2]=true;
                }
            }
        }
        //Droite
        if(levelData.estDansNiveau(pos+new Vector2(1f,0f))){
            int index = levelData.vector2index(pos+new Vector2(1f,0f));
            Level.Cell cell = levelData.niveau[index];
            if(cell.block!=null){
                if(cell.block.CompareTag("Cable")){
                    nombreVoisin++;
                    situation[3]=true;
                } 
            }
        }
        selectionSrpite(situation,nombreVoisin);
        voisin=situation;
        //Debug.Log(situation[0] +" "+situation[1] +" "+situation[2] +" "+situation[3]);
        
    }

    void selectionSrpite(bool [] situation,int nbVoisin){
        //if(!modifieDirection)
        //    return;

        SpriteRenderer sprite=GetComponent<SpriteRenderer>();
        sprite.flipX=false;
        sprite.flipY=false;
        switch (nbVoisin)
        {
            case 0:
                forme= Forme.seul;
                sprite.sprite=image[0];
                break;
            case 1:
                sprite.sprite=image[1];
                forme = Forme.fin;
                if(situation[0]==true){
                    direction=EDirection.Haut;
                }
                if(situation[1]==true){
                    direction=EDirection.Bas;
                }
                if(situation[2]==true){
                    direction=EDirection.Gauche;
                }
                if(situation[3]==true){
                    direction=EDirection.Droite;
                }
                break;
            case 2:
                if(situation[0]==true && situation[1]==true){
                    sprite.sprite=image[2];
                    forme=Forme.droit;
                    direction=EDirection.Haut;
                }else if(situation[0]==false && situation[1]==false){
                    direction=EDirection.Droite;
                    sprite.sprite=image[2];
                    forme=Forme.droit;
                }else
                {
                    sprite.sprite=image[3];
                    // SI YA UN BUG C LA
                    forme=Forme.coin;
                    if(situation[0]){
                        if(situation[2]){
                            direction=EDirection.Haut;
                            GetComponent<SpriteRenderer>().flipX=true;
                        } else {
                            direction=EDirection.Bas;
                        }
                    } else if (situation[1]){
                        if(situation[2]){
                            direction=EDirection.Gauche;
                            GetComponent<SpriteRenderer>().flipX=true;
                        } else {
                            direction=EDirection.Gauche;
                        }
                    }
                }
                break;
            case 3:
                forme=Forme.enT;
                sprite.sprite=image[4];
                if(situation[0]){
                    if(situation[1]){
                        if(situation[2]){
                            direction=EDirection.Haut;
                        }else
                        {
                            direction=EDirection.Bas;
                        }
                    }else{
                        direction=EDirection.Droite;
                    }
                }else{
                    direction=EDirection.Gauche;
                }
                break;
                case 4:
                    forme=Forme.croix;
                    sprite.sprite=image[5];   
                break;
            default:
            Debug.Log("ha ba merde");
            break;
        }
        if(modifieDirection)
            GetComponent<Cable>().setDirection(direction);
        GetComponent<ActualiserRotation>().rafraichir();
    }
}