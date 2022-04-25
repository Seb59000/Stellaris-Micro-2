using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class Troupe
    {
        public int index { get; set; }
     //   public int nbTroupe { get; set; }
        public Coordonnees localisation { get; set; }
        public Coordonnees systemeDepart { get; set; }
        public bool entree { get; set; }
       // public bool fuite { get; set; }
        public int avancement { get; set; }

        public Troupe(Coordonnees localisation, int index)
        {
            this.index = index;
           // this.nbTroupe = nbTroupe;
            this.localisation = localisation;
            this.systemeDepart = localisation;
            this.entree = false;
         //   this.fuite = false;
            this.avancement = 0;
        }

        public Troupe(Coordonnees localisation, Coordonnees systemeDepart, int avancement)
        {
            this.index = 0;
           // this.nbTroupe = nbTroupe;
            this.localisation = localisation;
            this.systemeDepart = systemeDepart;
            this.entree = true;
           // this.fuite = false;
            this.avancement = avancement;
        }

    }
}

