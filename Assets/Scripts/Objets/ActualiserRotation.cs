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

[ExecuteInEditMode]
public class ActualiserRotation : MonoBehaviour
{
    private Level.IBlockType block;
    private EDirection blockdir;

    private float zAngle;
    [SerializeField] private float offset; // vaut 0, 1, 2 ou 3

    private void OnEnable()
    {
        rafraichir();
    }

    public void rafraichir(){
        block = GetComponent<Level.IBlockType>();
        blockdir = block.getDirection();

        zAngle = -(Mathf.Max((float)blockdir - 1, 0) + offset) * 90;
        transform.rotation = Quaternion.Euler(0, 0, zAngle);
    }
}
