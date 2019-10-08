/* #################################################################
 * ####        Written by A.Delecroix & A.Guillemain            ####
 * ####                for the Ludum Dare 45                    ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diode :Cable
{
    public override void Start() {
        base.Start();
        detruit = false;
    }
    
    public override void notifierCharge(bool [] robert){
        
        if(direction == EDirection.Haut){
            if(robert[1]){
                monAieu=robert;
                charge=true;  
            }
        }
        else if(direction == EDirection.Bas){
            if(robert[0]){
                monAieu=robert;
                charge=true;
            }
        }
        else if(direction == EDirection.Gauche){
            if(robert[3]){
                monAieu=robert;
                charge=true;
            }
        }
        else if(direction == EDirection.Droite){
            if(robert[2]){
                monAieu=robert;
                charge=true;
            }
        } else {
            bool[] arr = {false, false, false, false};
            monAieu = arr;
        }

        detruit=false;
        bool[] empty = {false, false, false, false};
        provenanceElec = empty;
        monProchain();
    }

    public override void monProchain()
    {
        base.monProchain();
        int indexASauver = 0;
        if(direction == EDirection.Haut)
            indexASauver = 0; 
        else if(direction == EDirection.Bas)
            indexASauver = 1;
        else if(direction == EDirection.Gauche)
            indexASauver = 2;
        else if(direction == EDirection.Droite)
            indexASauver = 3;

        bool[] arr = {false, false, false, false};
        arr[indexASauver] = voisin[indexASauver];
        voisin = arr;
    }
}
