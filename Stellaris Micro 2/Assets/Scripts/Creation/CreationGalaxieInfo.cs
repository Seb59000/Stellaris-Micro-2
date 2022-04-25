using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;
using System;
//using QPathFinder;

[RequireComponent(typeof(EnnemiManager))]
public class CreationGalaxieInfo : MonoBehaviour
{
    const int NBRESSOURCES = 4;

    public float aleaNbRessources = 4f;
    public float aleaNbConnexions = 7f;
    public int nbEmpires = 4;

    /// <summary>
    /// Creation partie GameStatic.listInfosSystemeSolaire et  GameStatic.ressourcesJoueur
    /// </summary>
    public void CreationInfosCarte()
    {
        // on crée les InfosSystsolaire dans le tableau
        InitTableauxInfoSSol();

        // on cree les ressources dans le tableau des infossol
        CreationRessources();

        // On crée les connexions entre les SSol
        CreationConnexion();

        AttributionPlacesEmpires();
    }

    private void AttributionPlacesEmpires()
    {
        // A faire: une fonction qui prenne en entree un tableau d'adversaires
        AttributionAleatoirePlacesJoueurs();

        // l'empire du joueur est codé 1 en proprietaire.
        // 2,3,4 etc pour les autres joueurs. 
        // un syst non attribué est codé 0
        List<Coordonnees> placesEmpires = GameStatic.placesEmpires;

        CreationDatasJoueur(placesEmpires);

        // pour chaque empire j'attribue une place et réinitialise ses ressources
        for (int i = 0; i < placesEmpires.Count; i++)
        {
            ReinitialisationRessourcesSSol(placesEmpires[i], i + 1);
        }

        // Creation Ennemi Data
        CreationDatasIA(placesEmpires);
    }

    private void AttributionAleatoirePlacesJoueurs()
    {
        // écrire une fonction aleatoire
        GameStatic.placesEmpires = new List<Coordonnees>();
        GameStatic.placesEmpires.Add(new Coordonnees(1, 2));
        GameStatic.placesEmpires.Add(new Coordonnees(1, 6));
        GameStatic.placesEmpires.Add(new Coordonnees(4, 2));
        GameStatic.placesEmpires.Add(new Coordonnees(4, 6));
    }

    private static void CreationDatasJoueur(List<Coordonnees> placesEmpires)
    {
        // pour le joueur 
        InfoSystemeSolaire infos = GameStatic.tabloInfosSystemeSolaire[placesEmpires[0].indexLigneListSS][placesEmpires[0].indexColonneListSS];
        // je met son Syst solaire comme scanné/ et atteignable
        infos.isScanne = true;
        infos.isAtteignable = true;

        // je cree la planete du joueur
        Planete planet1 = new Planete(placesEmpires[0]);
        planet1.emplacement0 = 1; // usine d'alliage emplacement 0
        planet1.emplacement1 = 2; // centre de recherche dans le deuxieme emplacement
        infos.planet = planet1;

        // je MAJ nb ssolPossedés
        GameStatic.ressourcesJoueur.ssolPossedes.Add(placesEmpires[0]);

        // remplie forces base et Alliees
        infos.forceBase = (GameStatic.PUISSANCEBASEDEBUT + GameStatic.UPGRADEBASE);
        infos.forceAllies = GameStatic.NBFORCESALLIEESDEBUT;
        GameStatic.tabloInfosSystemeSolaire[placesEmpires[0].indexLigneListSS][placesEmpires[0].indexColonneListSS] = infos;
        GameStatic.listeTroupesAlliees.Add(new Troupe(placesEmpires[0], 0));

        // j'attibue 1 centre de recherche, une pdtion de vaisseaux et une usine d'alliage au ssol du joueur
        GameStatic.ressourcesJoueur.localisationCentreRecherche = placesEmpires[0];
        GameStatic.ressourcesJoueur.localisationUsineAlliage = placesEmpires[0];
        GameStatic.ressourcesJoueur.localisationProductionVaisseau = placesEmpires[0];
    }

    private void CreationDatasIA(List<Coordonnees> placesEmpires)
    {
        for (int i = 1; i < placesEmpires.Count; i++)
        {
        // Pour chaque ssol ennemi on crée une liste de data ennemi
            EnnemiData data = new EnnemiData(i + 1);

            // on y met le ssol possede
            data.listCoordPossede.Add(placesEmpires[i]);

            // pareil pour la liste ssol avec troupe
            data.listCoordTroupes.Add(new TroupeEnnemi(GameStatic.NBFORCESALLIEESDEBUT, placesEmpires[i]));
            StaticEnnemi.listEnnemiData.Add(data);

            // on Remplie la ListSsol a coloniser
            EnnemiManager.RemplissageListSsolAAColoniser(i - 1, placesEmpires[i]);
        }


    }

