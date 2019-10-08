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

public class ElectriciteAffichage : MonoBehaviour
{
    Level.LevelData levelData;
    Animator animator;
    ActualiserRotation actualiserRotation;
    private void Start() {
        levelData = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().niveau;
        animator=GetComponent<Animator>();
        actualiserRotation=GetComponent<ActualiserRotation>();
        //Invoke("updateForme",0.001f); 
        updateForme();
    }

    /*private void Update() {
        updateForme();
    }*/
    void updateForme(){
        if(levelData.estDansNiveau(transform.position)){
            int index = levelData.vector2index(transform.position);
            Level.Cell cell = levelData.niveau[index];
            if(cell.block!=null){
                if(cell.block.CompareTag("Cable")){
                    switch (cell.block.GetComponent<TuileRender>().forme)
                    {
                        case (TuileRender.Forme.seul):
                            animator.SetInteger("Direction",0);
                            GetComponent<Electricite>().setDirection(cell.block.GetComponent<Cable>().getDirection());
                            actualiserRotation.rafraichir();
                        break;
                        case (TuileRender.Forme.fin):
                            animator.SetInteger("Direction",0);
                            GetComponent<Electricite>().setDirection(cell.block.GetComponent<Cable>().getDirection());
                            actualiserRotation.rafraichir();
                        break;
                        case (TuileRender.Forme.droit):
                            animator.SetInteger("Direction",1);
                            GetComponent<Electricite>().setDirection(cell.block.GetComponent<Cable>().getDirection());
                            actualiserRotation.rafraichir();
                        break;
                        case (TuileRender.Forme.coin):
                            animator.SetInteger("Direction",2);
                            GetComponent<Electricite>().setDirection(cell.block.GetComponent<Cable>().getDirection());
                            actualiserRotation.rafraichir();
                        break;
                        case (TuileRender.Forme.enT):
                            animator.SetInteger("Direction",3);
                            GetComponent<Electricite>().setDirection(cell.block.GetComponent<Cable>().getDirection());
                            actualiserRotation.rafraichir();
                        break;
                        case (TuileRender.Forme.croix):
                            animator.SetInteger("Direction",4);
                            GetComponent<Electricite>().setDirection(cell.block.GetComponent<Cable>().getDirection());
                            actualiserRotation.rafraichir();
                        break;
                        default:
                        break;
                    }
                    GetComponent<SpriteRenderer>().flipX=cell.block.GetComponent<SpriteRenderer>().flipX;
                }
            }
        }else
        {
            print("electricte peaume");
        }
    }
}
