/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TimerEvent", menuName = "TimerEvent", order = 51)]
public class TimerEvent : ScriptableObject
{
    private List<TimerListener> listeners = new List<TimerListener>();
    
    public void raise() {
        foreach (TimerListener l in listeners)
        {
            l.timerEvent();
        }
    }

    public void addListener(TimerListener listener) {
        listeners.Add(listener);
    }

    public void removeListener(TimerListener listener) {
        listeners.Remove(listener);
    }
}