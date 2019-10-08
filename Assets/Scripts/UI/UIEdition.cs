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

[RequireComponent(typeof(AudioSource))]
public class UIEdition : MonoBehaviour
{
    GestionnaireJeu manager;
    SelectionManager selectionManager;
    UIManager uiManager;
    public bool editionTest = false;
    private bool lastEditionTest = false;

    public AudioClip sonEdition;
    public AudioClip sonRetour;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controller = GameObject.FindGameObjectWithTag("GameController");
        manager = controller.GetComponent<GestionnaireJeu>();
        selectionManager = controller.GetComponent<SelectionManager>();
        uiManager = controller.GetComponent<UIManager>();

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(editionTest != lastEditionTest) { //Debug
            modeEdition(editionTest);
            lastEditionTest = editionTest;
        }
    }

    public void modeEdition(bool etat) { //true -> edite, false -> joue
        if(!etat) {
            manager.stopNiveau();

            //Son 
            if(sonEdition != null) {
                source.pitch = 0.5f;
                source.PlayOneShot(sonEdition);
            }

            //On active le mode édition
            selectionManager.changeEditMode(true);
        } else {
            selectionManager.changeEditMode(false);
            manager.startNiveau();
        }
    }

    public void ExitLevel() {
        uiManager.retourMenu();
    }

    public void nextLevel() {
        uiManager.nextLevel();
    }
}
