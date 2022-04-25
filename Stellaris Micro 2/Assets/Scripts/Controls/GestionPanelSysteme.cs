using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using classDuJeu;
using System;

[RequireComponent(typeof(GalaxieRenderer))]
[RequireComponent(typeof(RessourcesManager))]
public class GestionPanelSysteme : MonoBehaviour
{
    protected GalaxieRenderer galaxieRenderer;
    protected RessourcesManager ressourcesManager;
    public GameObject panelGestionSysteme;
    public GameObject btnPdtionVaisseau;
    public GameObject textPdtionVaisseau;
    public GameObject partieUpgradeBase;
    public GameObject partieConstructionBatiment;
    public GameObject partieDemolitionBatiment;

    public Sprite blanc;
    public Sprite usineAlliage;
    public Sprite centreScientifique;

    public Button btnEmplacement0;
    public Button btnEmplacement1;
    public Button btnEmplacement2;
    public Button btnEmplacement3;
    public Button btnEmplacement4;
    public Button btnEmplacement5;
    public Button btnEmplacement6;
    public Button btnEmplacement7;
    public Button btnEmplacement8;

    public Button btnConstructionUsineAlliage;
    public Button btnConstructionCentreScien;

    public Text textDemolition;
    public Text textForceBase;

    public float posXBtnsCaches = -4000f;
    public float posYBtnsCaches = 1200f;

    void Awake()
    {
        galaxieRenderer = GetComponent<GalaxieRenderer>();
        ressourcesManager = GetComponent<RessourcesManager>();
    }

    public void Quitter()
    {
        // je remet le panelGestionSysteme sur le coté
        panelGestionSysteme.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je met le jeu en route
        GameStatic.pauseModeEdition = false;
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }

