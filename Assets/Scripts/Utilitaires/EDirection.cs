/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

public enum EDirection
{
    Nope,
    Haut,
    Droite,
    Bas,
    Gauche
}

public static class Direction {
    public static Vector2 dir2Vec(EDirection dir) {
        Vector2 vec = new Vector2();

        if (dir == EDirection.Haut)
            vec.y = 1;
        else if (dir == EDirection.Bas)
            vec.y = -1;
        else 
            vec.y = 0;

        if (dir == EDirection.Droite)
            vec.x = 1;
        else if (dir == EDirection.Gauche)
            vec.x = -1;
        else 
            vec.x = 0;
        
        return vec;
    }
}