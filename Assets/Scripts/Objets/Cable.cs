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

public class Cable : TimerListener, Level.IBlockType
{
    protected bool charge=false;
    protected bool detruit=false;
    [SerializeField] protected GameObject effetElec;
    [SerializeField] GameObject effetCC;
    protected bool [] monAieu;
    protected bool [] voisin;
    protected bool[] provenanceElec;
    protected GameObject elcetriciteActuelle;
    public EDirection direction;
    protected Level.LevelData levelData;

    private bool estEdite = false;

    public virtual void Start() {
        LevelManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        levelData = manager.niveau;
        bool [] blablabla = {false, false, false, false};
        provenanceElec = blablabla;
    }

    void depart(){
        bool [] masque = {false,false,false,false};
        notifierCharge(masque);
    }

    public EDirection getDirection()
    {
        return direction;
    }

    public void setDirection(EDirection dir){
        direction=dir;
    }

    

    public void modeEdition(bool etat)
    {
        estEdite = etat;
    }

    public void nextDirection()
    {
        if(++direction == EDirection.Nope) {
            direction = EDirection.Haut;
        }

        ActualiserRotation actu = GetComponent<ActualiserRotation>();
        if (actu != null)
            actu.rafraichir();
    }

    void propagerElectricite(){
        Vector2 pos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        //try {
        if(levelData.estDansNiveau(pos + Vector2.up)){
            Debug.Log("propager depuis " + pos);
            GameObject block = levelData.niveau[levelData.vector2index(pos + Vector2.up)].block;
            if (block != null) {
                Cable cablevoisin = block.GetComponent<Cable>();

                if(voisin[0] == true && (!provenanceElec[0] || block.GetComponent<Diode>() != null)){
                        bool [] masque = {false,true,false,false};
                        if (cablevoisin != null)
                            cablevoisin.notifierCharge(masque);
                    
                } else if (voisin[0]) { //On va transmettre à celui qui nous à indiquer le prochain coup
                    //On le prévient et on ne fait rien
                    Debug.Log("down");
                    gestionConflit();
                }
            }
        }

        if(levelData.estDansNiveau(pos + Vector2.down)){
            GameObject block = levelData.niveau[levelData.vector2index(pos + Vector2.down)].block;
            if (block != null) {

                Cable cablevoisin = block.GetComponent<Cable>();
                
                if(voisin[1]==true && (!provenanceElec[1] || block.GetComponent<Diode>() != null)){
                    bool [] masque = {true,false,false,false};
                    if (cablevoisin != null)
                        cablevoisin.notifierCharge(masque);                
                    
                } else if (voisin[1]) { //On va transmettre à celui qui nous à indiquer le prochain coup
                    //On le prévient et on ne fait rien
                    Debug.Log("up");
                    gestionConflit();
                }
            }
        }


        if(levelData.estDansNiveau(pos + Vector2.left)){
            GameObject block = levelData.niveau[levelData.vector2index(pos + Vector2.left)].block;
            if (block != null) {

                Cable cablevoisin = block.GetComponent<Cable>();
                
                if(voisin[2]==true && (!provenanceElec[2] || block.GetComponent<Diode>() != null)){
                    bool [] masque = {false,false,false,true};
                    if (cablevoisin != null)
                        cablevoisin.notifierCharge(masque);                
                    
                } else if (voisin[2]) { //On va transmettre à celui qui nous à indiquer le prochain coup
                    //On le prévient et on ne fait rien
                    Debug.Log("right");
                    gestionConflit();
                }
            }
        }

        
        if(levelData.estDansNiveau(pos + Vector2.right)){
            GameObject block = levelData.niveau[levelData.vector2index(pos + Vector2.right)].block;
            if (block != null) {

                Cable cablevoisin = block.GetComponent<Cable>();

                if(voisin[3]==true && (!provenanceElec[3] || block.GetComponent<Diode>() != null)){
                    bool [] masque = {false,false,true,false};
                    if (cablevoisin != null)
                        cablevoisin.notifierCharge(masque);                
                    
                } else if (voisin[3]) { //On va transmettre à celui qui nous à indiquer le prochain coup
                    //On le prévient et on ne fait rien
                    Debug.Log("left");
                    gestionConflit();
                }
            }
        }

        bool[] arr = {false, false, false, false};
        provenanceElec = arr;
        //} catch (System.IndexOutOfRangeException e) {
        //    Debug.LogError("Index out of range : " + e.StackTrace);
        //}
    }

    public override void timerEvent()
    {
        if(elcetriciteActuelle!=null){
            Destroy(elcetriciteActuelle);
        }
        if(detruit){
            if(elcetriciteActuelle!=null){
                Destroy(elcetriciteActuelle);
            }
            GameObject go = Instantiate(effetCC,transform.position,transform.rotation);
            Destroy(go,1f);
        }
        if(charge && !detruit){
            elcetriciteActuelle=Instantiate(effetElec,transform.position,transform.rotation);
            Invoke("propagerElectricite",0.001f);
        }
        detruit=false;
        charge=false;
        //utilite
        bool [] test={false,false,false,false};
        monAieu=test;
        //
    }

    public virtual void notifierCharge(bool [] robert){
        if(!charge){
            Vector2 pos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            Debug.Log("Notifié à la position " + pos);
            monAieu = (bool[]) robert.Clone();
            charge=true;
            monProchain();
        }else
        {
            detruit=true;
            for (int i=0; i<voisin.Length;i++){
                voisin[i] = false;
                monAieu[i] = false;
            }
            
        }

        for (int i = 0; i < robert.Length; i++)
                provenanceElec[i] = provenanceElec[i] | robert[i];
    }

    public virtual void monProchain(){
        voisin=(bool[]) GetComponent<TuileRender>().voisin.Clone();
        if (voisin == null)
            return;

        for (int i=0; i<monAieu.Length;i++){
            voisin[i] = voisin[i] && (!(monAieu[i]));
        }
    }
 
    public virtual void detruireCourant() { //Arrête toute propagation
        Destroy(elcetriciteActuelle);
        charge = false;
    }

    public void gestionConflit() {
        /*Vector2 posActuelle = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        for (int i = 0; i < voisin.Length; i++) {
            if (pos.Equals(posActuelle + Direction.dir2Vec(voisinALaCon(i)))) {
                provenanceElec[i] = false;
                Debug.Log("dir : " + voisinALaCon(i));
                int somme = 0;
                foreach(bool b in provenanceElec) {
                    if(b)
                        somme++;
                }

                if(somme == 1) {
                    detruit = false;
                    charge = true;
                } else if (somme == 0) {
                    detruit = false;
                    charge = false;
                } else {
                    charge = false;
                    detruit = true;
                }

                break;
            }

        }*/

        detruit = true;
        charge = false;
    }

    public EDirection voisinALaCon(int i) {
        switch (i) {
            case 0:
                return EDirection.Haut;
            case 1:
                return EDirection.Bas;
            case 2:
                return EDirection.Gauche;
            case 3:
                return EDirection.Droite;
            default:
                return EDirection.Nope;
        }
    }
}
