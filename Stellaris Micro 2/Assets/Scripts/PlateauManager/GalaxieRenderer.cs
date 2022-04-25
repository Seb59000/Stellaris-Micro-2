using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using classDuJeu;

/* Soccupe des SSol physiques et des panels Infos qui suivent les SSol*/

[RequireComponent(typeof(CreationGalaxieInfo))]
[RequireComponent(typeof(CreationSSolPhysique))]
public class GalaxieRenderer : MonoBehaviour
{
    protected CreationGalaxieInfo createGalaxieInfo;
    protected CreationSSolPhysique createSSolPhysique;
    public Material transparentMaterial;
    public Material arrowMaterial;
    public List<Material> listeCouleurs;
    public List<LineRenderer> listLinePrefab;
    public LineRenderer linePrefab;
    public Transform panelInfoPrefab;
    public Canvas canvas;

    ///<summary>Attention le nb de colonnes doit etre au moins egal a 3
    ///</summary>
    public static int nbColonnes = 8;
    ///<summary>Attention le nb de lignes doit etre au moins egal a 3
    ///</summary>
    public static int nbLignes = 7;

    protected List<Connexion> listconnexions = new List<Connexion>();

    // Called automatically by Unity when the script first exists in the scene.

    void Awake()
    {
        createGalaxieInfo = GetComponent<CreationGalaxieInfo>();
        createSSolPhysique = GetComponent<CreationSSolPhysique>();
    }

    void Start()
    {
        // si il est deja rempli je le vide
        int count = GameStatic.tabloInfosSystemeSolaire.Count;
        if (count > 0)
        {
            RemiseAZeroInfos();
        }
        CreerNouvelleMap();
    }

    void Update()
    {
        UpDatePosPanelsInfos();
    }

    void UpDatePosPanelsInfos()
    {
        int indexLigne = 0;
        //les panels suivent les positions des etoiles sur l'ecran
        foreach (List<GameObject> lignessol in GameStatic.tableauSystemeSolaire)
        {
            int indexColonne = 0;
            foreach (GameObject ssol in lignessol)
            {
                GameStatic.panelTomove[indexLigne][indexColonne].position = Camera.main.WorldToScreenPoint(ssol.transform.position);
                indexColonne++;
            }
            indexLigne++;
        }
    }

    private void RemiseAZeroInfos()
    {
        GameStatic.tabloInfosSystemeSolaire.Clear();

        GameStatic.clickDeplacement = false;
        GameStatic.enGuerre = false;
        GameStatic.listeTroupesAlliees.Clear();
        GameStatic.NBFORCESALLIEESDEBUT = 2;
        /*
        int count = GameStatic.panelTomove.Count;
        for (int i = 0; i < count; i++)
        {
            int count2 = GameStatic.panelTomove[i].Count;
            for (int j = 0; j < count2; j++)
            {
                Destroy(GameStatic.panelTomove[i][j].gameObject);
            }
        }*/
        GameStatic.placesEmpires.Clear();
        GameStatic.ressourcesJoueur = new RessourcesJoueur();
        GameStatic.ssolEnGuerre.Clear();
        /*
        count = GameStatic.tableauSystemeSolaire.Count;
        for (int i = 0; i < count; i++)
        {
            int count2 = GameStatic.tableauSystemeSolaire[i].Count;
            for (int j = 0; j < count2; j++)
            {
                Destroy(GameStatic.tableauSystemeSolaire[i][j].gameObject);
            }
        }
        GameStatic.tableauSystemeSolaire.Clear();*/
        /*
        count = listLinePrefab.Count;
        for (int i = 0; i < count; i++)
        {

                Destroy(listLinePrefab[i].gameObject);
        }
        */
        listLinePrefab.Clear();
        GameStatic.tabloInfosNavigation.Clear();
        GameStatic.tabloInfosSystemeSolaire.Clear();
    }

    void CreerNouvelleMap()
    {
        createGalaxieInfo.CreationInfosCarte();
        createSSolPhysique.CreationSSolDebutPartie();

        AffichageCouleursEmpire(1, GameStatic.placesEmpires[0]);

        // affichage connexions visibles et systemes atteignable
        AfficherConnexions(GameStatic.placesEmpires[0]);

        // on cree les panels ressources
        InstanciationPanelsInfos();
        MiseAjourInfosPanelsDebutPartie();
    }

    private void AffichageInfosSSolEnnemi()
    {
        foreach (EnnemiData data in StaticEnnemi.listEnnemiData)
        {
            // on affiche les forces de la base
            MAJAffichageForceBase(data.listCoordPossede[0], GameStatic.PUISSANCEBASEDEBUT);
            // on affiche les forces de la troupe alliée
            MAJAffichageNbTroupesAlliees(data.listCoordPossede[0], GameStatic.tabloInfosSystemeSolaire[data.listCoordPossede[0].indexLigneListSS][data.listCoordPossede[0].indexColonneListSS].forceAllies);
            // on affiche les couleurs
            AffichageCouleursEmpire(data.codeEnnemi, data.listCoordPossede[0]);
        }
    }

