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

[RequireComponent(typeof(AudioSource))]
public class Generatrice : Cable
{
    public AudioClip bruitGeneration;
    private AudioSource source;

    void Start() {
        base.Start();

        source = GetComponent<AudioSource>();
    }
    public override void timerEvent()
    {
        charge=false;
        bool [] masque = {false,false,false,false};
        notifierCharge(masque);
        Invoke("propagerElectricite",0.001f);
        if(bruitGeneration != null)
            source.PlayOneShot(bruitGeneration);
    }
}
