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
/**
    Script permettant d'avoir une scrolling texture.
 */
public class AnimationScroll : MonoBehaviour
{
    [SerializeField] float speed;
    float currentscroll=0;
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        currentscroll += speed * Time.deltaTime;
        material.mainTextureOffset=new Vector2(0f,currentscroll);
    }
}
