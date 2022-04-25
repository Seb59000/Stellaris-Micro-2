using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class RessourcesJoueur
    {
        public int energieTotal { get; set; }
        public int mineraiTotal { get; set; }
        public int alliageTotal { get; set; }
        public int science { get; set; }
        public int supEnergie { get; set; }
        public int supMinerai { get; set; }
        public int supAlliage { get; set; }
        public int supScience { get; set; }
        public int nbUsineAlliage { get; set; }
        public Coordonnees localisationUsineAlliage { get; set; }
        public int nbCentreRecherche { get; set; }
        public Coordonnees localisationCentreRecherche { get; set; }
        public Coordonnees localisationProductionVaisseau { get; set; }
        public int prixScience { get; set; }
        public int prixUpgradeBase { get; set; }
        public int baissePrixVaisseaux { get; set; }
        public int prixVaisseau { get; set; }
        public int forceVaisseaux { get; set; }
        public List<Coordonnees> ssolPossedes { get; set; }

        public RessourcesJoueur()
        {
            this.energieTotal = 50;
            this.mineraiTotal = 250;
            this.alliageTotal = 50;
            this.science = 2;
            this.supEnergie = 1;
            this.supMinerai = 0;
            this.supAlliage = GameStatic.SUPPALLIAGE;
            this.supScience = GameStatic.SUPPSCIENCE;
            this.nbUsineAlliage = 1;
            this.nbCentreRecherche = 1;
            this.prixScience = 100;
            this.prixVaisseau = 70;
            this.prixUpgradeBase = 200;
            this.baissePrixVaisseaux = 20;
            this.forceVaisseaux = GameStatic.FORCEVAISSEAUXDEBUT;
            this.ssolPossedes = new List<Coordonnees>();
        }
    }
}

