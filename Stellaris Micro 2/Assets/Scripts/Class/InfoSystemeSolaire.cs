using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class InfoSystemeSolaire
    {
        public int proprietaire { get; set; }
        public int forceBase { get; set; }
        public int forceAllies { get; set; }
        public int forceEnnemies { get; set; }
        public bool isAtteignable { get; set; }
        public int indexLigneListSS { get; set; }
        public int indexColonneListSS { get; set; }
        public Planete planet { get; set; }
        public bool isPlanete { get; set; }
        public bool isScanne { get; set; }
        public bool enGuerre { get; set; }
        public int avanceeScan { get; set; }
        public Coordonnees direction { get; set; }

        public List<Ressource> listRessources { get; set; }

        public InfoSystemeSolaire(int indexLigne, int indexColonne)
        {
            this.proprietaire = 0;
            this.forceAllies = 0;
            this.isAtteignable = false;
            this.indexLigneListSS = indexLigne;
            this.indexColonneListSS = indexColonne;
            this.isPlanete = false;
            this.enGuerre = false;
            this.isScanne = false;
            this.avanceeScan = 0;
            this.direction = new Coordonnees(indexLigne, indexColonne);
            listRessources = new List<Ressource>();
        }
    }
}

