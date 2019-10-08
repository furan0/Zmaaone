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

public class Tapis : TimerListener, Level.IBlockType
{
    public EDirection direction;
    public bool estModeEdition = false;

    public float dureeAnimation = 0.2f;
    private float dureeRestante = 0;
    private bool anime = false;
    private Animator animator;

    public EDirection getDirection()
    {
        return direction;
    }

    public void modeEdition(bool etat)
    {
        estModeEdition = etat;
    }

    public void nextDirection()
    {
        if(++direction == EDirection.Nope) {
            direction = EDirection.Haut;
        }

        ActualiserRotation actu = GetComponent<ActualiserRotation>();
        if (actu != null)
            actu.rafraichir();
    }

    public override void timerEvent()
    {
        dureeRestante = dureeAnimation;
        anime = true;
        animator.SetBool("Bouge", true);

    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Bouge", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (anime && dureeRestante > 0) {
            dureeRestante -= Time.deltaTime;
        } else if (anime) {
            anime = false;
            dureeRestante = 0;
            animator.SetBool("Bouge", false);
        }
    }
}
