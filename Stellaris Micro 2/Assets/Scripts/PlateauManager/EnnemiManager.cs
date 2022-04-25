using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;
using System;

/*
namespace QPathFinder
{
*/
[RequireComponent(typeof(GalaxieRenderer))]
[RequireComponent(typeof(TroupeManager))]
public class EnnemiManager : MonoBehaviour
{
    protected TroupeManager troupeManager;
    protected GalaxieRenderer galaxieRenderer;

    public GameObject cameraPosition;
    public GameObject panelAlerteAttaque;

    public Material avancementTroupe1;
    public Material avancementTroupe2;
    public Material avancementTroupe3;
    public Material avancementTroupe4;
    public Material materialTransparent;

    void Awake()
    {
        troupeManager = GetComponent<TroupeManager>();
        galaxieRenderer = GetComponent<GalaxieRenderer>();
    }

    public void DeplacementEnnemi()
    {
        // pour chaque ennemi
        for (int bot = 0; bot < StaticEnnemi.listEnnemiData.Count; bot++)
        {
            if (StaticEnnemi.listEnnemiData[bot].actif)
            {
                if (StaticEnnemi.listEnnemiData[bot].logiqueDeGuerre)
                {
                    if (!StaticEnnemi.listEnnemiData[bot].enGuerre)
                    {
                        StaticEnnemi.listEnnemiData[bot].cptrDeplacementTroupesEnnemi++;

                        // si il y a un itineraire
                        if (StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire.listeCoordonnees.Count > 0)
                        {
                            AffichageAvanceeTroupe(bot);
                        }
                        else
                        {
                            ChoisirSsolAAttaquer(bot);
                        }
                    }
                }
                else
                {
                    // calcul jour de CaptureSSol
                    bool jourDeCaptureSsol = CalculJourCaptureSSol(StaticEnnemi.listEnnemiData[bot].cptrDeplacementEnnemi++);

                    if (jourDeCaptureSsol)
                    {
                        if (StaticEnnemi.listEnnemiData[bot].actif)
                        {
                            StaticEnnemi.listEnnemiData[bot].cptrDeplacementEnnemi = 0;
                            CaptureNouveauSysteme(bot);
                        }
                    }
                }
            }
        }
    }

    private static bool CalculJourCaptureSSol(int cptrDeplacementTroupesEnnemi)
    {
        bool jourDeCaptureSsol = false;
        if (cptrDeplacementTroupesEnnemi == StaticEnnemi.RAPIDITEDEPLACEMENTENNEMI)
        {
            jourDeCaptureSsol = true;
        }

        return jourDeCaptureSsol;
    }

    private void AffichageAvanceeTroupe(int bot)
    {
        // recup localisation troupe
        Coordonnees localisation = StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation;

        //on recupere le ssol physique ou est la troupe
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();

        // recup itineraire troupe
        Itineraire itiTroupe = StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire;

        Coordonnees ssolArrivee = itiTroupe.listeCoordonnees[0];

        //on recupere le ssol physique ou va la troupe
        MeshRenderer[] allRendererChildrenArrivee = GameStatic.tableauSystemeSolaire[ssolArrivee.indexLigneListSS][ssolArrivee.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();

        switch (StaticEnnemi.listEnnemiData[bot].cptrDeplacementTroupesEnnemi)
        {
            case 1:
                //on montre le petit point qui est en position 1
                allRendererChildren[6].material = avancementTroupe1;

                // on l'oriente vers le ssol visé
                allRendererChildren[6].transform.LookAt(GameStatic.tableauSystemeSolaire[ssolArrivee.indexLigneListSS][ssolArrivee.indexColonneListSS].gameObject.transform.position);
                break;
            case 2:
                //on montre le petit point qui est en position 2
                allRendererChildren[6].material = avancementTroupe2;
                break;
            case 3:
                //on montre le petit point qui est en position 3
                allRendererChildren[6].material = avancementTroupe3;
                break;
            case 4:
                //on montre le petit point qui est en position 4
                allRendererChildren[6].material = avancementTroupe4;
                break;
            case 5:
                // changemennt de ssol
                //on montre le materialTransparent
                allRendererChildren[6].material = materialTransparent;
                break;
            case 6:
                // on l'oriente vers le ssol visé
                allRendererChildrenArrivee[6].transform.LookAt(GameStatic.tableauSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].gameObject.transform.position);

                //on montre le petit point qui est en position 4 dans ssol arrivée
                allRendererChildrenArrivee[6].material = avancementTroupe4;
                break;
            case 7:
                //on montre le petit point qui est en position 3 dans ssol arrivée
                allRendererChildrenArrivee[6].material = avancementTroupe3;
                break;
            case 8:
                //on montre le petit point qui est en position 2 dans ssol arrivée
                allRendererChildrenArrivee[6].material = avancementTroupe2;
                break;
            case 9:
                //on montre le petit point qui est en position 1 dans ssol arrivée
                allRendererChildrenArrivee[6].material = avancementTroupe1;
                break;
            case 10:
                //on montre le materialTransparent
                allRendererChildrenArrivee[6].material = materialTransparent;

                StaticEnnemi.listEnnemiData[bot].cptrDeplacementTroupesEnnemi = 0;
                DeplacementTroupe(bot);
                break;
            default:
                break;
        }
    }

