using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using classDuJeu;
using System;
//using QPathFinder;

[RequireComponent(typeof(RessourcesManager))]
[RequireComponent(typeof(GalaxieRenderer))]
[RequireComponent(typeof(EnnemiManager))]
public class TroupeManager : MonoBehaviour
{
    protected EnnemiManager ennemiManager;
    protected RessourcesManager ressourcesManager;
    protected GalaxieRenderer galaxieRenderer;
    //public List<Material> listeCouleurs;
    public Material avancementTroupe1;
    public Material avancementTroupe2;
    public Material avancementTroupe3;
    public Material avancementTroupe4;
    public Material materialTransparent;

    public GameObject cameraPosition;
    public GameObject panelDecisionGuerre;

    void Awake()
    {
        ennemiManager = GetComponent<EnnemiManager>();
        ressourcesManager = GetComponent<RessourcesManager>();
        galaxieRenderer = GetComponent<GalaxieRenderer>();
    }

    public void ActualisationTroupesJoueur()
    {
        // l'avancée des troupes renvoie une liste de troupes a supprimer
        List<int> listeTroupesASuppr = AvanceeTroupesJoueur();

        if (listeTroupesASuppr.Count > 0)
        {
            SuppressionTroupes(listeTroupesASuppr);
        }
    }

    private void DeclarationGuerre(Coordonnees localisation)
    {
        // sauvegarde systmInfo comme etant en guerre
        GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].enGuerre = true;

        // je sauvegarde l'état de guerre
        GameStatic.enGuerre = true;

