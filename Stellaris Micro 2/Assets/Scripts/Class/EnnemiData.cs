using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

namespace classDuJeu
{
    public class EnnemiData
    {
        public int codeEnnemi { get; set; }
        public int prixScience { get; set; }
        public int baissePrixVaisseaux { get; set; }
        public int prixVaisseau { get; set; }
        public int forceVaisseaux { get; set; }
        public int prixUpgradeBase { get; set; }
        public bool logiqueDeGuerre { get; set; }
        public bool enGuerre { get; set; }
        public bool actif { get; set; }
        public int bloque { get; set; }
        //public bool jourDavanceTroupe { get; set; }

        public int cptrDeplacementEnnemi { get; set; }
        public int cptrDeplacementTroupesEnnemi { get; set; }

        public bool enDeplacementTroupe { get; set; }
        public List<Coordonnees> listCoordPossede = new List<Coordonnees>();
        public List<TroupeEnnemi> listCoordTroupes = new List<TroupeEnnemi>();
        public List<Coordonnees> listCoordCible = new List<Coordonnees>();
        public List<Coordonnees> listCoordAColoniser = new List<Coordonnees>();
        public RessourceEnnemi ressourceEnnemi = new RessourceEnnemi();

        public EnnemiData(int codeEnnemi)
        {
            this.forceVaisseaux = GameStatic.FORCEVAISSEAUXDEBUT;
            this.prixVaisseau = 70;
            this.prixUpgradeBase = 200;
            this.baissePrixVaisseaux = 20;
            this.prixScience = 100;
            this.codeEnnemi = codeEnnemi;
            this.logiqueDeGuerre = false;
            this.enGuerre = false;
            this.actif = true;
            this.bloque = 0;
            // this.jourDavanceTroupe = false;
            this.cptrDeplacementEnnemi = 0;
            this.cptrDeplacementTroupesEnnemi = 0;
        }
    }
}

