/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

public abstract class TimerListener : MonoBehaviour
{
    public TimerEvent evenement;

    private void OnEnable() {
        evenement.addListener(this);
    }

    private void OnDisable() {
        evenement.removeListener(this);
    }
    public abstract void timerEvent();
}
