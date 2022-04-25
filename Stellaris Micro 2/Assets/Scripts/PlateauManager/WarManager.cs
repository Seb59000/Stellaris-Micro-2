using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;
using System;

[RequireComponent(typeof(RessourcesManager))]
[RequireComponent(typeof(GalaxieRenderer))]
[RequireComponent(typeof(EnnemiManager))]
public class WarManager : MonoBehaviour
{
    protected EnnemiManager ennemiManager;
    protected RessourcesManager ressourcesManager;
    protected GalaxieRenderer galaxieRenderer;
    public GameObject panelVictoireFinale;
    public GameObject panelDefaite;

    void Awake()
    {
        galaxieRenderer = GetComponent<GalaxieRenderer>();
        ennemiManager = GetComponent<EnnemiManager>();
        ressourcesManager = GetComponent<RessourcesManager>();
    }

    public void Guerre()
    {
        List<SSolEnGuerre> ssolDefaite = new List<SSolEnGuerre>();
        List<SSolEnGuerre> ssolVictoire = new List<SSolEnGuerre>();
        bool defaiteAttaquants = false;
        bool victoireAttaquants = false;

        // pour chaque systeme dans la liste des systemes en guerre
        // je decremente les forces et je sauvegarde les victoires et defaites
        foreach (SSolEnGuerre ssol in GameStatic.ssolEnGuerre)
        {
            // je retrouve ses infos
            InfoSystemeSolaire infos = GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS];
            // on retire -PUISSANCEDESTROUPES aux forces attaquants
            infos.forceEnnemies -= GameStatic.PUISSANCEDESTROUPES;
            // si il sont a zero on arrete et defaite
            if (infos.forceEnnemies == 0)
            {
                defaiteAttaquants = true;
                ssolDefaite.Add(ssol);
            }
            else
            { // sinon
                // si les troupes defensives == 0
                if (infos.forceAllies == 0)
                {
                    // on s'attaque à la base qu'on decremente
                    infos.forceBase -= GameStatic.PUISSANCEDESTROUPES;
                    // si force base ==0 
                    if (infos.forceBase == 0)
                    {
                        // victoire() 
                        victoireAttaquants = true;
                        ssolVictoire.Add(ssol);
                    }
                }
                else
                {
                    // on retire -1 aux troupesDefensives
                    infos.forceAllies -= GameStatic.PUISSANCEDESTROUPES;
                }
            }
            // je sauvegarde les modifs
            GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS] = infos;

