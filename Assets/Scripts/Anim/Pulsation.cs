/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine;

//Fait pulser les objets
public class Pulsation : TimerListener
{
    public float dureePulsation = 0.1f;
    public float intensite = 0.1f;
    public float delai = 0.021f;
    public bool utiliseAnimator = false; //Si on doit le faire avec une animation plutôt qu'à la main
    private Animator animator;
    
    private float dureeRestante;
    private bool pulse = false;

    void Start() {
        if(utiliseAnimator)
            animator = GetComponent<Animator>();
    }

    void Update() {
        if(pulse && dureeRestante > 0) {
            dureeRestante -= Time.deltaTime;
        } else if (pulse) {
            pulse = false;
            transform.localScale -= Vector3.one * intensite;
        }
    }

    public override void timerEvent()
    {
        if(!utiliseAnimator) {
            pulse = true;
            dureeRestante = dureePulsation + delai;
            Invoke("scaleUp", delai);
        } else if (animator != null) {
            animator.SetTrigger("pulse");
        }
    }

    private void scaleUp() {
        transform.localScale += Vector3.one * intensite;
    }
}
