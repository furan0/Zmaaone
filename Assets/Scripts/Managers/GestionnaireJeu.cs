/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

public class GestionnaireJeu : MonoBehaviour
{
    public int scoreAObtenir = 5;
    [SerializeField]
    private int score = 0;

    private LevelManager levelManager;
    private SongManager songManager;
    private UIManager uiManager;
    private DialogueManager dialogueManager;

    public float tempsAttenteMort = 2.0f;

    public AudioClip sonVictoire;
    public AudioClip sonDefaite;
    private AudioSource source;

    public Texture2D cursor;

    private bool finDuNiveau = false; //Pour éviter de gagner puis de perdre dans la foulée

    void Start() {
        levelManager = GetComponent<LevelManager>();
        songManager = GetComponent<SongManager>();
        uiManager = GetComponent<UIManager>();
        dialogueManager = GetComponent<DialogueManager>();
        source = GetComponent<AudioSource>();

        if(cursor != null)
            Cursor.SetCursor(cursor, new Vector2(4, 8), CursorMode.ForceSoftware);

        //On lance les dialogues
        Invoke("startDialogue", 0.0001f);
    }

    void startDialogue() {
        dialogueManager.startDialogue(songManager.StartPlaying);
    }
    
    public void cubeRecupere()
    {
        score++;
        if(score >= scoreAObtenir)
            victoire();
    }

    public void mauvaisCube()
    {
        defaite();
    }

    private void defaite() {
        if(!finDuNiveau) {
            finDuNiveau = true;
            Debug.Log("Défaite !");
            uiManager.switchPanel(2);
            if(sonDefaite != null)
                source.PlayOneShot(sonDefaite);
            Invoke("stopNiveau", tempsAttenteMort);
        }
    }

    private void victoire() {
        if(!finDuNiveau) {
            finDuNiveau = true;
            Debug.Log("Victoire !");
            if(sonVictoire != null)
                source.PlayOneShot(sonVictoire);
            uiManager.switchPanel(1); //Affichage message victoire
            //Jouer de la musique/effet ici  
        }
    }

    public void stopNiveau() {
        uiManager.switchPanel(0);
        GameObject[] cartons = GameObject.FindGameObjectsWithTag("Carton");
        foreach (GameObject obj in cartons)
        {
            Destroy(obj);
        }

        GameObject[] cables = GameObject.FindGameObjectsWithTag("Cable");
        foreach (GameObject obj in cables)
        {
            obj.GetComponent<Cable>().detruireCourant();
        }

        //On arrête la musique
        songManager.StopPlaying();

        //On fait repoper les cartons
        levelManager.initialiserCartons();
        score = 0; //RaZ score
        finDuNiveau = false;
    }

    public void startNiveau() {
        songManager.StartPlaying();
    }

    public void invokDefaite() {
        stopNiveau();
        GetComponent<SelectionManager>().changeEditMode(true);
    }
}
