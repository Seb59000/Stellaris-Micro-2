using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class GameStatic
    {
        public const int FORCEVAISSEAUXDEBUT = 2;

        public const int HAUSSEPRIXSCIENCE = 5;
        public const int BAISSEPRIXBASE = 15;
        public const int PUISSANCEBASEDEBUT = 2;
        public const int UPGRADEBASE = 3;
        public const int PRIXCONSTRUCBATIMENT = 350;
        public const int PRIXVAISSEAUXDEBUT = 70;
        public const int SUPPALLIAGE = 10;
        public const int CONSOALLIAGEnergie = 4;
        public const int CONSOALLIAGEMinerai = 10;
        public const int SUPPSCIENCE = 2;
        public const int CONSOSCIENCE = 4;
        public const int SUPMINERAIDESTRUCBATIMENT = 100;
        public const int TIMESPEED1 = 2;
        public const int TIMESPEED2 = 3;

        public static int NBFORCESALLIEESDEBUT = 2;
        public static int PUISSANCEDESTROUPES = 1;

        // Pour chaque ligne je fait une liste de SS
        // chaque ligne est dans le grande liste↓
        public static List<List<InfoSystemeSolaire>> tabloInfosSystemeSolaire = new List<List<InfoSystemeSolaire>>();
        public static List<List<InfosNavigation>> tabloInfosNavigation = new List<List<InfosNavigation>>();

        public static List<Troupe> listeTroupesAlliees = new List<Troupe>();
        public static List<List<GameObject>> tableauSystemeSolaire = new List<List<GameObject>>();
        public static List<List<Transform>> panelTomove;
        public static List<Coordonnees> placesEmpires = new List<Coordonnees>();

        public static Coordonnees ssolClicked = new Coordonnees(0, 0);

        public static RessourcesJoueur ressourcesJoueur = new RessourcesJoueur();
        public static bool clickDeplacement = false;
        public static int accelerateurTemps = 1;
        public static int accelerateurTempsPREF = 1;
        public static bool pauseModeEdition = false;

        public static Emplacement emplacementSelectionne;

        public static bool divisionTroupe = false;
       // public static int troupeEnattenteDivision;
        public static int forceTroupe1Divs;
        public static int forceTroupe2Divs;
        public static bool enGuerre = false;
        public static int troupeEnAttenteGuerre = 0;
        public static SSolEnGuerre ssolEnAttenteGuerre;
        public static List<SSolEnGuerre> ssolEnGuerre = new List<SSolEnGuerre>();
        //public static bool isBasesAReparer = false;
        //  public static List<Coordonnees> basesAReparer = new List<Coordonnees>();
        public static bool panelDejaEnGuerreDisplayed = false;
        
    }
}
