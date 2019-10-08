/* #################################################################
 * ####        Written by A.Delecroix & A.Guillemain            ####
 * ####                for the Ludum Dare 45                    ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTyper : MonoBehaviour
{
    
    public float tempsEntreChaqueLettres;
    Text texte;

    private string phrasePrecedante;
    private DialogueCallback kiLeDemandePrecedant;


    public List<AudioClip> sons;
    private AudioSource source;

    void Start()
    {
        texte = GetComponentInChildren<Text>(); 
        source = GetComponent<AudioSource>();
    }

    public void afficher(string phrase,DialogueCallback kiLeDemande){
        texte = GetComponentInChildren<Text>(); 
        StopAllCoroutines();
        texte.text="";
        phrasePrecedante = phrase;
        kiLeDemandePrecedant = kiLeDemande;
        StartCoroutine(type(phrase,kiLeDemande));
    }

    IEnumerator type(string phrase,DialogueCallback kiLeDemande){
        foreach(char c in phrase.ToCharArray()){
            texte.text += c;

            //On joue un son au pif
            int random = Mathf.RoundToInt(Random.Range(0, sons.Count + sons.Count));
            if(random < sons.Count) {
                AudioClip clip = sons[random];
                source.pitch = Random.Range(0.6f, 1f);
                source.Stop();
                source.PlayOneShot(clip);
            }

            yield return new WaitForSeconds(tempsEntreChaqueLettres);
        }

        kiLeDemande();
    }

    public void finirAffichage() {
        StopAllCoroutines();
        texte.text = phrasePrecedante;
        kiLeDemandePrecedant();
    }
}
