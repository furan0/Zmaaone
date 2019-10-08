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

//Fonction bloquant les cartons par le piston -> Reelement utile ?????
public class PistonBloquant : TimerListener
{
    public float delaiBlocage = 0.250f; //Délai avant de signaler le blocage
    private CartonEvent cartonBloqueEvent;
    private Piston pistonOriginel;

    public override void timerEvent()
    {
        Invoke("signalement", delaiBlocage);
    }

    // Start is called before the first frame update
    void Start()
    {
        LevelManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        cartonBloqueEvent = manager.eventCartonsBloque;

        pistonOriginel = GetComponent<Piston>();
    }

    void signalement() {
        
    }
}