            if (GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].isScanne)
            {
                // j'affiche les changements
                galaxieRenderer.MAJAffichageNbTroupesAlliees(ssol.localisation, infos.forceAllies);
                galaxieRenderer.MAJAffichageNbTroupesEnnemies(ssol.localisation, infos.forceEnnemies);
                galaxieRenderer.MAJAffichageForceBase(ssol.localisation, infos.forceBase);
            }
        }
        if (defaiteAttaquants)
        {
            MetFinGuerreDansSSolInfo(ssolDefaite);
            VerifYAEncore1GuerreDansGalaxie();

            MAJTroupesApresAttaqueRepoussee(ssolDefaite);
        }
        if (victoireAttaquants)
        {
            MetFinGuerreDansSSolInfo(ssolVictoire);
            VerifYAEncore1GuerreDansGalaxie();

            int forcesAllies = 0;
            foreach (SSolEnGuerre ssol in ssolVictoire)
            {
                forcesAllies = ChgtProprietaire(ssol);
            }

            DistinctionEntreIAEtJoueurVictoire(ssolVictoire, forcesAllies);
        }
    }

    private void DistinctionEntreIAEtJoueurVictoire(List<SSolEnGuerre> ssolVictoire, int forcesAllies)
    {
        foreach (SSolEnGuerre ssol in ssolVictoire)
        {
            if (ssol.codeAttaquant == 0)
            {
                // je met a jour les troupes du joueur dans la liste des troupes
                // MAJForceTroupeJoueur(ssol);
                SuppressionTroupeIA(ssol.localisation, ssol.codeDefenseur - 1);
                StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].enGuerre = false;
                StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordCible.Add(ssol.localisation);
            }
            else if (ssol.codeDefenseur == 0)
            {   //attaquant
                // MAJForceTroupeIA(ssol.localisation, ssol.codeAttaquant - 1);
                RetraitListeObjectifsCibles(ssol.localisation, ssol.codeAttaquant - 1);
                StaticEnnemi.listEnnemiData[ssol.codeAttaquant - 1].enGuerre = false;
                EnnemiManager.RemplissageListSsolAAColoniser(ssol.codeAttaquant - 1, ssol.localisation);

                //defenseur joueur
                SuppressionTroupeJoueur(ssol);
                galaxieRenderer.CacherFleche(ssol.localisation);
            }
            else
            {   //attaquant
                // MAJForceTroupeIA(ssol.localisation, ssol.codeAttaquant - 1);
                RetraitListeObjectifsCibles(ssol.localisation, ssol.codeAttaquant - 1);
                StaticEnnemi.listEnnemiData[ssol.codeAttaquant - 1].enGuerre = false;
                EnnemiManager.RemplissageListSsolAAColoniser(ssol.codeAttaquant - 1, ssol.localisation);

                //defenseur
                SuppressionTroupeIA(ssol.localisation, ssol.codeDefenseur - 1);
                StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].enGuerre = false;
                StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordCible.Add(ssol.localisation);
            }
            AffichageMAJ(ssol, forcesAllies);
        }
    }

    private void AffichageMAJ(SSolEnGuerre ssol, int forcesAllies)
    {
        if (GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].isScanne)
        {
            //Mise aux couleurs du joueur
            galaxieRenderer.AffichageCouleursEmpire(ssol.codeAttaquant + 1, ssol.localisation);

            // j'affiche les modifs
            galaxieRenderer.MAJAffichageNbTroupesAlliees(ssol.localisation, forcesAllies);
            galaxieRenderer.MAJAffichageNbTroupesEnnemies(ssol.localisation, 0);
            galaxieRenderer.MAJAffichageForceBase(ssol.localisation, GameStatic.PUISSANCEBASEDEBUT);
        }
    }

    private void RetraitListeObjectifsCibles(Coordonnees localisation, int indexIA)
    {
        int count = StaticEnnemi.listEnnemiData[indexIA].listCoordCible.Count;
        for (int i = 0; i < count; i++)
        {
            if (StaticEnnemi.listEnnemiData[indexIA].listCoordCible[i].indexLigneListSS == localisation.indexLigneListSS &&
                StaticEnnemi.listEnnemiData[indexIA].listCoordCible[i].indexColonneListSS == localisation.indexColonneListSS)
            {
                StaticEnnemi.listEnnemiData[indexIA].listCoordCible.RemoveAt(i);
                break;
            }
        }
    }
    /*
    private static void MAJForceTroupeIA(Coordonnees ssol, int indexEnnemi)
    {
        if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes.Count > 0)
        {
            // on met a jour les troupes def IA dans le ssol
            TroupeEnnemi crew = StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes[0];
            if (crew.localisation.indexLigneListSS == ssol.indexLigneListSS && crew.localisation.indexColonneListSS == ssol.indexColonneListSS)
            {
                StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes[0].nbTroupe = GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].forceAllies;
            }
        }
    }
    */
    private void SuppressionTroupeIA(Coordonnees ssol, int indexEnnemi)
    {
        if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes.Count > 0)
        {
            if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes[0].localisation.indexLigneListSS == ssol.indexLigneListSS &&
            StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes[0].localisation.indexColonneListSS == ssol.indexColonneListSS)
            {
                StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes.RemoveAt(0);
                if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordPossede.Count > 0)
                {
                    // creation troupe de 1 dans ssol natal
                    Coordonnees ssolNatal = StaticEnnemi.listEnnemiData[indexEnnemi].listCoordPossede[0];
                    StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes.Add(new TroupeEnnemi(1, ssolNatal));
                    StaticEnnemi.listEnnemiData[indexEnnemi].enDeplacementTroupe = false;
                    StaticEnnemi.listEnnemiData[indexEnnemi].enGuerre = false;
                    // MAJ force base
                    GameStatic.tabloInfosSystemeSolaire[ssolNatal.indexLigneListSS][ssolNatal.indexColonneListSS].forceAllies += 1;

                    if (GameStatic.tabloInfosSystemeSolaire[ssolNatal.indexLigneListSS][ssolNatal.indexColonneListSS].isScanne)
                    {
                        galaxieRenderer.MAJAffichageNbTroupesAlliees(ssolNatal, GameStatic.tabloInfosSystemeSolaire[ssolNatal.indexLigneListSS][ssolNatal.indexColonneListSS].forceAllies);
                    }
                }
                else
                {
                    // on rend l'ennemi inactif
                    StaticEnnemi.listEnnemiData[indexEnnemi].actif = false;

                    // on scan tous les ennemis
                    bool gagne = true;
                    foreach (EnnemiData data in StaticEnnemi.listEnnemiData)
                    {
                        // si il reste un ennemi actif 
                        if (data.actif)
                        {
                            // le joueur n'a pas gagné
                            gagne = false;
                        }
                    }
                    if (gagne)
                    {
                        // on a gagné
                        GameStatic.accelerateurTemps = 0;
                        panelVictoireFinale.SetActive(true);
                    }
                }
            }
        }
    }

    private int ChgtProprietaire(SSolEnGuerre ssol)
    {
        if (ssol.codeAttaquant == 0)
        {
            // ajout des ressources a la liste des ressourcesJoueur gagnant
            ressourcesManager.AjoutRessourcesSysteme(ssol.localisation);

            // MAJ ressources ia perdant
            ennemiManager.RetraitRessourcesSysteme(ssol.localisation, ssol.codeDefenseur - 1);

            GameStatic.ressourcesJoueur.ssolPossedes.Add(ssol.localisation);

            // on retire le ssol aux possessions du defenseur
            int count = StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede.Count;
            for (int i = 0; i < count; i++)
            {
                if (StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede[i].indexLigneListSS == ssol.localisation.indexLigneListSS &&
                    StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede[i].indexColonneListSS == ssol.localisation.indexColonneListSS)
                {
                    StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede.RemoveAt(i);
                    break;
                }
            }
        }
        else if (ssol.codeDefenseur == 0)
        {
            // MAJ ressources IA gagnant
            ennemiManager.AjoutRessourcesSysteme(ssol.localisation, ssol.codeAttaquant - 1);
            // on ajoute le ssol aux possessions
            StaticEnnemi.listEnnemiData[ssol.codeAttaquant - 1].listCoordPossede.Add(ssol.localisation);

            // MAJ ressources joueur perdant
            // retrait des ressources a la liste des ressourcesJoueur
            ressourcesManager.RetraitRessourcesSysteme(ssol.localisation);

            RetirerSsoldeListeSsolPossedes(ssol.localisation);

            if (GameStatic.ressourcesJoueur.ssolPossedes.Count == 0)
            {
                panelDefaite.SetActive(true);
                GameStatic.accelerateurTemps = 0;
            }
        }
        else
        {
            // MAJ ressources ia gagnant
            ennemiManager.AjoutRessourcesSysteme(ssol.localisation, ssol.codeAttaquant - 1);
            // on ajoute le ssol aux possessions
            StaticEnnemi.listEnnemiData[ssol.codeAttaquant - 1].listCoordPossede.Add(ssol.localisation);

            // MAJ ressources ia perdant
            ennemiManager.RetraitRessourcesSysteme(ssol.localisation, ssol.codeDefenseur - 1);

            // on retire le ssol aux possessions du defenseur
            int count = StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede.Count;
            for (int i = 0; i < count; i++)
            {
                if (StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede[i].indexLigneListSS == ssol.localisation.indexLigneListSS &&
                    StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede[i].indexColonneListSS == ssol.localisation.indexColonneListSS)
                {
                    StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].listCoordPossede.RemoveAt(i);
                    break;
                }
            }
        }
        // je change de propriétaire
        GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].proprietaire = ssol.codeAttaquant + 1;

        // je remplace les forces alliées par les forces ennemies
        int forcesAllies = GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].forceEnnemies;
        GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].forceAllies = forcesAllies;

        // je met les forces ennemies a zero
        GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].forceEnnemies = 0;

        // je repare la base
        GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].forceBase = GameStatic.PUISSANCEBASEDEBUT;
        /*
        // je lance la reparation de la base
        GameStatic.isBasesAReparer = true;
        GameStatic.basesAReparer.Add(ssol.localisation);
        */

        return forcesAllies;
    }

    private void RetirerSsoldeListeSsolPossedes(Coordonnees localisation)
    {
        for (int i = GameStatic.ressourcesJoueur.ssolPossedes.Count-1; i > -1; i--)
        {
            if (localisation.indexLigneListSS== GameStatic.ressourcesJoueur.ssolPossedes[i].indexLigneListSS &&
                localisation.indexColonneListSS == GameStatic.ressourcesJoueur.ssolPossedes[i].indexColonneListSS)
            {
                GameStatic.ressourcesJoueur.ssolPossedes.RemoveAt(i);
            }
        }
    }

    private void MAJForceTroupeJoueur(SSolEnGuerre ssol)
    {
        bool troupeTrouve = false;
        // on suppr toutes les troupes du joueur dans le ssol
        for (int i = GameStatic.listeTroupesAlliees.Count - 1; i > -1; i--)
        {
            if (GameStatic.listeTroupesAlliees[i].localisation.indexLigneListSS == ssol.localisation.indexLigneListSS && GameStatic.listeTroupesAlliees[i].localisation.indexColonneListSS == ssol.localisation.indexColonneListSS)
            {
                GameStatic.listeTroupesAlliees.RemoveAt(i);
                troupeTrouve = true;
            }
        }

        if (troupeTrouve)
        {
            // on crée une seule nouvelle crew en reprenant la force alliée du systeme ou elle se trouve
            Troupe newcrew = new Troupe(ssol.localisation, 0);
            GameStatic.listeTroupesAlliees.Add(newcrew);

            RemiseEnPlaceIndexListTroupesFinal();
        }
    }

    private void SuppressionTroupeJoueur(SSolEnGuerre ssol)
    {
        List<int> ListToRemove = new List<int>();
        // on recup l'index de toutes les troupes dans le systm
        int cptr = 0;
        foreach (Troupe troupe in GameStatic.listeTroupesAlliees)
        {
            // si la troupe est dans le systeme perdu
            if (troupe.localisation.indexLigneListSS == ssol.localisation.indexLigneListSS && troupe.localisation.indexColonneListSS == ssol.localisation.indexColonneListSS)
            {
                // on note l'index pour la detruire
                ListToRemove.Add(cptr);
            }
            cptr++;
        }

        //on supprime les troupes a tous les indexs trouvés
        for (int i = ListToRemove.Count - 1; i > -1; i--)
        {
            GameStatic.listeTroupesAlliees.RemoveAt(ListToRemove[i]);
        }

        // on remet les index de la liste des troupes en ordre
        RemiseEnPlaceIndexListTroupesFinal();
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

    private static void MetFinGuerreDansSSolInfo(List<SSolEnGuerre> ssols)
    {
        foreach (SSolEnGuerre ssol in ssols)
        {
            // je retire les ssol de la liste des ssol en guerre
            GameStatic.ssolEnGuerre.Remove(ssol);

            // sauvegarde systmInfo comme n'etant plus en guerre
            GameStatic.tabloInfosSystemeSolaire[ssol.localisation.indexLigneListSS][ssol.localisation.indexColonneListSS].enGuerre = false;
        }
    }

    private static void VerifYAEncore1GuerreDansGalaxie()
    {
        // si la liste des ssol en guerre est vide
        if (GameStatic.ssolEnGuerre.Count == 0)
        {
            //je met enGuerre = false; pour arreter les calculs
            GameStatic.enGuerre = false;
        }
    }

    private void MAJTroupesApresAttaqueRepoussee(List<SSolEnGuerre> ssolDefaite)
    {
        foreach (SSolEnGuerre ssol in ssolDefaite)
        {
            if (ssol.codeAttaquant == 0)
            {
                SuppressionTroupeJoueur(ssol);
                StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].enGuerre = false;
                // MAJForceTroupeIA(ssol.localisation, (ssol.codeDefenseur - 1));
            }
            else if (ssol.codeDefenseur == 0)
            {
                SuppressionTroupeIA(ssol.localisation, ssol.codeAttaquant - 1);
                StaticEnnemi.listEnnemiData[ssol.codeAttaquant - 1].enGuerre = false;
                //MAJForceTroupeJoueur(ssol);
            }
            else
            {
                SuppressionTroupeIA(ssol.localisation, ssol.codeAttaquant - 1);
                StaticEnnemi.listEnnemiData[ssol.codeAttaquant - 1].enGuerre = false;
                StaticEnnemi.listEnnemiData[ssol.codeDefenseur - 1].enGuerre = false;
                // MAJForceTroupeIA(ssol.localisation, (ssol.codeDefenseur - 1));
            }
        }
    }
}
