using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using classDuJeu;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(DijkstraPath))]
public class ClickSSolManager : MonoBehaviour
{
    protected DijkstraPath dijkstraPath;
    public int ligne = 1;

    public int colonne = 1;
    public Material redHoverMaterial;
    public Material hoverMaterial;
    public Material transparentMaterial;
    public Material arrowMaterial;

    private float ajustementx = 160f;
    private float ajustementy = 30f;

    public float posXBtnsCaches = -4000f;
    public float posYBtnsCaches = 1200f;
    void Awake()
    {
        dijkstraPath = GetComponent<DijkstraPath>();
    }

    void OnMouseOver()
    {
        // si je suis pas en mode edition sinon on pourrait changer le ssol cliqué
        if (!GameStatic.pauseModeEdition)
        {
            // si le systeme est en guerre
            if (GameStatic.tabloInfosSystemeSolaire[ligne][colonne].enGuerre)
            {
                // je montre le survol en changeant le material
                GetComponent<MeshRenderer>().material = hoverMaterial;

                //Onclick()
                if (Input.GetMouseButtonDown(0))
                {
                    RetraitTroupesEnGuerre();
                }
            }
            else
            {
                // je verifie que le ssol survolé est atteignable
                if (GameStatic.tabloInfosSystemeSolaire[ligne][colonne].isAtteignable)
                {
                    // et si j'en suis le propriétaire ou que je suis sur un deplacement ou une division de troupe
                    if (GameStatic.clickDeplacement || GameStatic.tabloInfosSystemeSolaire[ligne][colonne].proprietaire == 1 || GameStatic.divisionTroupe)
                    {
                        // je montre le survol en changeant le material
                        GetComponent<MeshRenderer>().material = hoverMaterial;

                        //Onclick()
                        if (Input.GetMouseButtonDown(0))
                        {
                            ClickDroit();
                        }
                        if (Input.GetMouseButtonDown(1))
                        {
                            ClickGauche();
                        }
                    }
                }
            }
        }
        //sinon je ne montre pas de survol
    }

    void CacherPanelDejaEnGuerre()
    {
        if (GameStatic.panelDejaEnGuerreDisplayed)
        {
            GameObject panelDejaEnGuerre;
            panelDejaEnGuerre = GameObject.FindGameObjectWithTag("PanelDejaEnGuerre");

            // on le cache
            //panelDejaEnGuerre.gameObject.SetActive(false);
            panelDejaEnGuerre.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

            GameStatic.panelDejaEnGuerreDisplayed = false;
        }
    }

    private void ClickDroit()
    {
        // si je suis sur une division de troupe et que je cherche un dernier ssol pour finaliser division
        if (GameStatic.divisionTroupe)
        {
            // s'il est en guerre ou appartient deja a qqn
            if (GameStatic.tabloInfosSystemeSolaire[ligne][colonne].enGuerre || GameStatic.tabloInfosSystemeSolaire[ligne][colonne].proprietaire > 1)
            {
                AffichageMessageEnGuerre();
            }
            else
            {
                GameStatic.clickDeplacement = false;

                DivisionTroupes();

                GameStatic.divisionTroupe = false;

                // je met le jeu en route
                GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
            }
        }
        else
        {
            // si on a deja cliqué sur un autre ssol avant 
            if (GameStatic.clickDeplacement)
            {
                GameStatic.clickDeplacement = false;

                GestionDeplacement();
            }
            // sinon
            else
            {
                GameStatic.clickDeplacement = true;

                // on sauvegarde les coordonnee du ssol cliqué 
                GameStatic.ssolClicked = new Coordonnees(ligne, colonne);
            }
        }
    }

