/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections.Generic;
using UnityEngine;

namespace Level {
    public interface IBlockType {
        void modeEdition(bool etat);
        void nextDirection();
        EDirection getDirection();
    }

    [System.Serializable]
    public struct Cell {
        public GameObject block;
        public bool estEditable;
        public EDirection direction;
    }

    [System.Serializable]
    public struct coupleCarton {
        public GameObject prefabCarton;
        public Vector2 pos;
    }

    //Scriptable object contenant un niveau
    [CreateAssetMenu(fileName = "New LevelData", menuName = "LevelData", order = 52)]
    public class LevelData : ScriptableObject
    {
        public int nbLigne;
        public int nbColonne;
        public List<Cell> niveau;

        public int vector2index(Vector2 pos) {
            return Mathf.Max((int) pos.y, 0) * nbLigne + Mathf.Max((int) pos.x, 0);
        }

        public bool estDansNiveau(Vector2 pos) {
            bool output = pos.x < nbColonne && pos.x >= 0;
            return output && pos.y < nbLigne && pos.y >= 0;
        }
    }
}