    private void RechercheDeCibleEtDeplacement(int bot)
    {
        //verif si on n'est pas attaqué?
        // si on est pas en guerre
        /*
        if (StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].enGuerre == false) 
        {*/
        // si on a deja un itineraire
        if (StaticEnnemi.listEnnemiData[bot].enDeplacementTroupe)
        {
            DeplacementTroupe(bot);
        }
        else
        {
            ChoisirSsolAAttaquer(bot);
            DeplacementTroupe(bot);
        }
        // }
    }

    private void DeplacementTroupe(int bot)
    {
        Coordonnees prochaineEscale = StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire.listeCoordonnees[0];
        StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire.listeCoordonnees.RemoveAt(0);

        if (GameStatic.tabloInfosSystemeSolaire[prochaineEscale.indexLigneListSS][prochaineEscale.indexColonneListSS].proprietaire == bot + 2)
        {
            ChangementDeSysteme(bot, StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation, prochaineEscale);
        }
        else if (GameStatic.tabloInfosSystemeSolaire[prochaineEscale.indexLigneListSS][prochaineEscale.indexColonneListSS].proprietaire == 0)
        {
            AppropriationSSol(bot, prochaineEscale);
            ChangementDeSysteme(bot, StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation, prochaineEscale);
        }
        else
        {
            ChangementDeSystemeEnnemi(bot, StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation, prochaineEscale);
        }

        // si l'itineraire est vide
        if (StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire.listeCoordonnees.Count == 0)
        {
            StaticEnnemi.listEnnemiData[bot].enDeplacementTroupe = false;
            ChoisirSsolAAttaquer(bot);
        }
    }

    /// <summary>
    /// on prend n'importe quel ssol detenu par le bot
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    private Coordonnees CalculProchaineEscaleAleatoire(int bot)
    {
        StaticEnnemi.listEnnemiData[bot].bloque++;
        if (StaticEnnemi.listEnnemiData[bot].bloque == 8)
        {
            StaticEnnemi.listEnnemiData[bot].bloque = 0;
            ChangementDeSystemeEnnemi(bot, StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation, StaticEnnemi.listEnnemiData[bot].listCoordCible[0]);
        }
        // on recup la liste des possessions
        List<Coordonnees> listSSol = StaticEnnemi.listEnnemiData[bot].listCoordPossede;
        int count = listSSol.Count;

        // on en selectionne un au hasard
        int alea = UnityEngine.Random.Range(0, count);

        return listSSol[alea];
    }