    private void ReinitialisationRessourcesSSol(Coordonnees placesEmpires, int proprietaire)
    {
        // on sauvegarde le code du priprietaire
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].proprietaire = proprietaire;
        /*
                Ressource ressource = new Ressource(0, 10);
                // j'ajoute la ressource a la liste des SSol
                GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].listRessources.Clear();
                GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].listRessources.Add(ressource);
        */

        //on efface ce qui etait deja rempli
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].listRessources.Clear();

        //on ajoute 10 d energie
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].listRessources.Add(new Ressource(0, 10));
        // 10 de minerai
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].listRessources.Add(new Ressource(1, 10));
        // 1 planete
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].listRessources.Add(new Ressource(4, 0));
        
        // 10 troupes 
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].forceAllies = GameStatic.NBFORCESALLIEESDEBUT;

        // 10 en force de base
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].forceBase = (GameStatic.PUISSANCEBASEDEBUT + GameStatic.UPGRADEBASE);

        // je met isPlanet a true
        GameStatic.tabloInfosSystemeSolaire[placesEmpires.indexLigneListSS][placesEmpires.indexColonneListSS].isPlanete = true;
    }

    void InitTableauxInfoSSol()
    {
        //Je crée les lignes puis les colonnes dans le tableau des infos systm solaires
        for (int i = 0; i < GalaxieRenderer.nbLignes; i++)
        {
            List<InfoSystemeSolaire> ligneSSol = new List<InfoSystemeSolaire>();
            // pour chaque ligne je remplie
            for (int j = 0; j < GalaxieRenderer.nbColonnes; j++)
            {
                ligneSSol.Add(new InfoSystemeSolaire(i, j));
            }
            GameStatic.tabloInfosSystemeSolaire.Add(ligneSSol);
        }
        //Je crée les lignes puis les colonnes dans le tableau des infos navigation
        for (int i = 0; i < GalaxieRenderer.nbLignes; i++)
        {
            List<InfosNavigation> ligneCoord = new List<InfosNavigation>();
            // pour chaque ligne je remplie
            for (int j = 0; j < GalaxieRenderer.nbColonnes; j++)
            {
                ligneCoord.Add(new InfosNavigation());
            }
            GameStatic.tabloInfosNavigation.Add(ligneCoord);
        }
        /*
        PathFinder.instance.graphData.nodes.Clear();

        //Je crée les nodes dans la liste des nodes
        int nbNodes = (GalaxieRenderer.nbLignes * GalaxieRenderer.nbColonnes);
        for (int i = 0; i < nbNodes; i++)
        {
            PathFinder.instance.graphData.nodes.Add(new Node(new Vector3(0, 0, 0)));
        }
        */
    }

    void CreationRessources()
    {
        foreach (List<InfoSystemeSolaire> listSSol in GameStatic.tabloInfosSystemeSolaire)
        {
            foreach (InfoSystemeSolaire SSol in listSSol)
            {
                // on choisi combien de ressources differentes on y place
                float nbRessources = UnityEngine.Random.Range(2, aleaNbRessources);

                List<Ressource> listRess = new List<Ressource>();
                for (int i = 1; i < nbRessources; i++)
                {
                    // on choisi quelle ressources va être ajouté
                    int codeRessource = UnityEngine.Random.Range(0, NBRESSOURCES);
                    // et en quelle quantite
                    int[] result = CalculQuantiteRessource(codeRessource);
                    int quantite = result[0];
                    Ressource ressource = new Ressource(codeRessource, quantite);

                    // j'ajoute la ressource a la liste des SSol
                    listRess.Add(ressource);
                    // si c'est une planete
                    if (result[1] == 1)
                    {
                        //Je met isPlanet a true
                        GameStatic.tabloInfosSystemeSolaire[SSol.indexLigneListSS][SSol.indexColonneListSS].isPlanete = true;
                    }
                }
                // je gere les doublons
                listRess = GestionDoublonsListRess(listRess);

                // j'ajoute la ressource a la liste des SSol
                GameStatic.tabloInfosSystemeSolaire[SSol.indexLigneListSS][SSol.indexColonneListSS].listRessources = listRess;
            }
        }
    }

    private List<Ressource> GestionDoublonsListRess(List<Ressource> listRess)
    {
        List<Ressource> listFinale = new List<Ressource>();
        // je remplie le tableau de 4 lignes (les 4 ressources plus plus tard planete)
        for (int i = 0; i < NBRESSOURCES; i++)
        {
            listFinale.Add(new Ressource(i, 0));
        }

        // on remplie le tableau vide avec listRess
        foreach (Ressource ress in listRess)
        {
            // on recupere le code ressource indiquant la ligne dans laquelle on va travailler
            // on ajoute la quantité de la ressource dans la bonne ligne du tableau final
            listFinale[ress.codeRessource].quantite += ress.quantite;
        }

        // on retire les lignes ou la quantité est de zero
        // en trouvant l'index
        List<int> indexToSuprr = new List<int>();
        for (int i = 0; i < listFinale.Count; i++)
        {
            if (listFinale[i].quantite == 0)
            {
                indexToSuprr.Add(i);
            }
        }
        // et en les supprimant
        for (int i = indexToSuprr.Count - 1; i > -1; i--)
        {
            listFinale.RemoveAt(indexToSuprr[i]);
        }
        return listFinale;
    }

    int[] CalculQuantiteRessource(int codeRessource)
    {
        // en sortie on met un tableau de int 
        // le premier indique le nb de ress 
        //et le 2 eme 1 si il y a une planete et 0 sinon
        int[] result = new int[2];
        //planet = result[1]
        result[1] = 0;
        int quantite = 0;
        switch (codeRessource)
        {
            case 0: // energie
                quantite = UnityEngine.Random.Range(2, 7);
                break;
            case 1:// 1 minerais
                quantite = UnityEngine.Random.Range(2, 8);
                break;
            case 2://alliages
                quantite = UnityEngine.Random.Range(1, 4);
                break;
            case 3:// sc phy
                quantite = UnityEngine.Random.Range(1, 4);
                break;
            case 4: // planet
                quantite = UnityEngine.Random.Range(0, 4);
                result[1] = 1;
                break;
            /*
        case 5:
            quantite = UnityEngine.Random.Range(1, 4);
            break;
        case 6:
            quantite = UnityEngine.Random.Range(1, 4);
            break;
        case 7:
            quantite = UnityEngine.Random.Range(1, 4);
            break;
        case 8:
            quantite = UnityEngine.Random.Range(1, 4);
            break;
        case 9:
            quantite = UnityEngine.Random.Range(1, 4);
            break;
        case 10:
            quantite = UnityEngine.Random.Range(1, 4);
            break;
        case 11:
            quantite = UnityEngine.Random.Range(1, 3);
            break;
            */
            default:
                quantite = 1;
                break;
        }
        result[0] = quantite;

        return result;
    }

    /// <summary>
    /// CreationConnexion
    /// </summary>
    void CreationConnexion()
    {
       // PathFinder.instance.graphData.paths.Clear();
        int cptrLigne = 0;

        foreach (List<InfoSystemeSolaire> listeSSol in GameStatic.tabloInfosSystemeSolaire)
        {
            if (cptrLigne > 0)
            {
                RelierSSolAuDessus(listeSSol);
                RelierSSolHautGauche(listeSSol);
                RelierSSolHautDroite(listeSSol);
            }
            else
            {
            }
            RelierSSolAGauche(listeSSol);
            cptrLigne++;
        }
    }

    void RelierSSolAGauche(List<InfoSystemeSolaire> listeSSol)
    {
        int cptrColonne = 0;
        foreach (InfoSystemeSolaire SSol in listeSSol)
        {
            if (cptrColonne > 0)
            {
                // je fais un random pour savoir si j'ajoute une connexion
                float testConnexion = UnityEngine.Random.Range(0, aleaNbConnexions);
                if (testConnexion > 4)
                { // si test ok
                  // j ajoute la coordonnée des SSol accessibles aux deux SSol concernés
                    GameStatic.tabloInfosNavigation[SSol.indexLigneListSS][SSol.indexColonneListSS].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS, SSol.indexColonneListSS - 1));
                    GameStatic.tabloInfosNavigation[SSol.indexLigneListSS][SSol.indexColonneListSS - 1].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS, SSol.indexColonneListSS));

                    /*
                    Coordonnees ssolDepart = new Coordonnees(SSol.indexLigneListSS, SSol.indexColonneListSS);
                    Coordonnees ssolArrivee = new Coordonnees(SSol.indexLigneListSS, SSol.indexColonneListSS - 1);
                    Path path = new Path(CoordTabloToListIndex(ssolDepart), CoordTabloToListIndex(ssolArrivee));
                    PathFinder.instance.graphData.paths.Add(path);
                    */
                }
            }
            cptrColonne++;
        }
    }

    void RelierSSolAuDessus(List<InfoSystemeSolaire> listeSSol)
    {
        int cptrColonne = 0;
        foreach (InfoSystemeSolaire SSol in listeSSol)
        {
            // je fais un random pour savoir si j'ajoute une connexion
            float testConnexion = UnityEngine.Random.Range(0, aleaNbConnexions);
            if (testConnexion > 4)
            { // si test ok
                // j ajoute la coordonnée des SSol accessibles aux deux SSol concernés
                GameStatic.tabloInfosNavigation[SSol.indexLigneListSS][cptrColonne].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS - 1, cptrColonne));
                GameStatic.tabloInfosNavigation[SSol.indexLigneListSS - 1][cptrColonne].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS, cptrColonne));
                /*
                Coordonnees ssolDepart = new Coordonnees(SSol.indexLigneListSS, cptrColonne);
                Coordonnees ssolArrivee = new Coordonnees(SSol.indexLigneListSS - 1, cptrColonne);
                Path path = new Path(CoordTabloToListIndex(ssolDepart), CoordTabloToListIndex(ssolArrivee));
                PathFinder.instance.graphData.paths.Add(path);
                */
            }
            cptrColonne++;
        }
    }

    void RelierSSolHautGauche(List<InfoSystemeSolaire> listeSSol)
    {
        int cptrColonne = 0;
        foreach (InfoSystemeSolaire SSol in listeSSol)
        {
            if (cptrColonne > 0)
            {
                // je fais un random pour savoir si j'ajoute une connexion
                float testConnexion = UnityEngine.Random.Range(0, aleaNbConnexions);
                if (testConnexion > 4)
                { // si test ok
                    // j ajoute la coordonnée des SSol accessibles aux deux SSol concernés
                    GameStatic.tabloInfosNavigation[SSol.indexLigneListSS][cptrColonne].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS - 1, cptrColonne - 1));
                    GameStatic.tabloInfosNavigation[SSol.indexLigneListSS - 1][cptrColonne - 1].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS, cptrColonne));
                    /*
                    Coordonnees ssolDepart = new Coordonnees(SSol.indexLigneListSS, cptrColonne);
                    Coordonnees ssolArrivee = new Coordonnees(SSol.indexLigneListSS - 1, cptrColonne - 1);
                    Path path = new Path(CoordTabloToListIndex(ssolDepart), CoordTabloToListIndex(ssolArrivee));
                    PathFinder.instance.graphData.paths.Add(path);
                    */
                }
            }
            cptrColonne++;
        }
    }

    void RelierSSolHautDroite(List<InfoSystemeSolaire> listeSSol)
    {
        int nbColonnes = GameStatic.tabloInfosSystemeSolaire[0].Count;
        int cptrColonne = 0;
        foreach (InfoSystemeSolaire SSol in listeSSol)
        {
            if (cptrColonne < nbColonnes - 1)
            {
                // je fais un random pour savoir si j'ajoute une connexion
                float testConnexion = UnityEngine.Random.Range(0, aleaNbConnexions);
                if (testConnexion > 4)
                { // si test ok
                    // j ajoute la coordonnée des SSol accessibles aux deux SSol concernés
                    GameStatic.tabloInfosNavigation[SSol.indexLigneListSS][cptrColonne].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS - 1, cptrColonne + 1));
                    GameStatic.tabloInfosNavigation[SSol.indexLigneListSS - 1][cptrColonne + 1].SSAtteignables.Add(new Coordonnees(SSol.indexLigneListSS, cptrColonne));

                    /*
                    Coordonnees ssolDepart = new Coordonnees(SSol.indexLigneListSS, cptrColonne);
                    Coordonnees ssolArrivee = new Coordonnees(SSol.indexLigneListSS - 1, cptrColonne + 1);
                    Path path = new Path(CoordTabloToListIndex(ssolDepart), CoordTabloToListIndex(ssolArrivee));
                    PathFinder.instance.graphData.paths.Add(path);
                    */
                }
            }
            cptrColonne++;
        }
    }

    /// <summary>
    /// prend une coordonnée du tableau 2D et 
    /// renvoie l'index du ssol correspondant dans la liste de node
    /// </summary>
    /// <param name="ssol"></param>
    /// <returns>index list de nodes</returns>
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
