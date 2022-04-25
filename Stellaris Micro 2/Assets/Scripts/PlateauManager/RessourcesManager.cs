using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using classDuJeu;

[RequireComponent(typeof(GalaxieRenderer))]
[RequireComponent(typeof(TroupeManager))]
[RequireComponent(typeof(GestionBtnsAction))]
[RequireComponent(typeof(EnnemiManager))]
[RequireComponent(typeof(WarManager))]

public class RessourcesManager : MonoBehaviour
{

    protected WarManager warManager;
    protected GalaxieRenderer galaxieRenderer;
    protected TroupeManager troupeManager;
    protected GestionBtnsAction gestionBtnsAction;
    protected EnnemiManager ennemiManager;

    public GameObject panelScience;
    public Button btnUpBase;
    public Button btnUpVaisseaux;
    public Button btnUpVaisseaux2;

    // Ressources
    public Text totalEnergieTxt;
    public Text supEnergieTxt;
    public Text totalMinerairTxt;
    public Text supMineraiTxt;
    public Text totalAlliageTxt;
    public Text supAlliageTxt;
    public Text supScienceTxt;

    // timer
    public Text anneeTxt;
    public Text moisTxt;
    public Text jourTxt;

    // var timer
    private int annee = 2100;
    private int mois = 1;
    private int jourInt = 1;
    private int dernierJourEnregistre = 1;
    private float jour = 01f;

    void Awake()
    {
        warManager = GetComponent<WarManager>();
        galaxieRenderer = GetComponent<GalaxieRenderer>();
        troupeManager = GetComponent<TroupeManager>();
        gestionBtnsAction = GetComponent<GestionBtnsAction>();
        ennemiManager = GetComponent<EnnemiManager>();
    }

    void Start()
    {
        MAJAffichageRessourcesDebut();
    }

    private void MAJAffichageRessourcesDebut()
    {
        AffichageTotAlliage(GameStatic.ressourcesJoueur.alliageTotal.ToString());
        AffichageTotEnergie(GameStatic.ressourcesJoueur.energieTotal.ToString());
        AffichageTotMinerai(GameStatic.ressourcesJoueur.mineraiTotal.ToString());
        AffichageSuppMinerai(GameStatic.ressourcesJoueur.supMinerai.ToString());
        AffichageSuppEnergie(GameStatic.ressourcesJoueur.supEnergie.ToString());
        AffichageSuppAlliage(GameStatic.ressourcesJoueur.supAlliage.ToString());
        AffichageSuppScience(GameStatic.ressourcesJoueur.supScience.ToString());
    }

    void Update()
    {
        GestionTemps();
    }

    private void GestionTemps()
    {
        //MAJ jours
        jour += Time.deltaTime * GameStatic.accelerateurTemps;
        jourInt = (int)jour;

        // si un jour entier est passé
        if (jourInt > dernierJourEnregistre)
        {
            // chgt jour detecté
            //MAJ des mois
            if (jourInt == 31)
            {
                jour = 0f;
                jourInt = 0;
                mois++;
                //MAJ des années
                if (mois == 13)
                {
                    annee++;
                    mois = 1;
                    AffichageAnnees();
                }
                AffichageMois();
                // tous les mois j'actualise les ressources
                MAJRessources();
                MAJRessourcesEnnemi();
            }
            AffichageJours();
            // chaque jour je met a jour la position des troupes du joueur
            troupeManager.ActualisationTroupesJoueur();
            ennemiManager.DeplacementEnnemi();
            // si je suis en guerre
            if (GameStatic.enGuerre)
            {
                warManager.Guerre();
            }
        }
        dernierJourEnregistre = jourInt;
    }

    private void AffichageAnnees()
    {
        anneeTxt.text = annee.ToString();
    }

    private void AffichageMois()
    {
        if (mois < 10)
        {
            // on rajoute un zero pour la mise en forme
            string stemp = mois.ToString();
            stemp = "0" + stemp;
            moisTxt.text = stemp;
        }
        else
        {
            moisTxt.text = mois.ToString();
        }
    }

    private void AffichageJours()
    {
        if (jour < 10)
        {
            // on rajoute un zero pour la mise en forme
            string stemp = jourInt.ToString();
            stemp = "0" + stemp;
            jourTxt.text = stemp;
        }
        else
        {
            jourTxt.text = jourInt.ToString();
        }
    }