    private void ChoisirSsolAAttaquer(int bot)
    {
        // on recup forces de la troupe
        Coordonnees localisation = StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation;
        int forceTroupe = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies;

        if (forceTroupe > GameStatic.PUISSANCEBASEDEBUT)
        {
            if (StaticEnnemi.listEnnemiData[bot].listCoordCible.Count > 0)
            {
                // on choisi un systeme a attaquer
                // pour chaque ssol dans la liste des cibles
                foreach (Coordonnees ssol in StaticEnnemi.listEnnemiData[bot].listCoordCible)
                {
                    // On calcul la force de la base et des troupes alliées dans le systeme
                    int forceSsol = GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].forceBase + GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].forceAllies;

                    // si force base assez faible et pas deja en guerre
                    if (forceSsol < forceTroupe && !GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].enGuerre)
                    { // on en fait notre cible
                        StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].cible = ssol;
                        TrouverItineraire(bot, ssol);
                        break;
                    }
                }
            }
            else
            {
                StaticEnnemi.listEnnemiData[bot].actif = false;
            }
        }
    }

    public void TrouverItineraire(int bot, Coordonnees cible)
    {
        // on crée un trajet
        bool itiTrouve = DijkstraPathEnnemi.CalculItineraireTroupeEnnemi(StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation, cible, bot);
        if (itiTrouve)
        {
            StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire = DijkstraPathEnnemi.itineraire;
            StaticEnnemi.listEnnemiData[bot].enDeplacementTroupe = true;
        }
        else
        {
            StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].itineraire.listeCoordonnees.Add(CalculProchaineEscaleAleatoire(bot));
        }
    }

    private void ChangementDeSystemeEnnemi(int bot, Coordonnees localisation, Coordonnees ssolArrivee)
    {
        int forceCrew=0;
        //StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].enGuerre = true;
        StaticEnnemi.listEnnemiData[bot].enGuerre = true;
        // si le ssol de depart est allié
        if (GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].proprietaire==bot+2)
        {
            forceCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies;

            // on met a jour list infos ssol départ
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies = 0;
        }
        else
        {
            forceCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies;

            // on met a jour list infos ssol départ
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies = 0;
        }

        // on teleporte la troupe là bas
        StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation = ssolArrivee;

        // on met a jour list infos ssol arrivée
        GameStatic.tabloInfosSystemeSolaire[ssolArrivee.indexLigneListSS][ssolArrivee.indexColonneListSS].forceEnnemies += forceCrew;

        DeclarationDeGuerre(bot, ssolArrivee);

        if (GameStatic.tabloInfosSystemeSolaire[ssolArrivee.indexLigneListSS][ssolArrivee.indexColonneListSS].isScanne)
        {
        // affichage MAJ
            galaxieRenderer.MAJAffichageNbTroupesAlliees(localisation, 0);
            galaxieRenderer.MAJAffichageNbTroupesEnnemies(ssolArrivee, forceCrew);
        }
    }

    private void DeclarationDeGuerre(int bot, Coordonnees localisation)
    {
        // sauvegarde systmInfo comme etant en guerre
        GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].enGuerre = true;

        // je sauvegarde l'état de guerre
        GameStatic.enGuerre = true;

        // je recup le code ennemi
        int codeEnnemi = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].proprietaire - 1;

        // si l'ennemi est le joueur on affiche un message d'alerte
        if (codeEnnemi == 0)
        {
            // on recupere la position du ssol a viser
            Vector3 posSsol = GameStatic.tableauSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].transform.position;

            // on met la camera au dessus du point visé
            cameraPosition.transform.position = new Vector3(posSsol.x, cameraPosition.transform.position.y, posSsol.z);
            panelAlerteAttaque.SetActive(true);
            GameStatic.accelerateurTemps = 0;
        }

        // je sauvegarde la localisation de la guerre
        GameStatic.ssolEnGuerre.Add(new SSolEnGuerre(localisation, (bot + 1), codeEnnemi));
    }

    private void ChangementDeSysteme(int bot, Coordonnees localisation, Coordonnees ssolArrivee)
    {
        int forceCrew = 0;
        // si on quitte un systeme possedé
        if (GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].proprietaire == bot + 2)
        {
            forceCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies;

            // on met a jour list infos ssol départ
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceAllies = 0;

            if (GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].isScanne)
            {
                // affichage MAJ
                galaxieRenderer.MAJAffichageNbTroupesAlliees(localisation, 0);
            }
        }
        else
        {
            forceCrew = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies;
            // on met fin a la guerre
            MetFinGuerre(localisation);
            // on met a jour list infos ssol départ
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies = 0;

            if (GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].isScanne)
            {
                // affichage MAJ
                galaxieRenderer.MAJAffichageNbTroupesEnnemies(localisation, 0);
            }
        }

        // on met a jour list infos ssol arrivée
        GameStatic.tabloInfosSystemeSolaire[ssolArrivee.indexLigneListSS][ssolArrivee.indexColonneListSS].forceAllies += forceCrew;

        // on teleporte la troupe là bas
        StaticEnnemi.listEnnemiData[bot].listCoordTroupes[0].localisation = ssolArrivee;

        GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceEnnemies = 0;

        if(GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].isScanne)
        {
            // affichage MAJ
            galaxieRenderer.MAJAffichageNbTroupesAlliees(ssolArrivee, GameStatic.tabloInfosSystemeSolaire[ssolArrivee.indexLigneListSS][ssolArrivee.indexColonneListSS].forceAllies);
        }
    }

    private static void MetFinGuerre(Coordonnees ssol)
    {
        int count = GameStatic.ssolEnGuerre.Count; 
        for (int i = count - 1; i > -1; i--)
        {
            if (GameStatic.ssolEnGuerre[i].localisation.indexLigneListSS == ssol.indexLigneListSS && GameStatic.ssolEnGuerre[i].localisation.indexColonneListSS == ssol.indexColonneListSS)
            {
                GameStatic.ssolEnGuerre.RemoveAt(i);
            }
        }

        // si la lise est vide on met fin a guerre dans la galaxie
        if (GameStatic.ssolEnGuerre.Count == 0)
        {
            GameStatic.enGuerre = false;
        }

        GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].enGuerre = false;
    }

    private void CaptureNouveauSysteme(int indexEnnemi)
    {
        // on reverifie les proprietaires des ssol cibles colonisation
        MAJListSSolAColoniser(indexEnnemi);

        DispatchTroupes(indexEnnemi);
    }

    private void DispatchTroupes(int indexEnnemi)
    {
        Coordonnees ssol1 = StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes[0].localisation;
        int forceCrew = GameStatic.tabloInfosSystemeSolaire[ssol1.indexLigneListSS][ssol1.indexColonneListSS].forceAllies;

        // on colonise au rythme des troupes que l'on a au début
        for (int indexCrew = 0; indexCrew < forceCrew; indexCrew++)
        {
            // si plus de ssol a coloniser 
            if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser.Count == 0)
            {
                // on rentre dans la logique de guerre 
                StaticEnnemi.listEnnemiData[indexEnnemi].logiqueDeGuerre = true;
            }
            else
            // sinon on s'approprie le premier ssol sur la liste
            {
                Coordonnees ssol = StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser[0];
                // s'il appartient a personne
                if (GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].proprietaire == 0)
                {
                    AppropriationSSol(indexEnnemi, StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser[0]);

                    // on Remplie la ListSsol a coloniser avec les ssol a proximité
                    RemplissageListSsolAAColoniser(indexEnnemi, StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser[0]);

                    // on supprime le ssol de la liste des ssol a coloniser
                    StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser.RemoveAt(0);
                }
                else
                {
                    // on l'ajoute a la liste des cibles
                    StaticEnnemi.listEnnemiData[indexEnnemi].listCoordCible.Add(ssol);

                    // on supprime le ssol de la liste des ssol a coloniser
                    StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser.RemoveAt(0);
                }
            }
        }
    }

    /// <summary>
    /// on retire les systemes deja detenus (soit par le joueur soit par d'autres) 
    /// de la liste listCoordAColoniser
    /// </summary>
    /// <param name="indexEnnemi"></param>
    private void MAJListSSolAColoniser(int indexEnnemi)
    {
        List<int> indexTosuppr = new List<int>();
        int index = 0;
        foreach (Coordonnees ssol in StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser)
        {
            int proprietaire = GameStatic.tabloInfosSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].proprietaire;
            if (proprietaire > 0)
            {
                // on le note pour suppr de liste a coloniser
                indexTosuppr.Add(index);

                if (proprietaire != indexEnnemi + 2)
                {
                    // on le met dans la liste des systemes cibles(pour logique de guerre)
                    StaticEnnemi.listEnnemiData[indexEnnemi].listCoordCible.Add(ssol);
                }
            }
            index++;
        }

        for (int i = indexTosuppr.Count - 1; i >= 0; i--)
        {
            StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser.RemoveAt(indexTosuppr[i]);
        }
    }

    private void AppropriationSSol(int indexEnnemi, Coordonnees CoordAColoniser)
    {
        // on prend possession du systeme
        GameStatic.tabloInfosSystemeSolaire[CoordAColoniser.indexLigneListSS][CoordAColoniser.indexColonneListSS].proprietaire = indexEnnemi + 2;

        // on ajoute 
        StaticEnnemi.listEnnemiData[indexEnnemi].listCoordPossede.Add(CoordAColoniser);

        // on ajoute les ressources aux ressources de l'ennemi
        AjoutRessourcesSysteme(CoordAColoniser, indexEnnemi);

        // on met la force de la base a 5
        GameStatic.tabloInfosSystemeSolaire[CoordAColoniser.indexLigneListSS][CoordAColoniser.indexColonneListSS].forceBase = GameStatic.PUISSANCEBASEDEBUT;

        if (GameStatic.tabloInfosSystemeSolaire[CoordAColoniser.indexLigneListSS][CoordAColoniser.indexColonneListSS].isScanne)
        {
            galaxieRenderer.MAJAffichageForceBase(CoordAColoniser, GameStatic.PUISSANCEBASEDEBUT);
            galaxieRenderer.AffichageCouleursEmpire(StaticEnnemi.listEnnemiData[indexEnnemi].codeEnnemi, CoordAColoniser);
            galaxieRenderer.AfficherConnexions(CoordAColoniser);
        }

    }

    public void AjoutRessourcesSysteme(Coordonnees localisation, int indexEnnemi)
    {
        // on recupere la liste des ressources du ssol
        List<Ressource> ressList = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].listRessources;

        // pour chaque ressource
        foreach (Ressource ress in ressList)
        {
            switch (ress.codeRessource)
            {
                // si cest d l'energie
                case 0:
                    // je calcul et sauvegarde 
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supEnergie += ress.quantite;
                    break;
                // si minerai
                case 1:
                    // je calcul et sauvegarde 
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supMinerai += ress.quantite;
                    break;
                // si alliage
                case 2:
                    // je calcul et sauvegarde 
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supAlliage += ress.quantite;
                    break;
                // si science           
                case 3:
                    // je calcul et sauvegarde
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supScience = StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supScience + ress.quantite;
                    break;
                default:
                    break;
            }
        }
    }

    public void RetraitRessourcesSysteme(Coordonnees localisation, int indexEnnemi)
    {
        // on recupere la liste des ressources du ssol
        List<Ressource> ressList = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].listRessources;
        // EnnemiData data = StaticEnnemi.listEnnemiData[indexEnnemi];
        // pour chaque ressource
        foreach (Ressource ress in ressList)
        {
            switch (ress.codeRessource)
            {
                // si cest d l'energie
                case 0:
                    // je calcul et sauvegarde 
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supEnergie -= ress.quantite;
                    break;
                // si minerai
                case 1:
                    // je calcul et sauvegarde 
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supMinerai -= ress.quantite;
                    break;
                // si alliage
                case 2:
                    // je calcul et sauvegarde 
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supAlliage -= ress.quantite;
                    break;
                // si science           
                case 3:
                    // je calcul et sauvegarde
                    StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supScience = StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.supScience - ress.quantite;
                    break;
                default:
                    break;
            }
        }

    }

    /// <summary>
    /// on remplie la liste de ssol a coloniser
    /// </summary>
    /// <param name="indexEnnemi"></param>
    /// <param name="ssolOrigine"></param>
    /// <returns></returns>
    public static void RemplissageListSsolAAColoniser(int indexEnnemi, Coordonnees ssolOrigine)
    {
        // on recupere les systemes accessibles
        List<Coordonnees> ssolAtteignables = GameStatic.tabloInfosNavigation[ssolOrigine.indexLigneListSS][ssolOrigine.indexColonneListSS].SSAtteignables;

        // avec on remplie la liste des AColoniser et celle des systemes cibles
        foreach (var ssolAtteignable in ssolAtteignables)
        {
            // on retire les systemes deja detenus (soit par le joueur soit par d'autres) 
            int proprietaire = GameStatic.tabloInfosSystemeSolaire[ssolAtteignable.indexLigneListSS][ssolAtteignable.indexColonneListSS].proprietaire;
            if (proprietaire > 0)
            {
                if (proprietaire != indexEnnemi + 2)
                {
                    // on les met dans la liste des systemes cibles(pour logique de guerre)
                    StaticEnnemi.listEnnemiData[indexEnnemi].listCoordCible.Add(ssolAtteignable);
                }
            }
            else
            {
                // sinon j'ajoute pour division ulterieure comme SSolAColoniser
                StaticEnnemi.listEnnemiData[indexEnnemi].listCoordAColoniser.Add(ssolAtteignable);
            }
        }
    }
    public static int CoordTabloToListIndex(Coordonnees ssol)
    {
        int indexList = (ssol.indexColonneListSS + (ssol.indexLigneListSS * GalaxieRenderer.nbColonnes)) + 1;

        return indexList;
    }
    public static Coordonnees IndexListToCoordTablo(int index)
    {
        int indexLigne = (index - 1) / GalaxieRenderer.nbColonnes;
        int indexCol = index - (indexLigne * GalaxieRenderer.nbColonnes) - 1;

        return new Coordonnees(indexLigne, indexCol);
    }
}
/*
}*/
