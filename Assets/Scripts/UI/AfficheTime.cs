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

public class AfficheTime : MonoBehaviour
{
    TextMesh textMesh;
    CompteurGenerateur compteur;
    private void Start() {
        compteur=GetComponent<CompteurGenerateur>();
        textMesh=GetComponent<TextMesh>();
    }
    private void Update() {
        textMesh.text=compteur.nextGeneration.ToString();
    }
}
