/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

public class hideInGame : MonoBehaviour
{
    private SpriteRenderer spRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        if(!Application.isEditor && spRenderer != null) {
            spRenderer.enabled = false;
        }
    }
}
