using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class TroupeEnnemi
    {
        public int blocked { get; set; }
       // public bool enGuerre { get; set; }
        public Coordonnees localisation { get; set; }
        public Coordonnees cible { get; set; }
        public Itineraire itineraire { get; set; }

        public TroupeEnnemi(int nbTroupe, Coordonnees localisation)
        {
            this.blocked = 0;
           // this.enGuerre = false;
            this.localisation = localisation;
            this.itineraire = new Itineraire();
        }
    }
}


