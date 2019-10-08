/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

public class CompteurGenerateur : TimerListener
{
    public int nbBeatsGenerateur = 8;
    public int offsetGenerateur = 0;

    public int nextGeneration;
    void Start() {
        nextGeneration = offsetGenerateur + nbBeatsGenerateur;
    }

    public override void timerEvent()
    {
        nextGeneration--;
        if (nextGeneration == 0)
            nextGeneration = nbBeatsGenerateur;
    }
}