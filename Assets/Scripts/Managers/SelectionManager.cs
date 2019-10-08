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

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private bool editMode = true;

    public GameObject prefabCable;
    public GameObject prefabDiode;
    public Texture2D curseurAjouter;
    public Texture2D curseurImpossible;
    public Vector2 offsetCursor;

    private GameObject prefabSelection;
    private GameObject blockSelection;
    private Level.IBlockType composantSelection;
    private Vector2 posSelection;
    private Vector2 lastPos;
    private bool dejaPop = false;


    private LevelManager manager;
    private CableEvent raffraichirCable;

    public AudioClip bruitPose;
    public AudioClip bruitDetruit;
    private AudioSource source;

    public GameObject effetDetruit;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<LevelManager>();
        raffraichirCable = manager.eventCableRafraichissement;
        prefabSelection = prefabCable;
        dejaPop = false;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(editMode) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posSelection = new Vector2(Mathf.Max(Mathf.Round(mousePos.x), 0), Mathf.Max(Mathf.Round(mousePos.y), 0));

            //Si dans le niveau, on déplace le gameobject associé
            
            if(manager.niveau.estDansNiveau(posSelection)) {
                int index =  manager.niveau.vector2index(posSelection);
                Vector3 position3 = new Vector3(posSelection.x, posSelection.y, 0);

                if(manager.niveau.niveau[index].estEditable) {
                    if(blockSelection == null) { //Instanciation si nécessaire 
                        blockSelection = Instantiate(prefabSelection, position3, Quaternion.identity);
                        blockSelection.GetComponent<SnapToGrid>().enabled = false;
                        blockSelection.GetComponent<SpriteRenderer>().sortingLayerName = "Edition";
                        composantSelection = blockSelection.GetComponent<Level.IBlockType>();
                        composantSelection.modeEdition(true); //On le met en mode édition
                    } else
                        blockSelection.transform.position = position3;
                    
                    //Modif curseur
                    if(!lastPos.Equals(posSelection))
                        Cursor.SetCursor(curseurAjouter, Vector2.zero, CursorMode.Auto);

                    //MaJ des cables
                    if(!lastPos.Equals(posSelection)) {
                        raffraichirCable.Invoke();
                        
                    }

                    if(dejaPop && !lastPos.Equals(posSelection))
                        dejaPop = false;

                    //Gestion Inputs
                    if((Input.GetMouseButton(0) | Input.GetMouseButtonUp(0)) && blockSelection != null) { //Clic gauche -> on place le bloc si possible
                        if(!dejaPop) //On n'a pas déjà posé ce bloc
                        {
                            //On détruit l'ancien si présent
                            GameObject ancien = manager.niveau.niveau[index].block; 
                            if (ancien != null)
                                Destroy(ancien);

                            //On insère le nouveau
                            blockSelection.transform.position = position3;
                            manager.niveau.niveau[index] = new Level.Cell {
                                    block = blockSelection,
                                    estEditable = true,
                                    direction = composantSelection.getDirection()
                                };
                            composantSelection.modeEdition(false);
                            blockSelection.GetComponent<SpriteRenderer>().sortingLayerName = "CablesTapis";
                            raffraichirCable.Invoke(); //MaJ cablage

                            //On dissocie l'instance
                            blockSelection = null;
                            composantSelection = null;
                            dejaPop = true;

                            //On joue un son
                            if(bruitPose != null) {
                                source.pitch = Random.Range(0.8f, 1.2f);
                                source.PlayOneShot(bruitPose);
                            }
                        }
                    } else if (Input.GetMouseButtonUp(1)) { //Clic droit -> on switch entre la diode et le cable
                        prefabSelection = (prefabSelection == prefabCable)? prefabDiode : prefabCable; //on inverse
                        
                        if(blockSelection != null) { //On détruit l'ancien
                            Destroy(blockSelection);
                        }

                        //On créer le nouveau
                        blockSelection = Instantiate(prefabSelection, position3, Quaternion.identity);
                        blockSelection.GetComponent<SnapToGrid>().enabled = false;
                        blockSelection.GetComponent<SpriteRenderer>().sortingLayerName = "Edition";
                        composantSelection = blockSelection.GetComponent<Level.IBlockType>();
                        if(composantSelection != null)
                            composantSelection.modeEdition(true); //On le met en mode édition
                        manager.eventCableRafraichissement.Invoke(); //On raffraichit les cables

                    } else if (Input.GetButtonUp("Rotate")) {
                        if(composantSelection != null)
                            composantSelection.nextDirection();
                    
                    } else if (Input.GetButtonUp("Destroy")) { //On détruit celui en dessous
                        if (manager.niveau.niveau[index].estEditable) {
                            Destroy(manager.niveau.niveau[index].block);
                            manager.niveau.niveau[index] = new Level.Cell {
                                block = null,
                                estEditable = true,
                                direction = EDirection.Nope
                            };

                            //On joue un son
                            if(bruitDetruit != null) {
                                source.pitch = 1.0f;
                                source.PlayOneShot(bruitDetruit);
                            }

                            //On joue un effet
                            if(effetDetruit != null) {
                                GameObject obj = Instantiate(effetDetruit, posSelection, Quaternion.identity);
                                Destroy(obj, 0.2f);
                            }
                        }
                    }
                } else {
                    if(!lastPos.Equals(posSelection))
                        Cursor.SetCursor(curseurImpossible, Vector2.zero, CursorMode.Auto);
                }
            } else {
                    if(!lastPos.Equals(posSelection))
                        Cursor.SetCursor(curseurImpossible, Vector2.zero, CursorMode.Auto);
                }

            lastPos = posSelection;
        }
    }

    public void changeEditMode(bool etat) {
        if (etat) {
            editMode = true;
            dejaPop = false;
            Cursor.SetCursor(curseurImpossible, Vector2.zero, CursorMode.Auto);

            Vector3 position3 = new Vector3(posSelection.x, posSelection.y, 0);
            if(blockSelection == null) { //Instanciation si nécessaire 
                blockSelection = Instantiate(prefabSelection, position3, Quaternion.identity);
                blockSelection.GetComponent<SnapToGrid>().enabled = false;
                blockSelection.GetComponent<SpriteRenderer>().sortingLayerName = "Edition";
                composantSelection = blockSelection.GetComponent<Level.IBlockType>();
                composantSelection.modeEdition(true); //On le met en mode édition
            } else
                blockSelection.transform.position = position3;
        } else {
            editMode = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if(blockSelection != null)
                Destroy(blockSelection);
        }
    }
} 