    private void AffichageMessageEnGuerre()
    {
        // j'interdit et je montre un petit panel "deja en guerre"
        GameObject panelDejaEnGuerre = GameObject.FindGameObjectWithTag("PanelDejaEnGuerre");
        // sauvegarde position souris
        Vector3 mousePos = Input.mousePosition;
        //les met juste a cote du ssol cliqué
        //panelDejaEnGuerre.gameObject.SetActive(true);
        panelDejaEnGuerre.gameObject.transform.position = new Vector3(mousePos.x + ajustementx, mousePos.y + ajustementy, 0);

        GameStatic.panelDejaEnGuerreDisplayed = true;
    }

    private void RetraitTroupesEnGuerre()
    {
        GameStatic.clickDeplacement = false;

        // on sauvegarde les coordonnee du ssol cliqué 
        GameStatic.ssolClicked = new Coordonnees(ligne, colonne);

        // sauvegarde position souris
        Vector3 mousePos = Input.mousePosition;

        //recupere btns a afficher
        GameObject btnsActionsSSol;
        btnsActionsSSol = GameObject.FindGameObjectWithTag("BtnsRetraitTroupes");

        //les met juste a cote du ssol cliqué
        btnsActionsSSol.gameObject.transform.position = new Vector3(mousePos.x + ajustementx, mousePos.y + ajustementy, 0);
        // on montre le bouton retraitTroupes

        // je met le jeu en pause
        GameStatic.accelerateurTemps = 0;
    }

    private void ClickGauche()
    {
        // si on est sur la gestion d'un deplacement 
        if (GameStatic.clickDeplacement)
        {
            GestionDeplacement();
        }
        // sinon on affiche les boutons d'action
        else
        {
            GameStatic.clickDeplacement = false;

            // on sauvegarde les coordonnee du ssol cliqué 
            GameStatic.ssolClicked = new Coordonnees(ligne, colonne);

            AffichageBtnsAction();
        }
    }