    public void Emplacement0()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement0;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(0, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(0, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(0, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement1()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement1;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);
                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(1, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(1, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(1, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement2()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement2;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);
                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(2, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(2, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(2, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement3()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement3;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(3, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(3, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(3, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement4()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement4;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(4, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(4, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(4, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement5()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement5;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(5, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(5, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(5, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement6()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement6;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(6, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(6, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(6, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement7()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement7;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(7, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(7, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(7, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Emplacement8()
    {
        int emplacement = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement8;
        switch (emplacement)
        {
            case 0:// si il y a rien
                   // j'affiche le panel construction
                partieConstructionBatiment.gameObject.SetActive(true);
                partieDemolitionBatiment.gameObject.SetActive(false);

                // si ressources insuffisantes je grise les boutons
                if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.PRIXCONSTRUCBATIMENT)
                {
                    btnConstructionUsineAlliage.interactable = false;
                    btnConstructionCentreScien.interactable = false;
                }
                else
                {
                    btnConstructionCentreScien.interactable = true;
                    btnConstructionUsineAlliage.interactable = true;
                }

                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(8, 0);
                break;
            case 1:// sinon le panel destruction
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement a ete selectionné et ce qu'il contient pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(8, 1);
                textDemolition.text = "Usine d'Alliage";
                break;
            case 2:
                partieConstructionBatiment.gameObject.SetActive(false);
                partieDemolitionBatiment.gameObject.SetActive(true);
                // on sauvegarde quel emplacement et ce qu'il contient a ete selectionné pour l'edition après
                GameStatic.emplacementSelectionne = new Emplacement(8, 2);
                textDemolition.text = "Centre scientifique";
                break;
            default:
                break;
        }
    }

    public void Demolir()
    {
        // je recupere l'emplacement a demolir
        Emplacement emplacement = GameStatic.emplacementSelectionne;

        switch (emplacement.codeBatiment)
        {
            case 1:
                // si le code batiment est 1 demolition usine d'alliage
                MAJRessDemolitionUsineAlliage();
                //affichage et sauvegarde emplacement vide de la planete qui se trouve dans le ssolClicked
                DemolitionEmplacement(emplacement.index);
                break;
            case 2:
                // si centre sc 
                MAJRessDemolitionCentreScientifique();
                //sauvegarde emplacement vide de la planete qui se trouve dans le ssolClicked
                DemolitionEmplacement(emplacement.index);
                break;
            default:
                break;
        }
        // je sauvegarde dans gamestatic emplacement planete du ssol cliqué

        // je fais didparaiter le panel demolition
        partieDemolitionBatiment.SetActive(false);

        // je fais apparaitre la parte construction
        partieDemolitionBatiment.SetActive(false);
    }

    private void MAJRessDemolitionCentreScientifique()
    {
        // - 2 en sup sc
        GameStatic.ressourcesJoueur.supAlliage -= GameStatic.SUPPSCIENCE;
        // on affiche MAJ
        ressourcesManager.AffichageSuppScience(GameStatic.ressourcesJoueur.supAlliage.ToString());

        // +4 en sup energie
        GameStatic.ressourcesJoueur.supEnergie += GameStatic.CONSOSCIENCE;
        // on affiche MAJ
        ressourcesManager.AffichageSuppEnergie(GameStatic.ressourcesJoueur.supEnergie.ToString());

        // rajoute 100 en minerai
        GameStatic.ressourcesJoueur.mineraiTotal += GameStatic.SUPMINERAIDESTRUCBATIMENT;
        // on affiche MAJ
        ressourcesManager.AffichageTotMinerai(GameStatic.ressourcesJoueur.mineraiTotal.ToString());
    }

    private void DemolitionEmplacement(int indexEmplacement)
    {// affichage et sauvegarde espace libre
        switch (indexEmplacement)
        {
            case 0:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement0 = 0;
                // affichage espace libre
                btnEmplacement0.image.sprite = blanc;
                break;
            case 1:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement1 = 0;
                // affichage espace libre
                btnEmplacement1.image.sprite = blanc;
                break;
            case 2:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement2 = 0;
                // affichage espace libre
                btnEmplacement2.image.sprite = blanc;
                break;
            case 3:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement3 = 0;
                // affichage espace libre
                btnEmplacement3.image.sprite = blanc;
                break;
            case 4:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement4 = 0;
                // affichage espace libre
                btnEmplacement4.image.sprite = blanc;
                break;
            case 5:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement5 = 0;
                // affichage espace libre
                btnEmplacement5.image.sprite = blanc;
                break;
            case 6:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement6 = 0;
                // affichage espace libre
                btnEmplacement6.image.sprite = blanc;
                break;
            case 7:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement7 = 0;
                // affichage espace libre
                btnEmplacement7.image.sprite = blanc;
                break;
            case 8:
                //on rentre le code zero a l'emplacement x de la planete qui se trouve dans le ssolClicked
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement8 = 0;
                // affichage espace libre
                btnEmplacement8.image.sprite = blanc;
                break;
            default:
                break;
        }
    }

    private void MAJRessDemolitionUsineAlliage()
    {
        // j'enleve 10 en sup alliage
        GameStatic.ressourcesJoueur.supAlliage = GameStatic.ressourcesJoueur.supAlliage - GameStatic.SUPPALLIAGE;
        // on affiche MAJ
        ressourcesManager.AffichageSuppAlliage(GameStatic.ressourcesJoueur.supAlliage.ToString());

        // rajoute +4 en sup energie
        GameStatic.ressourcesJoueur.supEnergie += GameStatic.CONSOALLIAGEnergie;
        // on affiche MAJ
        ressourcesManager.AffichageSuppEnergie(GameStatic.ressourcesJoueur.supEnergie.ToString());

        // rajoute +4 en sup minerai
        GameStatic.ressourcesJoueur.supMinerai += GameStatic.CONSOALLIAGEMinerai;
        // on affiche MAJ
        ressourcesManager.AffichageSuppMinerai(GameStatic.ressourcesJoueur.supMinerai.ToString());

        // rajoute 100 en minerai
        GameStatic.ressourcesJoueur.mineraiTotal += GameStatic.SUPMINERAIDESTRUCBATIMENT;
        // on affiche MAJ
        ressourcesManager.AffichageTotMinerai(GameStatic.ressourcesJoueur.mineraiTotal.ToString());
    }

    public void ConstrucUsineAlliage()
    {
        // sauvegarde et mise a jour emlacement
        // je recupere l'index de l'emplacement cliqué (sa place sur l'echiquier)
        switch (GameStatic.emplacementSelectionne.index)
        {
            case 0:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement0 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement0.image.sprite = usineAlliage;
                break;
            case 1:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement1 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement1.image.sprite = usineAlliage;
                break;
            case 2:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement2 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement2.image.sprite = usineAlliage;
                break;
            case 3:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement3 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement3.image.sprite = usineAlliage;
                break;
            case 4:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement4 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement4.image.sprite = usineAlliage;
                break;
            case 5:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement5 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement5.image.sprite = usineAlliage;
                break;
            case 6:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement6 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement6.image.sprite = usineAlliage;
                break;
            case 7:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement7 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement7.image.sprite = usineAlliage;
                break;
            case 8:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement8 = 1;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement8.image.sprite = usineAlliage;
                break;
            default:
                break;

        }
        // MAJ ressources
        GameStatic.ressourcesJoueur.mineraiTotal -= GameStatic.PRIXCONSTRUCBATIMENT;
        // affichage
        ressourcesManager.AffichageTotMinerai(GameStatic.ressourcesJoueur.mineraiTotal.ToString());
        // on grise les btns de construction vu qu'on ne peut plus construir à cet emplacement
        btnConstructionCentreScien.interactable = false;
        btnConstructionUsineAlliage.interactable = false;
    }


    public void ConstrucCentreScientifique()
    {
        // sauvegarde et mise a jour emlacement
        // je recupere l'index de l'emplacement cliqué (sa place sur l'echiquier)
        switch (GameStatic.emplacementSelectionne.index)
        {
            case 0:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement0 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement0.image.sprite = centreScientifique;
                break;
            case 1:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement1 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement1.image.sprite = centreScientifique;
                break;
            case 2:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement2 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement2.image.sprite = centreScientifique;
                break;
            case 3:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement3 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement3.image.sprite = centreScientifique;
                break;
            case 4:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement4 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement4.image.sprite = centreScientifique;
                break;
            case 5:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement5 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement5.image.sprite = centreScientifique;
                break;
            case 6:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement6 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement6.image.sprite = centreScientifique;
                break;
            case 7:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement7 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement7.image.sprite = centreScientifique;
                break;
            case 8:
                // je sauvegarde le code 1 a son emplacement sur sa planete
                GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].planet.emplacement8 = 2;
                // j'affiche les MAJ sur le btn du menu planete
                btnEmplacement8.image.sprite = centreScientifique;
                break;
            default:
                break;

        }
        // MAJ ressources
        GameStatic.ressourcesJoueur.mineraiTotal -= GameStatic.PRIXCONSTRUCBATIMENT;
        // affichage
        ressourcesManager.AffichageTotMinerai(GameStatic.ressourcesJoueur.mineraiTotal.ToString());
        // on grise les btns de construction vu qu'on ne peut plus construir à cet emplacement
        btnConstructionCentreScien.interactable = false;
        btnConstructionUsineAlliage.interactable = false;
    }

    public void Activer()
    {

    }

    public void Desactiver()
    {

    }

    public void RenforcerBase()
    {
        // on retire PRIXUPGRADEBASE au total de minerai
        GameStatic.ressourcesJoueur.mineraiTotal -= GameStatic.ressourcesJoueur.prixUpgradeBase;

        // on affiche la MAJ sur le menu du haut
        ressourcesManager.AffichageTotMinerai(GameStatic.ressourcesJoueur.mineraiTotal.ToString());

        // on upgrade la base sur listinfos systeme
        int forcesBase = GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].forceBase;
        forcesBase = forcesBase + GameStatic.UPGRADEBASE;
        GameStatic.tabloInfosSystemeSolaire[GameStatic.ssolClicked.indexLigneListSS][GameStatic.ssolClicked.indexColonneListSS].forceBase = forcesBase;

        // on affiche les modifs sur le panelInfo du ssol 
        galaxieRenderer.MAJAffichageForceBase(GameStatic.ssolClicked, forcesBase);

        // on affiche les modifs dans le panel gestion systeme  
        textForceBase.text = forcesBase.ToString();

        // si les reserves de minerais sup a PRIXUPGRADEBASE      
        if (GameStatic.ressourcesJoueur.mineraiTotal < GameStatic.ressourcesJoueur.prixUpgradeBase)
        {
            // on desactive btn upgrade
            partieUpgradeBase.gameObject.SetActive(false);
        }
    }

    public void ProduireVaisseauxIci()
    {
        // on sauvegarde la pdtion de vaisseaux ici
        GameStatic.ressourcesJoueur.localisationProductionVaisseau = new Coordonnees(GameStatic.ssolClicked.indexLigneListSS, GameStatic.ssolClicked.indexColonneListSS);

        // on cache le btn pour localiser la pdtion là
        btnPdtionVaisseau.gameObject.SetActive(false);
        // on affiche le txt
        textPdtionVaisseau.gameObject.SetActive(true);
    }
}