    private void MAJRessourcesEnnemi()
    {
        // pour chaque ennemi
        int indexEnnemi = 0;
        foreach (EnnemiData data in StaticEnnemi.listEnnemiData)
        {
            // calcul energie tot
            data.ressourceEnnemi.energieTotal = data.ressourceEnnemi.energieTotal + data.ressourceEnnemi.supEnergie;

            //calcul minerai tot
            data.ressourceEnnemi.mineraiTotal = data.ressourceEnnemi.mineraiTotal + data.ressourceEnnemi.supMinerai;

            //calcul alliages tot
            data.ressourceEnnemi.alliageTotal = data.ressourceEnnemi.alliageTotal + data.ressourceEnnemi.supAlliage;

            // si alliage >= prixVaisseau => pdtion d'un vaisseau et  on retire prixVaisseau a total alliage
            if (data.ressourceEnnemi.alliageTotal >= data.ressourceEnnemi.prixVaisseau)
            {
                // on retire le prix du vaisseau des ressources du joueur
                data.ressourceEnnemi.alliageTotal = data.ressourceEnnemi.alliageTotal - data.ressourceEnnemi.prixVaisseau;

                // on met a jour le nb de vaisseaux dans le systeme ou est basé la production
                MAJNbVaisseauxEnnemi(indexEnnemi);
            }

            //calcul science
            data.ressourceEnnemi.science = data.ressourceEnnemi.science + data.ressourceEnnemi.supScience;
            if (data.ressourceEnnemi.science >= data.prixScience)
            {
                // deblocage nelle technologie
                if (data.prixUpgradeBase > 20)
                {
                    int aleaScience = UnityEngine.Random.Range(0, 3);

                    if (aleaScience == 0)
                    {

                        // UpgradeBaseIA
                        data.prixUpgradeBase -= GameStatic.BAISSEPRIXBASE;
                    }
                    else
                    {
                        // up vaisseaux
                        if (data.prixVaisseau < 25)
                        {
                            // on remet le prix des vaisseaux comme au debut
                            data.prixVaisseau = GameStatic.PRIXVAISSEAUXDEBUT;

                            // on augmente la force des vaisseaux
                            data.forceVaisseaux += 2;
                        }
                        else
                        {
                            // on diminue le prix des vaisseaux
                            data.prixVaisseau -= data.baissePrixVaisseaux;
                            // aprés chaque baisse on rend la prochaine baisse moins importante
                            if (data.baissePrixVaisseaux > 1)
                            {
                                data.baissePrixVaisseaux--;
                            }
                        }
                    }
                }
                else
                {
                    // up vaisseaux
                    if (data.prixVaisseau < 25)
                    {
                        // on remet le prix des vaisseaux comme au debut
                        data.prixVaisseau = GameStatic.PRIXVAISSEAUXDEBUT;

                        // on augmente la force des vaisseaux
                        data.forceVaisseaux += 2;
                    }
                    else
                    {
                        // on diminue le prix des vaisseaux
                        data.prixVaisseau -= data.baissePrixVaisseaux;
                        // aprés chaque baisse on rend la prochaine baisse moins importante
                        if (data.baissePrixVaisseaux > 1)
                        {
                            data.baissePrixVaisseaux--;
                        }
                    }
                }
                // on retire le prix de l'avancée technologique
                data.ressourceEnnemi.science -= data.prixScience;
                // a chaque avancée tech on rend la science plus chère
                data.prixScience += GameStatic.HAUSSEPRIXSCIENCE;
            }
            indexEnnemi++;
        }
    }

