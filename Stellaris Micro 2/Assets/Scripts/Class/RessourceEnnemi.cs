using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class RessourceEnnemi
    {
        public int energieTotal { get; set; }
        public int mineraiTotal { get; set; }
        public int alliageTotal { get; set; }
        public int science { get; set; }
        public int supEnergie { get; set; }
        public int supMinerai { get; set; }
        public int supAlliage { get; set; }
        public int supScience { get; set; }
        public int prixVaisseau { get; set; }
        public int forceVaisseaux { get; set; }

        public RessourceEnnemi()
        {
            this.energieTotal = 50;
            this.mineraiTotal = 250;
            this.alliageTotal = 50;
            this.science = 2;
            this.supEnergie = 1;
            this.supMinerai = 0;
            this.supAlliage = GameStatic.SUPPALLIAGE;
            this.supScience = GameStatic.SUPPSCIENCE;
            this.forceVaisseaux = 2;
            this.prixVaisseau = 70;
        }
    }
}

