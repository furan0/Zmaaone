/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    public bool editeLaMatrice = false;
    private LevelManager manager;
    private Vector2 pos = new Vector2(-1, -1);
    public bool editableParDefault = true;
    public bool dummyJustePourEditable = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        pos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        if (editeLaMatrice && manager.niveau.estDansNiveau(pos)) {
            int index = manager.niveau.vector2index(pos);
            if (manager.niveau.niveau[index].block == null) { //On vérifie qu'il n'y ai rienen dessous !
                GameObject obj = (dummyJustePourEditable)? null : gameObject;
                manager.niveau.niveau[index] = new Level.Cell { //On enregistre le block dans le niveau
                        estEditable = editableParDefault,
                        block = obj,
                        direction = EDirection.Nope
                    };
            }
        }

        if(dummyJustePourEditable && !Application.isEditor)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor) {
            Vector2 newPos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
            if(editeLaMatrice && !newPos.Equals(pos)) {
                
                if (manager != null && manager.niveau.estDansNiveau(newPos)) {
                    int index = manager.niveau.vector2index(newPos);
                    manager.niveau.niveau[index] = new Level.Cell { //On enregistre le block dans le niveau
                            estEditable = editableParDefault,
                            block = gameObject,
                            direction = EDirection.Nope
                        };
                }

                //on supprime l'ancienne position
                if(manager.niveau.estDansNiveau(pos)) {
                    int oldIndex = manager.niveau.vector2index(pos);
                    manager.niveau.niveau[oldIndex] = new Level.Cell { //On enregistre le block dans le niveau
                            estEditable = false,
                            block = null,
                            direction = EDirection.Nope
                        };
                }
                pos = newPos;
            } else if (!newPos.Equals(pos)) {
                pos = newPos;
            }
            transform.position = new Vector3(pos.x, pos.y, 0); //MaJ pos block
        } 
    }

    void OnDestroy() {
       if(editeLaMatrice && manager != null && manager.niveau.estDansNiveau(pos)) {
                    int oldIndex = manager.niveau.vector2index(pos);
                    manager.niveau.niveau[oldIndex] = new Level.Cell { //On enregistre le block dans le niveau
                            estEditable = false,
                            block = null,
                            direction = EDirection.Nope
                        };
                } 
    }
}
