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

public class obstacle : TimerListener
{
    private LevelManager manager;
    private PistonBloqueEvent pistonBloqueEvent;
    private CartonEvent cartonEvent;

    private Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        manager.eventPiston.AddListener(blocagePiston);
        pistonBloqueEvent = manager.eventPistonBloque;
        cartonEvent = manager.eventCartonsBloque;

        pos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    public override void timerEvent() //Timer des cartons
    {
        cartonEvent.Invoke(pos);
    }

    private void blocagePiston(Vector2 posPoussage, EDirection _) { //Lorsqu'un piston pousse
        if(pos.Equals(posPoussage))
            pistonBloqueEvent.Invoke(pos);
    }
}