    private void DivisionTroupes()
    {
        // si le systeme est atteignable depuis le ssolCliqué
        if (isAtteignable(GameStatic.ssolClicked, new Coordonnees(ligne, colonne)))
        {
            // on sauvegarde dans liste infos forceAllies systeme d'origine
            GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].forceAllies = GameStatic.forceTroupe2Divs;
            // on sauvegarde dans liste infos forceAllies systeme arrivée
            GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies += GameStatic.forceTroupe1Divs;

            // on affiche les MAJ sur les panels origine
            MAJAffichageNbTroupesAlliees(GameStatic.ssolClicked, GameStatic.forceTroupe2Divs);
            // sur les panels d arrivée
            MAJAffichageNbTroupesAlliees(new Coordonnees(ligne, colonne), GameStatic.forceTroupe1Divs);

            // s'il ne contient pas deja de troupe
            if (ContientDejaUneTroupe(new Coordonnees(ligne, colonne)))
            {
                MAJAffichageNbTroupesAlliees(new Coordonnees(ligne, colonne), GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies);
            }
            else
            {
                // on crée une nouvelle troupe dans le systeme d'apres (celui qui vient d'etre cliqué)
                GameStatic.listeTroupesAlliees.Add(new Troupe(new Coordonnees(ligne, colonne), GameStatic.ssolClicked, 5));

                //on remet les index des troupes en place
                RemiseEnPlaceIndexListTroupesFinal();
            }

            CacherFleche(new Coordonnees(ligne, colonne));

            // et on verifie aussi que le systeme d'arrivé ne renvoie pas vers le ssol d'origine
            Coordonnees ctemp = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].direction;
            // si c'est le cas
            if (ctemp.indexLigneListSS == GameStatic.ssolClicked.indexLigneListSS && ctemp.indexColonneListSS == GameStatic.ssolClicked.indexColonneListSS)
            {
                // on met la direction du systeme d'arrivée a zero
                // on change la direction sur le systeme origine du tableau de sauvegardeStatic
                GameStatic.tabloInfosSystemeSolaire[ligne][colonne].direction = new Coordonnees(ligne, colonne);
            }
        }
    }

    public bool ContientDejaUneTroupe(Coordonnees ssol)
    {
        bool retour = false;
        foreach (Troupe crew in GameStatic.listeTroupesAlliees)
        {
            if (crew.localisation.indexLigneListSS == ssol.indexLigneListSS && crew.localisation.indexColonneListSS == ssol.indexColonneListSS)
            {
                retour = true;
            }
        }
        return retour;
    }

    public void MAJAffichageNbTroupesAlliees(Coordonnees localisation, int nbTroupe)
    {
        //on recupere le panel concernes
        Transform panel = GameStatic.panelTomove[localisation.indexLigneListSS][localisation.indexColonneListSS];

        // on recupere les infos a changer sur le panel
        Image[] allImgChildren = panel.transform.GetComponentsInChildren<Image>();
        Text[] allTextChildren = panel.transform.GetComponentsInChildren<Text>();

        //on les met a jour
        if (nbTroupe == 0)
        { // on cache
            allImgChildren[2].enabled = false;
            allTextChildren[1].text = nbTroupe.ToString();
            allTextChildren[1].enabled = false;
        }
        else
        {
            // si il reste des forces ennemies on met a jour
            allImgChildren[2].enabled = true;
            allTextChildren[1].enabled = true;
            allTextChildren[1].text = nbTroupe.ToString();
        }
    }

    private void RemiseEnPlaceIndexListTroupesFinal()
    {
        int newIndex = 0;
        foreach (Troupe item in GameStatic.listeTroupesAlliees)
        {
            item.index = newIndex;
            newIndex++;
        }
    }

    private void AffichageBtnsAction()
    {
        // sauvegarde position souris
        Vector3 mousePos = Input.mousePosition;

        //recupere btns a afficher
        GameObject btnsActionsSSol;
        btnsActionsSSol = GameObject.FindGameObjectWithTag("BtnsActionsSSol");

        Button[] btns = btnsActionsSSol.transform.GetComponentsInChildren<Button>(true);

        // si il y a des forces alliees dans le systeme
        if (GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies > 1)
        {
            // on rajoute le bouton diviser les troupes 
            btns[1].gameObject.SetActive(true);
        }
        else
        { // sinon on le cache
            btns[1].gameObject.SetActive(false);
        }

        //les met juste a cote du ssol cliqué
        btnsActionsSSol.gameObject.transform.position = new Vector3(mousePos.x + ajustementx, mousePos.y + ajustementy, 0);
    }

    ///<summary>on change la direction sur liste des infos SSol</summary>
    private void GestionDeplacement()
    {
        // on prend les coordonnées des systemes dont il faut changer la direction
        Coordonnees SSolOrigine = GameStatic.ssolClicked;
        Coordonnees SSolArrivee = new Coordonnees(ligne, colonne);

        // on verif que syst d'origine n'est pas le syst d'arrivee
        // si c'est le cas 
        if (SSolOrigine.indexLigneListSS == ligne && SSolOrigine.indexColonneListSS == colonne)
        {
            // on cache la fleche
            CacherFleche(SSolOrigine);
        }
        else
        {
            // on verifie que il est atteignable depuis le ssol d'origine
            if (isAtteignable(SSolOrigine, SSolArrivee))
            {
                // on affiche la fleche
                AffichageFleche(SSolOrigine, SSolArrivee);

                // on change la direction sur le systeme origine du tableau de sauvegardeStatic
                GameStatic.tabloInfosSystemeSolaire[SSolOrigine.indexLigneListSS][SSolOrigine.indexColonneListSS].direction = SSolArrivee;
            }
            else
            {
                // si il n'est pas atteignable directement on calcul l'itinéraire le plus court
                dijkstraPath.CalculItineraireTroupe(SSolOrigine, SSolArrivee);
            }
        }
        // et on verifie aussi que le systeme d'arrivé ne renvoie pas vers le ssol d'origine
        Coordonnees ctemp = GameStatic.tabloInfosSystemeSolaire[SSolArrivee.indexLigneListSS][SSolArrivee.indexColonneListSS].direction;
        // si c'est le cas
        if (ctemp.indexLigneListSS == SSolOrigine.indexLigneListSS && ctemp.indexColonneListSS == SSolOrigine.indexColonneListSS)
        {
            // on cache la fleche qui pointait en contre sens
            CacherFleche(SSolArrivee);
            // on change la direction sur le systeme origine du tableau de sauvegardeStatic
            GameStatic.tabloInfosSystemeSolaire[SSolArrivee.indexLigneListSS][SSolArrivee.indexColonneListSS].direction = SSolArrivee;
        }

        // on remet le ssol hover du premier systeme en transparent
        MeshRenderer[] list = GameStatic.tableauSystemeSolaire[SSolOrigine.indexLigneListSS][SSolOrigine.indexColonneListSS].GetComponentsInChildren<MeshRenderer>();
        list[2].material = transparentMaterial;
    }

    private static List<Itineraire> InitialisationListeItiPossibles(Coordonnees sSolOrigine)
    {
        // je crée un Itineraire avec la coordonnée de départ
        Itineraire itineraire = new Itineraire();
        itineraire.listeCoordonnees.Add(sSolOrigine);

        // je l'ajoute a la liste des itineraires possibles
        List<Itineraire> listItineraireFinale = new List<Itineraire>();
        listItineraireFinale.Add(itineraire);
        return listItineraireFinale;
    }
    private bool isAtteignable(Coordonnees sSolOrigine, Coordonnees sSolArrivee)
    {
        bool isAtteignable = false;
        // on recupere les infosSsol
        List<Coordonnees> listSsolAtteignables = GameStatic.tabloInfosNavigation[sSolOrigine.indexLigneListSS][sSolOrigine.indexColonneListSS].SSAtteignables;
        // on passe en revue la liste des systemes atteignables
        foreach (Coordonnees ssol in listSsolAtteignables)
        {
            // si les coordonnées de ssolArrivee correspondent a l'un d'entre eux
            if (ssol.indexLigneListSS == sSolArrivee.indexLigneListSS && ssol.indexColonneListSS == sSolArrivee.indexColonneListSS)
            {
                // on passe isAtteignable à true;
                isAtteignable = true;
            }
        }
        return isAtteignable;
    }
    private void AffichageFleche(Coordonnees SSolOrigine, Coordonnees SSolArrivee)
    {
        // on affiche la fleche
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[SSolOrigine.indexLigneListSS][SSolOrigine.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        allRendererChildren[4].material = arrowMaterial;

        // on l'oriente vers le ssol visé
        allRendererChildren[4].transform.LookAt(GameStatic.tableauSystemeSolaire[SSolArrivee.indexLigneListSS][SSolArrivee.indexColonneListSS].gameObject.transform.position);
    }

    private void CacherFleche(Coordonnees SSol)
    {
        // on cache la fleche 
        MeshRenderer allRendererChildren = GameStatic.tableauSystemeSolaire[SSol.indexLigneListSS][SSol.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>()[4];
        allRendererChildren.material = transparentMaterial;

        // on met la direction du ssol a lui meme
        GameStatic.tabloInfosSystemeSolaire[SSol.indexLigneListSS][SSol.indexColonneListSS].direction = SSol;
    }

    void OnMouseExit()
    {
        // s'il a ete selected
        if (GameStatic.clickDeplacement && (GameStatic.ssolClicked.indexLigneListSS == ligne && GameStatic.ssolClicked.indexColonneListSS == colonne))
        {
            // on met le material en rouge
            GetComponent<MeshRenderer>().material = redHoverMaterial;
        }
        else
        {
            GetComponent<MeshRenderer>().material = transparentMaterial;

        }
        CacherPanelDejaEnGuerre();
    }
}