    private void MAJNbVaisseauxEnnemi(int indexEnnemi)
    {
        // si elle existe
        if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes.Count > 0)
        {
            // on recupere le SSol ou se trouve la premiere troupe de l'ennemi
            Coordonnees localisationTroupe = StaticEnnemi.listEnnemiData[indexEnnemi].listCoordTroupes[0].localisation;

            // si c'est un ssol qui lui appartient
            if (GameStatic.tabloInfosSystemeSolaire[localisationTroupe.indexLigneListSS][localisationTroupe.indexColonneListSS].proprietaire == indexEnnemi + 2)
            {
                CreationVaisseauxSsolAllie(indexEnnemi, localisationTroupe);
            }
            else
            {
                CreationVaisseauxSsolEnnemi(indexEnnemi, localisationTroupe);
            }
        }
        else
        {
            // sinon s'il existe
            if (StaticEnnemi.listEnnemiData[indexEnnemi].listCoordPossede.Count > 0)
            {
                // on crée une nouvelle troupe dans le 1er ssol de l'ennemi
                Coordonnees ssol = StaticEnnemi.listEnnemiData[indexEnnemi].listCoordPossede[0];

                CreationVaisseauxSsolAllie(indexEnnemi, ssol);
            }
        }
    }

    private void CreationVaisseauxSsolEnnemi(int indexEnnemi, Coordonnees localisationTroupeEnnemie)
    {
        // MAJ infos ssol troupes ennemies
        GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].forceEnnemies += StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.forceVaisseaux;

        // si scanné on met a jour les panels infos ssol 
        if (GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].isScanne)
        {
            galaxieRenderer.MAJAffichageNbTroupesEnnemies(localisationTroupeEnnemie, GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].forceEnnemies);
        }
    }

    private void CreationVaisseauxSsolAllie(int indexEnnemi, Coordonnees localisationTroupeEnnemie)
    {
        // MAJ infos ssol troupes alliées
        GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].forceAllies += StaticEnnemi.listEnnemiData[indexEnnemi].ressourceEnnemi.forceVaisseaux;

        if (GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].isScanne)
        {
            galaxieRenderer.MAJAffichageNbTroupesAlliees(localisationTroupeEnnemie, GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].forceAllies);
        }

        // si scanné on met a jour les panels infos ssol 
        if (GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].isScanne)
        {
            galaxieRenderer.MAJAffichageNbTroupesAlliees(localisationTroupeEnnemie, GameStatic.tabloInfosSystemeSolaire[localisationTroupeEnnemie.indexLigneListSS][localisationTroupeEnnemie.indexColonneListSS].forceAllies);
        }
    }

    private void MAJRessources()
    {
        // pour gagner en ressoures cpu ↓ pas sur que ce soit optimale?
        RessourcesJoueur ressourcesJoueur = GameStatic.ressourcesJoueur;

        // calcul energie tot
        ressourcesJoueur.energieTotal = ressourcesJoueur.energieTotal + ressourcesJoueur.supEnergie;
        // affichage du total MAJ
        AffichageTotEnergie(ressourcesJoueur.energieTotal.ToString());

        //calcul minerai tot
        ressourcesJoueur.mineraiTotal = ressourcesJoueur.mineraiTotal + ressourcesJoueur.supMinerai;
        // affichage du total MAJ
        AffichageTotMinerai(ressourcesJoueur.mineraiTotal.ToString());

        // activationBtnsAction
        gestionBtnsAction.VerifEtActivationBtnForceBase();

        //calcul alliages tot
        ressourcesJoueur.alliageTotal = ressourcesJoueur.alliageTotal + ressourcesJoueur.supAlliage;

        // si alliage >= prixVaisseau => pdtion d'un vaisseau et  on retire prixVaisseau a total alliage
        if (ressourcesJoueur.alliageTotal >= ressourcesJoueur.prixVaisseau)
        {
            // on retire le prix du vaisseau des ressources du joueur
            ressourcesJoueur.alliageTotal = ressourcesJoueur.alliageTotal - ressourcesJoueur.prixVaisseau;

            // on met a jour le nb de vaisseaux dans le systeme ou est basé la production
            MAJNbVaisseaux(ressourcesJoueur);
        }
        // affichage du total MAJ
        AffichageTotAlliage(ressourcesJoueur.alliageTotal.ToString());

        //calcul science
        ressourcesJoueur.science = ressourcesJoueur.science + ressourcesJoueur.supScience;
        if (ressourcesJoueur.science >= GameStatic.ressourcesJoueur.prixScience)
        {
            // deblocage nelle technologie
            //on verifie que les btns peuvent être actifs
            if (GameStatic.ressourcesJoueur.prixVaisseau < 25)
            {
                btnUpVaisseaux.gameObject.SetActive(false);
                btnUpVaisseaux2.gameObject.SetActive(true);
            }
            else
            {
                btnUpVaisseaux.gameObject.SetActive(true);
                btnUpVaisseaux2.gameObject.SetActive(false);
            }
            if (GameStatic.ressourcesJoueur.prixUpgradeBase < 20)
            {
                btnUpBase.interactable = false;
            }
            panelScience.SetActive(true);
            ressourcesJoueur.science -= GameStatic.ressourcesJoueur.prixScience;
            GameStatic.accelerateurTemps = 0;
        }


        // pour gagner en ressoures cpu ↓ pas sur que ce soit optimale?
        GameStatic.ressourcesJoueur = ressourcesJoueur;
    }

    public void AffichageTotAlliage(string suppAlliage)
    {
        totalAlliageTxt.text = suppAlliage;
    }

    public void AffichageTotMinerai(string suppMinerai)
    {
        totalMinerairTxt.text = suppMinerai;
    }

    public void AffichageTotEnergie(string suppEnergie)
    {
        totalEnergieTxt.text = suppEnergie;
    }

    public void AffichageSuppScience(string suppScience)
    {
        supScienceTxt.text = suppScience;
    }

    public void AffichageSuppAlliage(string suppAlliage)
    {
        supAlliageTxt.text = suppAlliage;
    }

    public void AffichageSuppMinerai(string suppMinerai)
    {
        supMineraiTxt.text = suppMinerai;
    }

    public void AffichageSuppEnergie(string suppEnergie)
    {
        supEnergieTxt.text = suppEnergie;
    }

    private void MAJNbVaisseaux(RessourcesJoueur ressourcesJoueur)
    {
        // on recupere le panel info du SSol ou se localise la production de vaisseaux
        Coordonnees localisationPdtionVaisseaux = ressourcesJoueur.localisationProductionVaisseau;

        // on recupere le nb des Forces alliées dans le ssol concerné (depuis le tablo infoSSOl)
        int nbForcesAllieesDansSystm = GameStatic.tabloInfosSystemeSolaire[localisationPdtionVaisseaux.indexLigneListSS][localisationPdtionVaisseaux.indexColonneListSS].forceAllies;

        // si il n'y en avait pas dejà dans le systeme 
        if (nbForcesAllieesDansSystm == 0)
        {
            // on crée l'index de l'escouade
            int index = GameStatic.listeTroupesAlliees.Count;
            // on crée une nouvelle escouade/ troupe
            GameStatic.listeTroupesAlliees.Add(new Troupe(new Coordonnees(localisationPdtionVaisseaux.indexLigneListSS, localisationPdtionVaisseaux.indexColonneListSS), index));

            // on sauvegarde le nb de forces créées dans le systeme ou est produit le vaisseau dans GameStatic.listInfosSystemeSolaire
            GameStatic.tabloInfosSystemeSolaire[localisationPdtionVaisseaux.indexLigneListSS][localisationPdtionVaisseaux.indexColonneListSS].forceAllies += ressourcesJoueur.forceVaisseaux;

            // on affiche les infos dans le panel infos ressources du ssol concerné
            galaxieRenderer.MAJAffichageNbTroupesAlliees(localisationPdtionVaisseaux, ressourcesJoueur.forceVaisseaux);
        }
        else
        {
            // calcul de la force totale de l'escouade
            nbForcesAllieesDansSystm += ressourcesJoueur.forceVaisseaux;

            // on sauvegarde le nb de forces alliées dans GameStatic.listInfosSystemeSolaire
            GameStatic.tabloInfosSystemeSolaire[localisationPdtionVaisseaux.indexLigneListSS][localisationPdtionVaisseaux.indexColonneListSS].forceAllies += ressourcesJoueur.forceVaisseaux;

            // on affiche les infos dans le panel infos ressources du ssol concerné
            galaxieRenderer.MAJAffichageNbTroupesAlliees(localisationPdtionVaisseaux, nbForcesAllieesDansSystm);
        }
    }

    public void AjoutRessourcesSysteme(Coordonnees localisation)
    {
        // on recupere la liste des ressources du ssol
        List<Ressource> ressList = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].listRessources;

        // pour chaque item
        foreach (Ressource ress in ressList)
        {
            switch (ress.codeRessource)
            {
                // si cest d l'energie
                case 0:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur
                    GameStatic.ressourcesJoueur.supEnergie += ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppEnergie(GameStatic.ressourcesJoueur.supEnergie.ToString());
                    break;
                // si minerai
                case 1:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur
                    GameStatic.ressourcesJoueur.supMinerai += ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppMinerai(GameStatic.ressourcesJoueur.supMinerai.ToString());
                    break;
                // si alliage
                case 2:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur
                    GameStatic.ressourcesJoueur.supAlliage += ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppAlliage(GameStatic.ressourcesJoueur.supAlliage.ToString());
                    break;
                // si science           
                case 3:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur.supp
                    GameStatic.ressourcesJoueur.supScience = GameStatic.ressourcesJoueur.supScience + ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppScience(GameStatic.ressourcesJoueur.supScience.ToString());
                    break;
                default:
                    break;

            }
        }
    }

    public void RetraitRessourcesSysteme(Coordonnees localisation)
    {
        // on recupere la liste des ressources du ssol
        List<Ressource> ressList = GameStatic.tabloInfosSystemeSolaire[localisation.indexLigneListSS][localisation.indexColonneListSS].listRessources;

        // pour chaque item
        foreach (Ressource ress in ressList)
        {
            switch (ress.codeRessource)
            {
                // si cest d l'energie
                case 0:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur
                    GameStatic.ressourcesJoueur.supEnergie -= ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppEnergie(GameStatic.ressourcesJoueur.supEnergie.ToString());
                    break;
                // si minerai
                case 1:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur
                    GameStatic.ressourcesJoueur.supMinerai -= ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppMinerai(GameStatic.ressourcesJoueur.supMinerai.ToString());
                    break;
                // si alliage
                case 2:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur
                    GameStatic.ressourcesJoueur.supAlliage -= ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppAlliage(GameStatic.ressourcesJoueur.supAlliage.ToString());
                    break;
                // si science           
                case 3:
                    // je calcul et sauvegarde dans GameStatic.ressourcesJoueur.supp
                    GameStatic.ressourcesJoueur.supScience = GameStatic.ressourcesJoueur.supScience - ress.quantite;
                    // affichage panel ressources haut
                    AffichageSuppScience(GameStatic.ressourcesJoueur.supScience.ToString());
                    break;
                default:
                    break;
            }
        }
    }

}
