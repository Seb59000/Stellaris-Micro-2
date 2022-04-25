using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class ScienceManager : MonoBehaviour
{
    public void BaissePrixVaisseaux()
    {
        // on diminue le prix des vaisseaux
        GameStatic.ressourcesJoueur.prixVaisseau -= GameStatic.ressourcesJoueur.baissePrixVaisseaux;
        // aprés chaque baisse on rend la prochaine baisse moins importante
        if (GameStatic.ressourcesJoueur.baissePrixVaisseaux>1)
        {
            GameStatic.ressourcesJoueur.baissePrixVaisseaux--;
        }
        // a chaque avancée tech on rend la science plus chère
        GameStatic.ressourcesJoueur.prixScience += GameStatic.HAUSSEPRIXSCIENCE;

        // on remet le temps en marche
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }
    public void AugmenterPuissanceVaisseaux()
    {
        // on remet le prix des vaisseaux comme au debut
        GameStatic.ressourcesJoueur.prixVaisseau = GameStatic.PRIXVAISSEAUXDEBUT;

        // on augmente la force des vaisseaux
        GameStatic.ressourcesJoueur.forceVaisseaux+=2;

        // a chaque avancée tech on rend la science plus chère
        GameStatic.ressourcesJoueur.prixScience += GameStatic.HAUSSEPRIXSCIENCE;

        // on remet le temps en marche
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }

    public void BaissePrixUpgradeBase()
    {
        // on diminue le prix des upgrades de base
        GameStatic.ressourcesJoueur.prixUpgradeBase -= GameStatic.BAISSEPRIXBASE;

        // a chaque avancée tech on rend la science plus chère
        GameStatic.ressourcesJoueur.prixScience += GameStatic.HAUSSEPRIXSCIENCE;

        // on remet le temps en marche
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }
}
