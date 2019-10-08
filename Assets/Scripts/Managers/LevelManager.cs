/* #################################################################
 * ####      Written by A.Delecroix for the Ludum Dare 45       ####
 * ####                                                         ####
 * ####          Copyrighted and licensed under the             ####
 * ####            GNU General Public License v3.0              ####
 * #################################################################
 */

using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class CartonEvent : UnityEvent<Vector2> {} //Classe des événements carton
public class CableEvent : UnityEvent {} //Event rafraichissement
public class PistonEvent : UnityEvent<Vector2, EDirection> {} //Event piston qui bouge
public class PistonBloqueEvent : UnityEvent<Vector2> {} //Event case bloquée piston

[ExecuteInEditMode]
public class LevelManager : MonoBehaviour
{
    public List<Level.coupleCarton> cartonsInitiaux = new List<Level.coupleCarton>();
    public Level.LevelData niveau;
    public CartonEvent eventCartonsBloque = new CartonEvent();
    public CableEvent eventCableRafraichissement =  new CableEvent();
    public PistonEvent eventPiston = new PistonEvent();
    public PistonBloqueEvent eventPistonBloque = new PistonBloqueEvent();

    
    // Start is called before the first frame update
    void Start()
    {
        initialiserCartons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        if(Application.isEditor) { //Cleanup level
            if(niveau == null)
                return;
                
            for(int i = 0; i < niveau.niveau.Count; i++) {
                niveau.niveau[i] = new Level.Cell {
                        block = null,
                        estEditable = niveau.niveau[i].estEditable,
                        direction = EDirection.Nope
                    };
            }
        }
    }

    public void initialiserCartons() {
        //On nettoie
        GameObject[] cartons = GameObject.FindGameObjectsWithTag("Carton");
        foreach (GameObject obj in cartons)
        {
            DestroyImmediate(obj);
        }

        //On en remet
        foreach (Level.coupleCarton c in cartonsInitiaux)
        {
            GameObject obj = Instantiate(c.prefabCarton, c.pos, Quaternion.identity);
            SnapToGrid snap = obj.GetComponent<SnapToGrid>();
            if(snap != null)
                snap.enabled = false;
        }
    }
}
