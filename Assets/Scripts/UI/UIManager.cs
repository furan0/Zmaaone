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
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public string nextLevelName;
    [SerializeField] GameObject [] panels;

    private int activePanel = 0;

    public AudioClip sonClic;
    private AudioSource source;

    void Start() {
        switchPanel(activePanel);
        source = GetComponent<AudioSource>();
    }

    public void allerAlaScene(string nomDeScene){
        SceneManager.LoadScene(nomDeScene);
    }

    public void switchPanel(int numeroPanel){
        activePanel = numeroPanel;
        if (numeroPanel < panels.Length){
            if(sonClic != null && source != null) {
                source.pitch = 0.5f;
                source.PlayOneShot(sonClic);
            }
            for(int i = 0; i< panels.Length;i++){
                panels[i].SetActive(false);
            }
            panels[numeroPanel].SetActive(true);
        }
    }

    public int getActivePanel() {
        return activePanel;
    }


    public void retourMenu() {
        allerAlaScene("EcranTitre");
    }

    public void nextLevel() {
        allerAlaScene(nextLevelName);
    }
}