        // je recup le code ennemi
        int codeEnnemi = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].proprietaire - 1;

        // je sauvegarde la localisation de la guerre
        GameStatic.ssolEnGuerre.Add(new SSolEnGuerre(localisation, 0, codeEnnemi));
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

    private void SuppressionTroupes(List<int> listeTroupesFusion)
    {
        // on supprime les troupes a fusionner de la liste des troupes 
        foreach (int indexCrew in listeTroupesFusion)
        {
            GameStatic.listeTroupesAlliees.RemoveAt(indexCrew);
        }
        RemiseEnPlaceIndexListTroupesFinal();
    }

    private List<int> AvanceeTroupesJoueur()
    {
        List<int> listeTroupesASuppr = new List<int>();
        // pour chaque troupe du joueur
        int count = GameStatic.listeTroupesAlliees.Count;
        for (int i = count - 1; i > -1; i--)
        {
            Troupe crew = GameStatic.listeTroupesAlliees[i];
            // on verifie si on est entrain d'entrer
            if (crew.entree)
            {
                // on decremente l'avancement
                crew.avancement--;

                crew.entree = EntreeTroupes(crew);
            }
            // sinon on est en sortie ou stationnaire
            else
            {
                // on recupere la localisation du crew
                int ligne = crew.localisation.indexLigneListSS;
                int colonne = crew.localisation.indexColonneListSS;

                // on regarde si systeme est en guerre
                bool enGuerre = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].enGuerre;
                // s'il est pas en guerre on peut avancer
                if (!enGuerre)
                {
                    // on verifie que le systeme a ete scanné
                    bool isScanne = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].isScanne;
                    if (isScanne)
                    {
                        // on regarde la direction du ssol selectionné
                        Coordonnees direction = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].direction;

                        //si direction n'est pas celle du systeme lui meme (troupes postées) on bouge les troupes
                        if ((direction.indexLigneListSS != ligne || direction.indexColonneListSS != colonne))
                        {
                            // s'il est pas deja en guerre
                            if (!GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].enGuerre)
                            {
                                // on incremente l'avancement
                                crew.avancement++;
                                crew.entree = AnimationSortieTroupes(crew, direction);

                                // si on change de systeme
                                if (crew.entree)
                                {
                                    // s'il contient deja une troupe
                                    if (ContientDejaUneTroupe(direction))
                                    {
                                        listeTroupesASuppr.Add(crew.index);
                                    }
                                    else
                                    {
                                        //chgt systm dans GameStatic.coordonneesTroupesAlliees
                                        crew.localisation = direction;

                                        //sauvegarde du systeme de depart
                                        crew.systemeDepart = new Coordonnees(ligne, colonne);
                                    }
                                    // si  proprietaire=ennemi
                                    if (GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].proprietaire > 1)
                                    {
                                        TransfereTroupesSystemeEnnemi(ligne, colonne, direction);
                                    }
                                    else
                                    {
                                        TransfereTroupeSystemeNonHostile(ligne, colonne, direction);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ScanSSol(crew.localisation);
                        // crew.avancement--;
                    }
                }
            }
            //sauvegarde des modifs
            GameStatic.listeTroupesAlliees[i] = crew;
        }
        /*
        foreach (Troupe crew in GameStatic.listeTroupesAlliees)
        {
            // on verifie si on est entrain d'entrer
            if (crew.entree)
            {
                // on decremente l'avancement
                crew.avancement--;
                bool[] outPutFunction = new bool[2];

                outPutFunction = EntreeTroupes(crew);
                crew.entree = outPutFunction[0];

                // si on a besoin d'une fusion
                if (outPutFunction[1] == true)
                {
                    // on note l'index, les coordonnees du systeme et la force de la troupe a fusionner
                    listeTroupesFusion.Add(new OutilFusion(crew.localisation, crew.nbTroupe, crew.index));
                }
            }
            // sinon on est en sortie ou stationnaire
            else
            {
                // on recupere la localisation du crew
                int ligne = crew.localisation.indexLigneListSS;
                int colonne = crew.localisation.indexColonneListSS;

                // on regarde si systeme est en guerre
                bool enGuerre = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].enGuerre;
                // s'il est pas en guerre
                if (!enGuerre)
                {
                    // on verifie que le systeme a ete scanné
                    bool isScanne = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].isScanne;
                    if (isScanne)
                    {
                        // on regarde la direction du ssol selectionné
                        Coordonnees direction = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].direction;

                        //si direction n'est pas celle du systeme lui meme (troupes postées) on bouge les troupes
                        if ((direction.indexLigneListSS != ligne || direction.indexColonneListSS != colonne))
                        {
                            // on incremente l'avancement
                            crew.avancement++;
                            crew.entree = SortieTroupes(crew, direction);

                            // si on change de systeme
                            if (crew.entree)
                            {
                                if (crew.fuite)
                                {
                                    // on sauvegarde le nb de forces ennemies dans GameStatic.listInfosSystemeSolaire.systeme d'ou on vient
                                    // on retire nos forces du tot de forces ennemies du systeme
                                    int forceEnnemies = GameStatic.tabloInfosSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].forceEnnemies;
                                    forceEnnemies = forceEnnemies - crew.nbTroupe;
                                    GameStatic.tabloInfosSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].forceEnnemies = forceEnnemies;

                                    //disparition affichage nbtroupe dans panelInfosssol de départ
                                    galaxieRenderer.MAJAffichageNbTroupesEnnemies(crew.systemeDepart, forceEnnemies);

                                    // je cache la fleche dans le systeme qu'on fuit
                                    galaxieRenderer.CacherFleche(crew.systemeDepart);
                                    crew.fuite = false;
                                }
                                else
                                {
                                    // on calcul et sauvegarde le nb de forces alliées dans GameStatic.listInfosSystemeSolaire.systeme d'ou on vient
                                    // on retire nos forces du tot de forces alliées du systeme
                                    int forcesAlliees = GameStatic.tabloInfosSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].forceAllies;
                                    forcesAlliees = forcesAlliees - crew.nbTroupe;
                                    GameStatic.tabloInfosSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].forceAllies = forcesAlliees;

                                    //disparition affichage troupesAlliees dans panelInfosssol de départ
                                    galaxieRenderer.MAJAffichageNbTroupesAlliees(crew.localisation, forcesAlliees);
                                }

                                //chgt systm dans GameStatic.coordonneesTroupesAlliees
                                crew.localisation = direction;

                                //sauvegarde du systeme de depart
                                crew.systemeDepart = new Coordonnees(ligne, colonne);
                            }
                        }
                    }
                    else
                    {
                        ScanSSol(crew.localisation, crew.index, crew.nbTroupe);
                        // crew.avancement--;
                    }
                }
            }
        }
        */
        return listeTroupesASuppr;
    }

    private void TransfereTroupeSystemeNonHostile(int ligne, int colonne, Coordonnees direction)
    {
        // on transfer les troupes dans ssol infos
        int forcesAlliees = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies;
        GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies = 0;
        GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].forceAllies += forcesAlliees;

        //MAJ affichage troupesAlliees dans panelInfosssol de départ
        galaxieRenderer.MAJAffichageNbTroupesAlliees(new Coordonnees(ligne, colonne), 0);

        //MAJ affichage troupesAlliees dans panelInfosssol d arrivée
        galaxieRenderer.MAJAffichageNbTroupesAlliees(direction, GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].forceAllies);

        // s'il appartient a prsonn on se l'approprie
        if (GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].proprietaire == 0)
        {
            GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].proprietaire = 1;
        }
    }

    private void TransfereTroupesSystemeEnnemi(int ligne, int colonne, Coordonnees direction)
    {
        // on transfer les troupes dans ssol infos
        int forcesEnnemi = GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies;
        GameStatic.tabloInfosSystemeSolaire[ligne][colonne].forceAllies = 0;
        GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].forceEnnemies += forcesEnnemi;

        //MAJ affichage troupesAlliees dans panelInfosssol de départ
        galaxieRenderer.MAJAffichageNbTroupesAlliees(new Coordonnees(ligne, colonne), 0);

        //MAJ affichage troupesAlliees dans panelInfosssol d arrivée
        galaxieRenderer.MAJAffichageNbTroupesEnnemies(direction, GameStatic.tabloInfosSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].forceEnnemies);
    }

    private void ScanSSol(Coordonnees localisation)
    {
        // on recupere l'avancée du scan puis on incrémente sur Static save
        int avanceeScan = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].avanceeScan++;

        // selon l'avancée
        switch (avanceeScan)
        {
            case 0:
                // sauvegarde appropriation systeme par joueur
                GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].proprietaire = 1;
                // ajout aux possessions
                GameStatic.ressourcesJoueur.ssolPossedes.Add(localisation);
                break;
            case 1:

                break;
            case 2:
                //augmentation force base a 3
                GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceBase = 1;
                galaxieRenderer.MAJAffichageForceBase(localisation, 1);

                break;
            case 3:
                //apparition des ressources
                galaxieRenderer.AffichageRessources(localisation.indexLigneListSS, localisation.indexColonneListSS);
                break;
            case 4:
                // affichage connexions visibles et systemes atteignable
                galaxieRenderer.AfficherConnexions(localisation);
                break;
            case 5:
                galaxieRenderer.AffichageCouleursEmpire(1, localisation);
                //augmentation force base a PUISSANCEBASEDEBUT
                galaxieRenderer.MAJAffichageForceBase(localisation, GameStatic.PUISSANCEBASEDEBUT);
                GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].forceBase = GameStatic.PUISSANCEBASEDEBUT;

                // ajout des ressources a la liste des ressourcesJoueur
                ressourcesManager.AjoutRessourcesSysteme(localisation);

                // on le marque comme scanné
                GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].isScanne = true;
                break;
            default:
                break;
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

    private bool EntreeTroupes(Troupe crew)
    {
        bool entree = true;
        //on recupere le ssol physique
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();

        switch (crew.avancement)
        {
            case 4:
                //on montre le petit point qui est en position 4
                allRendererChildren[5].material = avancementTroupe4;

                // on l'oriente vers le ssol visé
                allRendererChildren[5].transform.LookAt(GameStatic.tableauSystemeSolaire[crew.systemeDepart.indexLigneListSS][crew.systemeDepart.indexColonneListSS].gameObject.transform.position);
                break;
            case 3:
                //on montre le petit point qui est en position 3
                allRendererChildren[5].material = avancementTroupe3;
                break;
            case 2://on montre le petit point qui est en position 2
                allRendererChildren[5].material = avancementTroupe2;
                break;
            case 1://on montre le petit point qui est en position 1
                allRendererChildren[5].material = avancementTroupe1;
                break;
            case 0://on est au centre du systeme
                allRendererChildren[5].material = materialTransparent;

                // si le systeme appartient deja a qqn d'autre
                int proprietaire = GameStatic.tabloInfosSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].proprietaire;
                if (proprietaire > 1)
                {
                    bool isScanne = GameStatic.tabloInfosSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].isScanne;
                    EntreeSystemeOccupe(crew.localisation, crew.index, proprietaire, isScanne);
                }
                // on signale que l'escouade est arrivée au centre du systeme
                entree = false;
                break;
            default:
                break;
        }
        return entree;
    }

    void EntreeSystemeOccupe(Coordonnees localisation, int indexTroupe, int proprietaire, bool isScanne)
    {
        // si il es pas scanné↓
        if (isScanne == false)
        {
            // on met le marqueur d'entree a false pour la troupe
            GameStatic.listeTroupesAlliees[indexTroupe].entree = false;

            // on marque le systeme comme scanné
            GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].isScanne = true;

            // affichage connexions visibles et systemes atteignable
            galaxieRenderer.AfficherConnexions(localisation);

            // on affiche les ressources et les forces du ssol
            AffichageRessourcesEnnemi(localisation.indexLigneListSS, localisation.indexColonneListSS, indexTroupe);

            galaxieRenderer.AffichageCouleursEmpire(proprietaire, localisation);

            // pour dialogue avec boite decision
            // on met en pause
            GameStatic.accelerateurTemps = 0;

            // je sauvegarde l'index de la troupe 
            GameStatic.troupeEnAttenteGuerre = indexTroupe;

            // je sauvegarde les coordonnées du systeme en attente 
            GameStatic.ssolEnAttenteGuerre = new SSolEnGuerre(localisation, 0, (proprietaire - 1));

            // on recupere la position du ssol a viser
            Vector3 posSsol = GameStatic.tableauSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].transform.position;

            // on met la camera au dessus du point visé
            cameraPosition.transform.position = new Vector3(posSsol.x, cameraPosition.transform.position.y, posSsol.z);

            // j'affiche la boite de dialogue ?decision guerre
            panelDecisionGuerre.gameObject.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        }
        else
        {
            // on met le marqueur d'entree a false pour la troupe
            GameStatic.listeTroupesAlliees[indexTroupe].entree = false;

            // si le systeme est pas deja en guerre
            if (!GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].enGuerre)
            {
                // on declare la guerre
                DeclarationGuerre(localisation);
            }
            /*
            else
            { // c'est qu'il y a deja qqn entrain de faire la guerre
                Debug.Log("a implementer");
            }
            */
        }
    }

    void AffichageRessourcesEnnemi(int indexLigneTraite, int indexColonneTraite, int indexTroupe)
    {
        // recuperation du panel info a MAJ
        GameObject panelInfo = GameStatic.panelTomove[indexLigneTraite][indexColonneTraite].gameObject;

        // panels forces militaires etc...
        Image[] allImgChildren = panelInfo.transform.GetComponentsInChildren<Image>();
        Text[] allTextChildren = panelInfo.transform.GetComponentsInChildren<Text>();

        // on montre nos troupes dans le carré rouge !!carré ennemi
        allImgChildren[1].enabled = true;
        allTextChildren[0].enabled = true;
        allTextChildren[0].text = GameStatic.tabloInfosSystemeSolaire[indexLigneTraite][indexColonneTraite].forceEnnemies.ToString();
        galaxieRenderer.AffichageRessources(indexLigneTraite, indexColonneTraite);
    }

    private bool AnimationSortieTroupes(Troupe crew, Coordonnees direction)
    {
        bool entree = false;
        //on recupere le ssol physique
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[crew.localisation.indexLigneListSS][crew.localisation.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();

        switch (crew.avancement)
        {
            case 1:
                //on montre le petit point qui avance de 1
                allRendererChildren[5].material = avancementTroupe1;

                // on l'oriente vers le ssol visé
                allRendererChildren[5].transform.LookAt(GameStatic.tableauSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].gameObject.transform.position);
                break;
            case 2:
                //on montre le petit point qui avance en 2
                allRendererChildren[5].material = avancementTroupe2;

                // on l'oriente vers le ssol visé
                allRendererChildren[5].transform.LookAt(GameStatic.tableauSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].gameObject.transform.position);
                break;
            case 3://on montre le petit point qui avance en 3
                allRendererChildren[5].material = avancementTroupe3;

                // on l'oriente vers le ssol visé
                allRendererChildren[5].transform.LookAt(GameStatic.tableauSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].gameObject.transform.position);
                break;
            case 4://on montre le petit point qui avance en 4
                allRendererChildren[5].material = avancementTroupe4;

                // on l'oriente vers le ssol visé
                allRendererChildren[5].transform.LookAt(GameStatic.tableauSystemeSolaire[direction.indexLigneListSS][direction.indexColonneListSS].gameObject.transform.position);
                break;
            case 5://on change de systeme
                allRendererChildren[5].material = materialTransparent;
                entree = true;
                break;
            default:
                break;
        }
        return entree;
    }
}
