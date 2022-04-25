using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(GalaxieRenderer))]
public class GestionDecisionGuerre : MonoBehaviour
{
    protected GalaxieRenderer galaxieRenderer;
    public Material arrowMaterial;
    public Material transparentMaterial;
    public GameObject panelDecisionGuerre;
    public float posXBtnsCaches = -4000f;
    public float posYBtnsCaches = 1200f;

    void Awake()
    {
        galaxieRenderer = GetComponent<GalaxieRenderer>();
    }

    public void StartWar()
    {
        // sauvegarde systmInfo comme etant en guerre
        GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolEnAttenteGuerre.localisation.indexLigneListSS][GameStatic.ssolEnAttenteGuerre.localisation.indexColonneListSS].enGuerre = true;
        Troupe crew = GameStatic.listeTroupesAlliees[GameStatic.troupeEnAttenteGuerre];

        // je sauvegarde l'empire comme étant en guerre
        GameStatic.enGuerre = true;

        // je sauvegarde la localisation de la guerre
        GameStatic.ssolEnGuerre.Add(GameStatic.ssolEnAttenteGuerre);

        // je remet les btns sur le coté
        panelDecisionGuerre.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je refait partir le temps
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }

    public void RetraitTroupes()
    {
        // je remet les btns sur le coté
        panelDecisionGuerre.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // on recupere la troupe
        Troupe crew = GameStatic.listeTroupesAlliees[GameStatic.troupeEnAttenteGuerre];

        // on change la direction du systeme qui nous a amené ici
        GameStatic.tabloInfosSystemeSolaire[crew.systemeDepart.indexLigneListSS][crew.systemeDepart.indexColonneListSS].direction = new Coordonnees(crew.systemeDepart.indexLigneListSS, crew.systemeDepart.indexColonneListSS);
        // on cache la fleche
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[crew.systemeDepart.indexLigneListSS][crew.systemeDepart.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        allRendererChildren[4].material = transparentMaterial;

        Coordonnees ssolDepart = crew.systemeDepart;
        Coordonnees localisation = crew.localisation;

        if (ContientDejaUneTroupe(ssolDepart))
        {
            GameStatic.listeTroupesAlliees.RemoveAt(GameStatic.troupeEnAttenteGuerre);

            //on remet les index des troupes en place
            RemiseEnPlaceIndexListTroupesFinal();
        }
        else
        {
            // on change les marqueur d'entree de la troupe 
            crew.entree = true;
            crew.avancement = 4;

            // et son systeme de depart et d'arrivée
            crew.systemeDepart = localisation;
            crew.localisation = ssolDepart;

            GameStatic.listeTroupesAlliees[GameStatic.troupeEnAttenteGuerre] = crew;
        }

        // on inverse les troupes dans ssol infos
        int forcesCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies;
        GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies = 0;
        GameStatic.tabloInfosSystemeSolaire[ssolDepart.indexLigneListSS][ssolDepart.indexColonneListSS].forceAllies += forcesCrew;

        //MAJ affichage troupesAlliees dans panelInfosssol d arrivée
        galaxieRenderer.MAJAffichageNbTroupesAlliees(ssolDepart, GameStatic.tabloInfosSystemeSolaire[ssolDepart.indexLigneListSS][ssolDepart.indexColonneListSS].forceAllies);

        //MAJ affichage troupesEnnemies dans panelInfosssol de départ
        galaxieRenderer.MAJAffichageNbTroupesEnnemies(localisation, 0);

        // on relance le timer
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
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

    private void RemiseEnPlaceIndexListTroupesFinal()
    {
        int newIndex = 0;
        foreach (Troupe item in GameStatic.listeTroupesAlliees)
        {
            item.index = newIndex;
            newIndex++;
        }
    }
}
