using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using classDuJeu;
using System;

public class GestionBtnsAction : MonoBehaviour
{
    public Material transparentMaterial;
    public GameObject textPdtionVaisseau;
    public GameObject btnPdtionVaisseau;
    public GameObject btnsActionSSol;
    public GameObject panelDivisionTroupes;
    public GameObject panelGestionSysteme;
    public GameObject panelPartieSysteme;
    public GameObject panelPartiePlanete;
    public GameObject panelConstruction;
    public GameObject panelDestruction;
    public GameObject btnGestionplanet;
    public GameObject btnUpgradeBase;

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
    /*
    public Button btnPause;
    public Button btnPlay;
    public Button btnSpeed1;
    public Button btnSpeed2;
    */
    public Text textTroupeRestante;
    public Text textForceBase;
    public Text textTroupePartante;
    public Text textCoutBase;
    public float posXBtnsCaches = -4000f;
    public float posYBtnsCaches = 1200f;

    public void Pause()
    {
        // je met le jeu en pause
        GameStatic.accelerateurTemps = 0;
    }

    public void Play()
    {
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF = 1;
    }

    public void Speed()
    {
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF = GameStatic.TIMESPEED1;
    }

    public void Speed2()
    {
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF = GameStatic.TIMESPEED2;
    }

    public void Annuler()
    {
        // je remet les btns d'action sur le coté
        btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);
    }

    public void DivisionDesTroupes()
    {
        // je remet les btns d'action sur le coté
        btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je met le jeu en pause
        GameStatic.accelerateurTemps = 0;
        GameStatic.pauseModeEdition = true;

        CalculNbTroupesApresDivision();

        // je montre le panel Division troupes
        panelDivisionTroupes.gameObject.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    }

    private void CalculNbTroupesApresDivision()
    {
        //on recupere le ssol precedemment cliqué
        int indexLigne = GameStatic.ssolClicked.indexLigneListSS;
        int indexColonne = GameStatic.ssolClicked.indexColonneListSS;

        // sa force alliée
        int forceTroupe = GameStatic.tabloInfosSystemeSolaire[indexLigne][indexColonne].forceAllies;

        // on calcul et affiche la force des troupes
        CalculAffichageForceTroupe(forceTroupe);
    }

    private void CalculAffichageForceTroupe(int forceTroupe)
    {
        if (forceTroupe % 2 == 0)
        {// pair
            int moyenneForceTroupe = forceTroupe / 2;
            // calcul et sauvegarde force de chaque troupe
            GameStatic.forceTroupe1Divs = moyenneForceTroupe;
            GameStatic.forceTroupe2Divs = moyenneForceTroupe;

            //affichage
            textTroupePartante.text = moyenneForceTroupe.ToString();
            textTroupeRestante.text = moyenneForceTroupe.ToString();
        }
        else
        {
            // chiffre impair
            int temp = forceTroupe / 2;
            int temp2 = temp + 1;
            // calcul et sauvegarde force de chaque troupe
            GameStatic.forceTroupe1Divs = temp;
            GameStatic.forceTroupe2Divs = temp2;

            //affichage
            textTroupePartante.text = temp.ToString();
            textTroupeRestante.text = temp2.ToString();
        }
    }

    public void AugmenterTroupesEnPartance()
    {
        if (GameStatic.forceTroupe2Divs > 1)
        {
            GameStatic.forceTroupe1Divs += 1;
            GameStatic.forceTroupe2Divs -= 1;

            //affichage
            textTroupePartante.text = GameStatic.forceTroupe1Divs.ToString();
            textTroupeRestante.text = GameStatic.forceTroupe2Divs.ToString();
        }
    }

    public void DiminuerTroupesEnPartance()
    {
        if (GameStatic.forceTroupe1Divs > 1)
        {
            GameStatic.forceTroupe1Divs -= 1;
            GameStatic.forceTroupe2Divs += 1;

            //affichage
            textTroupePartante.text = GameStatic.forceTroupe1Divs.ToString();
            textTroupeRestante.text = GameStatic.forceTroupe2Divs.ToString();
        }
    }

    public void AnnulerDivisionTroupe()
    {
        // je remet le panelDivisionTroupes sur le coté
        panelDivisionTroupes.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je met le jeu en route
        GameStatic.pauseModeEdition = false;
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }

    public void ChoisirDirectionApresDivision()
    {
        // je remet le panelDivisionTroupes sur le coté
        panelDivisionTroupes.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        GameStatic.pauseModeEdition = false;
        GameStatic.divisionTroupe = true;
    }

    public void GererSysteme()
    {
        GameStatic.clickDeplacement = false;

        // je recupere le systeme cliqué 
        int indexLigne = GameStatic.ssolClicked.indexLigneListSS;
        int indexColonne = GameStatic.ssolClicked.indexColonneListSS;

        VerifEtActivationPlanete(indexLigne, indexColonne);

        VerifEtActivationPdtionVaisseaux(indexLigne, indexColonne);

        // j affiche la force de la base dans le panel gestion systeme
        textForceBase.text = GameStatic.tabloInfosSystemeSolaire[indexLigne][indexColonne].forceBase.ToString();
        VerifEtActivationBtnForceBase();

        DesactiverPanelsConstructionDemolition();

        // je remet les btnsActionSSol sur le coté
        btnsActionSSol.gameObject.transform.position = new Vector3(posXBtnsCaches, posYBtnsCaches, 0);

        // je met le jeu en pause
        GameStatic.accelerateurTemps = 0;
        GameStatic.pauseModeEdition = true;

        // je montre le panel Gestion systeme
        panelGestionSysteme.gameObject.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    }

    private void DesactiverPanelsConstructionDemolition()
    {
        panelConstruction.SetActive(false);
        panelDestruction.SetActive(false);
    }

    public void VerifEtActivationBtnForceBase()
    {
        // si les reserves de minerais sup a PRIXUPGRADEBASE      
        if (GameStatic.ressourcesJoueur.mineraiTotal >= GameStatic.ressourcesJoueur.prixUpgradeBase)
        {
            // on active btn upgrade
            btnUpgradeBase.gameObject.SetActive(true);
            // on met a jour le cout de l'upgrade
            textCoutBase.text = GameStatic.ressourcesJoueur.prixUpgradeBase.ToString();
        }
        else
        {
            // on desactive btn upgrade
            btnUpgradeBase.gameObject.SetActive(false);
        }
    }

    private void VerifEtActivationPdtionVaisseaux(int indexLigne, int indexColonne)
    {
        // si la pdtion de vaisseaux se fait dans le systeme
        if (PductionVaisseauDansSSol(indexLigne, indexColonne))
        {
            // on affiche le texte
            textPdtionVaisseau.gameObject.SetActive(true);
            // on cache le btn
            btnPdtionVaisseau.gameObject.SetActive(false);
        }
        else
        {
            // sinon on affiche le btn pour localiser la pdtion là
            btnPdtionVaisseau.gameObject.SetActive(true);
            // on cache le txt
            textPdtionVaisseau.gameObject.SetActive(false);
        }
    }

    private static bool PductionVaisseauDansSSol(int indexLigne, int indexColonne)
    {
        return GameStatic.ressourcesJoueur.localisationProductionVaisseau.indexLigneListSS == indexLigne && GameStatic.ressourcesJoueur.localisationProductionVaisseau.indexColonneListSS == indexColonne;
    }

    private void VerifEtActivationPlanete(int indexLigne, int indexColonne)
    {
        // si il ya une planete dans le systeme
        if (GameStatic.tabloInfosSystemeSolaire[indexLigne][indexColonne].isPlanete)
        {
            // j'affiche le btn gestion planete
            btnGestionplanet.gameObject.SetActive(true);
            MAJbtnsEmplacement(indexLigne, indexColonne);
        }
        else
        {
            // sinon je le cache
            btnGestionplanet.gameObject.SetActive(false);
            // je desactive le panel correspondant
            panelPartieSysteme.SetActive(true);
            panelPartiePlanete.SetActive(false);
        }
    }

    ///<summary> je met a jour les empacements</summary>
    private void MAJbtnsEmplacement(int indexLigne, int indexColonne)
    {
        Planete planet = GameStatic.tabloInfosSystemeSolaire[indexLigne][indexColonne].planet;

        // pour chaque emplacement
        // je met a jour l'affichage des emplacements
        MAJEmplacement0(planet.emplacement0);
        MAJEmplacement1(planet.emplacement1);
        MAJEmplacement2(planet.emplacement2);
        MAJEmplacement3(planet.emplacement3);
        MAJEmplacement4(planet.emplacement4);
        MAJEmplacement5(planet.emplacement5);
        MAJEmplacement6(planet.emplacement6);
        MAJEmplacement7(planet.emplacement7);
        MAJEmplacement8(planet.emplacement8);
    }

    private void MAJEmplacement8(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement8.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement8.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement8.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement7(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement7.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement7.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement7.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement6(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement6.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement6.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement6.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement5(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement5.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement5.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement5.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement4(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement4.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement4.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement4.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement3(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement3.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement3.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement3.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement2(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement2.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement2.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement2.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement1(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement1.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement1.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement1.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }

    private void MAJEmplacement0(int emplacement)
    {
        switch (emplacement)
        {
            case 0:// si il y a rien j'affiche rien en image de fond
                btnEmplacement0.image.sprite = blanc;
                break;
            case 1:// si il y a une usine d'alliage
                btnEmplacement0.image.sprite = usineAlliage;
                break;
            case 2:// si il y a un centre scientifique
                btnEmplacement0.image.sprite = centreScientifique;
                break;
            default:
                break;
        }
    }
}
