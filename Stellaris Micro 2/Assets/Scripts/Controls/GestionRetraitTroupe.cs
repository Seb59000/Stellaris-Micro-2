using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;
using System;

[RequireComponent(typeof(GalaxieRenderer))]
public class GestionRetraitTroupe : MonoBehaviour
{
    protected GalaxieRenderer galaxieRenderer;
    public GameObject panelRetraitTroupes;
    public float posXBtnsCaches = -4000f;
    public float posYBtnsCaches = 1200f;

    void Awake()
    {
        galaxieRenderer = GetComponent<GalaxieRenderer>();
    }

    public void RetraitTroupes()
    {
        // on recupere le ssol cliqué
        Coordonnees ssolClicked = GameStatic.ssolClicked;
        Troupe crew = TrouverTroupeDansSSol(ssolClicked);

        Coordonnees ssolDArrivee = GameStatic.ressourcesJoueur.localisationProductionVaisseau;
        Coordonnees localisation = crew.localisation;

        if (ContientDejaUneTroupe(ssolDArrivee))
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
            crew.localisation = ssolDArrivee;

            GameStatic.listeTroupesAlliees[GameStatic.troupeEnAttenteGuerre] = crew;
        }

        // si le ssol nous appartient
        if (GameStatic.tabloInfosSystemeSolaire[ssolClicked.indexLigneListSS][ssolClicked.indexColonneListSS].proprietaire == 1)
        {
            // je laisse le ssol en guerre
            // on MAJ les troupes dans ssol infos
            int forcesCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies;
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies = 0;

            //MAJ affichage troupesAllies dans panelInfosssol de départ
            galaxieRenderer.MAJAffichageNbTroupesAlliees(localisation, 0);

            // si le ssol d'arrivée est a nous
            if (GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].proprietaire == 1)
            {
                GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceAllies += forcesCrew;
                //MAJ affichage troupesAlliees dans panelInfosssol d arrivée
                galaxieRenderer.MAJAffichageNbTroupesAlliees(ssolDArrivee, GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceAllies);
            }
            else
            {
                GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceEnnemies += forcesCrew;
                //MAJ affichage troupesEnemies dans panelInfosssol d arrivée
                galaxieRenderer.MAJAffichageNbTroupesEnnemies(ssolDArrivee, GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceEnnemies);

                DeclarationGuerreSSol(ssolDArrivee);
            }
        }
        else
        {
            // je retire le ssol de la liste des ssol en guerre
            SuppressionSsolListeSSolEnguerre(ssolClicked);

            // on inverse les troupes dans ssol infos
            int forcesCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies;
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies = 0;

            //MAJ affichage troupesAllies dans panelInfosssol de départ
            galaxieRenderer.MAJAffichageNbTroupesEnnemies(localisation, 0);

            // si le ssol d'arrivée est a nous
            if (GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].proprietaire == 1)
            {
                GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceAllies += forcesCrew;
                //MAJ affichage troupesAlliees dans panelInfosssol d arrivée
                galaxieRenderer.MAJAffichageNbTroupesAlliees(ssolDArrivee, GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceAllies);
            }
            else
            { // si ssol ennemi
                GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceEnnemies += forcesCrew;
                //MAJ affichage troupesEnemies dans panelInfosssol d arrivée
                galaxieRenderer.MAJAffichageNbTroupesEnnemies(ssolDArrivee, GameStatic.tabloInfosSystemeSolaire[ssolDArrivee.indexLigneListSS][ssolDArrivee.indexColonneListSS].forceEnnemies);

                DeclarationGuerreSSol(ssolDArrivee);
            }
        }
        // on met la direction du systeme d'arrivée a zero
        galaxieRenderer.CacherFleche(ssolDArrivee);

        // je remet les btns d'action sur le coté
        panelRetraitTroupes.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je met le jeu en route
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }

    private static Troupe TrouverTroupeDansSSol(Coordonnees ssolClicked)
    {
        Troupe crew = new Troupe(ssolClicked, 0); ;

        // on recupere la troupe qui est dans le systeme
        foreach (Troupe troupe in GameStatic.listeTroupesAlliees)
        {
            // on recupere la troupe
            if (troupe.localisation.indexLigneListSS == ssolClicked.indexLigneListSS && troupe.localisation.indexColonneListSS == ssolClicked.indexColonneListSS)
            {
                crew = troupe;
            }
        }

        return crew;
    }

    private void DeclarationGuerreSSol(Coordonnees ssol)
    {
        // sauvegarde systmInfo comme etant en guerre
        GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].enGuerre = true;

        // je sauvegarde l'empire comme étant en guerre
        GameStatic.enGuerre = true;

        // je sauvegarde la localisation de la guerre
        GameStatic.ssolEnGuerre.Add(new SSolEnGuerre(ssol, 0, (GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].proprietaire - 1)));
    }

    private void SuppressionSsolListeSSolEnguerre(Coordonnees ssolClicked)
    {
        int indexSSol = 0;
        int cptr = 0;
        // on localise l'index du ssol en guerre dans sa  liste
        foreach (SSolEnGuerre ssol in GameStatic.ssolEnGuerre)
        {
            if (ssol.localisation.indexLigneListSS == ssolClicked.indexLigneListSS && ssol.localisation.indexColonneListSS == ssolClicked.indexColonneListSS)
            {
                // on sauvegarde l'index
                indexSSol = cptr;
            }
            cptr++;
        }
        // on le supprime
        GameStatic.ssolEnGuerre.RemoveAt(indexSSol);

        // si la liste des ssol en guerre est vide
        if (GameStatic.ssolEnGuerre.Count == 0)
        {
            //je met enGuerre = false; pour arreter les calculs
            GameStatic.enGuerre = false;
        }

        // je retire enguerre sur list infos ssol
        GameStatic.tabloInfosSystemeSolaire[ssolClicked.indexLigneListSS][ssolClicked.indexColonneListSS].enGuerre = false;
    }

    public void AnnulerRetraitTroupes()
    {
        // je remet le panelDivisionTroupes sur le coté
        panelRetraitTroupes.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je met le jeu en route
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
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
}
