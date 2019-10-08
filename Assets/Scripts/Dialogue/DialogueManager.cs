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

public delegate void DialogueCallback();

public class DialogueManager : MonoBehaviour
{
    public List<string> phrases;
    public DialogueTyper dialogueTyper;

    private bool dialogueEnCours = false;
    private bool tourne = false;
    private int ligneActuelle = 0;

    private DialogueCallback callback;

    //lance le dialogue et appel le callback lorsque c'est fini
    public void startDialogue(DialogueCallback callbackInput) {
        callback = callbackInput;

        if (phrases.Count > 0) {
            dialogueEnCours = true;
            tourne = true;
            dialogueTyper.afficher(phrases[0], nextPhrase);
        } else
            finDialogue();
    }

    public void nextPhrase() {
        if (!dialogueEnCours) {
            ligneActuelle++;
            dialogueEnCours = true;

            if (ligneActuelle < phrases.Count)
                dialogueTyper.afficher(phrases[ligneActuelle], nextPhrase);
            else
                finDialogue();

        } else {
            dialogueEnCours = false; //on attend l'input
        }
    }

    void Update() {
        if (tourne && Input.GetButtonUp("Destroy"))
        {
            if(dialogueEnCours)     //dialogue en cours, on affiche tout
                dialogueTyper.finirAffichage();
            else                    //dialogue fini, on passe à la phrase suivante
                nextPhrase();
        }
    }

    void finDialogue() {
        tourne = false;
        dialogueTyper.gameObject.SetActive(false);
        callback();
    }
}