    public void AffichageCouleursEmpire(int indexCouleur, Coordonnees ssol)
    {
        // je met la couleur du systm solaire aux couleurs de l'empire du joueur
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[ssol.indexLigneListSS][ssol.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        allRendererChildren[3].material = listeCouleurs[indexCouleur];
    }

    private void remplissageTableauConnexionsVisibles(Coordonnees coordonnees)
    {
        Vector3 positionSystOrigine = GameStatic.tableauSystemeSolaire[coordonnees.indexLigneListSS][coordonnees.indexColonneListSS].gameObject.transform.position;
        foreach (Coordonnees ssolAtteignable in GameStatic.tabloInfosNavigation[coordonnees.indexLigneListSS][coordonnees.indexColonneListSS].SSAtteignables)
        {
            // j'ajoute la connexion a la liste des connexions pour rendering
            listconnexions.Add(new Connexion(GameStatic.tableauSystemeSolaire[ssolAtteignable.indexLigneListSS][ssolAtteignable.indexColonneListSS].gameObject.transform.position, positionSystOrigine));
            // je le marque comme atteignable dans le tableau General
            GameStatic.tabloInfosSystemeSolaire[ssolAtteignable.indexLigneListSS][ssolAtteignable.indexColonneListSS].isAtteignable = true;
        }
    }

    private void InstanciationPanelsInfos()
    {
        GameStatic.panelTomove = new List<List<Transform>>();

        // on crée le bon nb de lignes dans la liste panelTomove
        for (int i = 0; i < nbLignes; i++)
        {
            GameStatic.panelTomove.Add(new List<Transform>());
        }

        foreach (List<InfoSystemeSolaire> lignessol in GameStatic.tabloInfosSystemeSolaire)
        {
            foreach (InfoSystemeSolaire ssol in lignessol)
            {
                // pour chaque ssol on instancie un prefab de panel
                Transform clone = Instantiate<Transform>(panelInfoPrefab, transform.position, transform.rotation, canvas.transform);
                GameStatic.panelTomove[ssol.indexLigneListSS].Add(clone);
            }
        }
    }

    void MiseAjourInfosPanelsDebutPartie()
    {
        // on cache tous les panels infos sauf celui du ssol du joueur
        for (int i = 0; i < nbLignes; i++)
        {
            for (int j = 0; j < nbColonnes; j++)
            {
                if (GameStatic.tabloInfosSystemeSolaire[i][j].proprietaire == 1)
                {
                    InitialisationInfosPanel(i, j, GameStatic.panelTomove[i][j].gameObject);
                    //CacherPanelInfoJoueur(i, j, GameStatic.panelTomove[i][j].gameObject);
                    MAJAffichageForceBase(new Coordonnees(i, j), (GameStatic.PUISSANCEBASEDEBUT + GameStatic.UPGRADEBASE));
                    MAJAffichageNbTroupesAlliees(new Coordonnees(i, j), GameStatic.NBFORCESALLIEESDEBUT);
                    MAJAffichageNbTroupesEnnemies(new Coordonnees(i, j), 0);
                }
                else
                {
                    InitialisationInfosPanel(i, j, GameStatic.panelTomove[i][j].gameObject);
                    CacherPanelInfo(i, j, GameStatic.panelTomove[i][j].gameObject);
                }
            }
        }
    }

    private void CacherPanelInfoJoueur(int indexLigneTraite, int indexColonneTraite, GameObject panelInfo)
    {
        // on cache les panels forces militaires ennemies
        Image[] allImgChildren = panelInfo.transform.GetComponentsInChildren<Image>();
        allImgChildren[1].enabled = false;

        // on cache les textes Forces militaires ennemi
        Text[] allTextChildren = panelInfo.transform.GetComponentsInChildren<Text>();
        allTextChildren[0].enabled = false;

    }
    private void CacherPanelInfo(int indexLigneTraite, int indexColonneTraite, GameObject panelInfo)
    {
        // on cache les panels forces militaires etc...
        Image[] allImgChildren = panelInfo.transform.GetComponentsInChildren<Image>();
        int tailleTablo = allImgChildren.Length;
        for (int i = 0; i < tailleTablo; i++)
        {
            allImgChildren[i].enabled = false;
        }

        // on cache les textes
        Text[] allTextChildren = panelInfo.transform.GetComponentsInChildren<Text>();
        int tailleTablo2 = allTextChildren.Length;
        for (int i = 0; i < tailleTablo2; i++)
        {
            allTextChildren[i].enabled = false;
        }

        // on cache les images
        RawImage[] allMeshChildren = panelInfo.transform.GetComponentsInChildren<RawImage>();
        int tailleTablo3 = allMeshChildren.Length;
        for (int i = 0; i < tailleTablo3; i++)
        {
            allMeshChildren[i].enabled = false;
        }
    }

    void InitialisationInfosPanel(int indexLigneTraite, int indexColonneTraite, GameObject panelInfo)
    {
        //je recupere les differents texts contenus dans mon panel
        Text[] allTextChildren = panelInfo.transform.GetComponentsInChildren<Text>(true);

        List<Ressource> listRessources = GameStatic.tabloInfosSystemeSolaire[indexLigneTraite][indexColonneTraite].listRessources;
        bool noEnergie = true;
        bool noMinerai = true;
        bool noalliages = true;
        bool nocristaux = true;
        bool nogaz = true;
        bool noparticules = true;
        bool noScSo = true;
        bool noScPhy = true;
        bool noInge = true;
        bool noCommerce = true;
        bool noPlanet = true;
        int codePlanet = -1;

        foreach (Ressource res in listRessources)
        {
            switch (res.codeRessource)
            {
                case 0:
                    noEnergie = false;
                    // on change le texte
                    allTextChildren[3].text = res.quantite.ToString();
                    break;
                case 1:
                    noMinerai = false;
                    allTextChildren[4].text = res.quantite.ToString();
                    break;
                case 2:
                    noalliages = false;
                    allTextChildren[5].text = res.quantite.ToString();
                    break;
                case 3:
                    noScPhy = false;
                    allTextChildren[6].text = res.quantite.ToString();
                    break;
                case 4:
                    noPlanet = false;
                    codePlanet = res.quantite;
                    break;
                case 5:
                    noparticules = false;
                    allTextChildren[8].text = res.quantite.ToString();
                    break;
                case 6:
                    noScSo = false;
                    allTextChildren[9].text = res.quantite.ToString();
                    break;
                case 7:
                    nocristaux = false;
                    allTextChildren[10].text = res.quantite.ToString();
                    break;
                case 8:
                    noInge = false;
                    allTextChildren[11].text = res.quantite.ToString();
                    break;
                case 9:
                    noCommerce = false;
                    allTextChildren[12].text = res.quantite.ToString();
                    break;
                case 10:
                    nogaz = false;
                    allTextChildren[7].text = res.quantite.ToString();
                    break;
                default:
                    break;
            }
        }

        if (noEnergie)
        {
            allTextChildren[3].gameObject.SetActive(false);
        }
        if (noMinerai)
        {
            allTextChildren[4].gameObject.SetActive(false);
        }
        if (noalliages)
        {
            allTextChildren[5].gameObject.SetActive(false);
        }
        if (noScPhy)
        {
            allTextChildren[6].gameObject.SetActive(false);
        }
        if (noparticules)
        {
            allTextChildren[7].gameObject.SetActive(false);
        }
        if (noScSo)
        {
            allTextChildren[8].gameObject.SetActive(false);
        }
        if (nocristaux)
        {
            allTextChildren[9].gameObject.SetActive(false);
        }
        if (noInge) // phy
        {
            allTextChildren[10].gameObject.SetActive(false);
        }
        if (noCommerce) // ingé
        {
            allTextChildren[11].gameObject.SetActive(false);
        }
        if (nogaz) // commerce
        {
            allTextChildren[12].gameObject.SetActive(false);
        }

        // partie planetes
        Image[] allImageChildren = panelInfo.transform.GetComponentsInChildren<Image>();
        if (noPlanet)
        {
            allImageChildren[4].gameObject.SetActive(false);
            allImageChildren[5].gameObject.SetActive(false);
            allImageChildren[6].gameObject.SetActive(false);
            allImageChildren[7].gameObject.SetActive(false);
        }
        else
        {
            switch (codePlanet)
            {
                case 0:
                    allImageChildren[4].gameObject.SetActive(true);
                    allImageChildren[5].gameObject.SetActive(false);
                    allImageChildren[6].gameObject.SetActive(false);
                    allImageChildren[7].gameObject.SetActive(false);
                    break;
                case 1:
                    allImageChildren[4].gameObject.SetActive(false);
                    allImageChildren[5].gameObject.SetActive(true);
                    allImageChildren[6].gameObject.SetActive(false);
                    allImageChildren[7].gameObject.SetActive(false);
                    break;
                case 2:
                    allImageChildren[4].gameObject.SetActive(false);
                    allImageChildren[5].gameObject.SetActive(false);
                    allImageChildren[6].gameObject.SetActive(true);
                    allImageChildren[7].gameObject.SetActive(false);
                    break;
                case 3:
                    allImageChildren[4].gameObject.SetActive(false);
                    allImageChildren[5].gameObject.SetActive(false);
                    allImageChildren[6].gameObject.SetActive(false);
                    allImageChildren[7].gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    public void AfficherConnexions(Coordonnees coordonnees)
    {
        remplissageTableauConnexionsVisibles(coordonnees);

        foreach (Connexion con in listconnexions)
        {
            LineRenderer lr = Instantiate(linePrefab, transform);
            lr.positionCount = 2;
            lr.SetPosition(0, con.position1);
            lr.SetPosition(1, con.position2);
            listLinePrefab.Add(linePrefab);
        }
        //on vide la liste des nouvelles connexions a afficher pour pas les afficher deux fois
        listconnexions.Clear();
    }

    public void MAJAffichageNbTroupesEnnemies(Coordonnees localisation, int nbTroupe)
    {
        //on recupere le panel concernes
        Transform panelOrigine = GameStatic.panelTomove[localisation.indexLigneListSS][localisation.indexColonneListSS];

        // on recupere les infos a changer sur le panel
        Image[] allImgChildrenOrigine = panelOrigine.transform.GetComponentsInChildren<Image>();
        Text[] allTextChildrenOrigine = panelOrigine.transform.GetComponentsInChildren<Text>();

        //on les met a jour
        if (nbTroupe == 0)
        { // on cache
            allImgChildrenOrigine[1].enabled = false;
            allTextChildrenOrigine[0].text = nbTroupe.ToString();
            allTextChildrenOrigine[0].enabled = false;
        }
        else
        {
            // si il reste des forces ennemies on met a jour
            allImgChildrenOrigine[1].enabled = true;
            allTextChildrenOrigine[0].enabled = true;
            allTextChildrenOrigine[0].text = nbTroupe.ToString();
        }
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

    public void MAJAffichageForceBase(Coordonnees localisation, int nbTroupe)
    {
        //on recupere le panel concernes
        Transform panel = GameStatic.panelTomove[localisation.indexLigneListSS][localisation.indexColonneListSS];

        // on recupere les infos a changer sur le panel
        Image[] allImgChildren = panel.transform.GetComponentsInChildren<Image>(true);
        Text[] allTextChildren = panel.transform.GetComponentsInChildren<Text>(true);

        //on les met a jour
        if (nbTroupe == 0)
        { // on cache
            allImgChildren[3].enabled = false;
            allTextChildren[2].text = nbTroupe.ToString();
            allTextChildren[2].enabled = false;
        }
        else
        {
            // si il reste des forces ennemies on met a jour
            allImgChildren[3].enabled = true;
            allTextChildren[2].enabled = true;
            allTextChildren[2].text = nbTroupe.ToString();
        }
    }

    public void CacherFleche(Coordonnees localisation)
    {
        // on change la direction du systeme qui nous a amené ici
        GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].direction = new Coordonnees(localisation.indexLigneListSS, localisation.indexColonneListSS);
        // on cache la fleche
        MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        allRendererChildren[4].material = transparentMaterial;
    }

    public void AffichageRessources(int indexLigneTraite, int indexColonneTraite)
    {
        // recuperation du panel info a MAJ
        GameObject panelInfo = GameStatic.panelTomove[indexLigneTraite][indexColonneTraite].gameObject;

        // panels forces militaires etc...
        Image[] allImgChildren = panelInfo.transform.GetComponentsInChildren<Image>();
        Text[] allTextChildren = panelInfo.transform.GetComponentsInChildren<Text>();

        // on recupere les forces presentes dans le systeme
        int forcesSys = GameStatic.tabloInfosSystemeSolaire[indexLigneTraite][indexColonneTraite].forceAllies;
        // si il ya des troupes ennemies on les montre !! dans le carré bleu 
        if (forcesSys > 0)
        {
            allImgChildren[2].enabled = true;
            allTextChildren[1].enabled = true;
            allTextChildren[1].text = forcesSys.ToString();
        }

        // on affiche la force de la base
        allImgChildren[3].enabled = true;
        allTextChildren[2].enabled = true;
        allTextChildren[2].text = GameStatic.tabloInfosSystemeSolaire[indexLigneTraite][indexColonneTraite].forceBase.ToString();

        // on montre les textes des ressources
        int tailleTablo2 = allTextChildren.Length;
        for (int i = 3; i < tailleTablo2; i++)
        {
            allTextChildren[i].enabled = true;
        }

        // on montre les images des ressources
        RawImage[] allMeshChildren = panelInfo.transform.GetComponentsInChildren<RawImage>();
        int tailleTablo3 = allMeshChildren.Length;
        for (int i = 0; i < tailleTablo3; i++)
        {
            allMeshChildren[i].enabled = true;
        }
    }
}
