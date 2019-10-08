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

public class Electricite : MonoBehaviour, Level.IBlockType
{
    public EDirection direction;
    public void alimenter()
    {
        //todo 
    }

    public EDirection getDirection()
    {
        return direction;
    }

    public void setDirection(EDirection dir){
        direction=dir;
    }

    

    public void modeEdition(bool etat)
    {
        //todo
    }

    public void nextDirection()
    {
        //todo
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
