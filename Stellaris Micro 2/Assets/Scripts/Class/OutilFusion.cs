using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class OutilFusion
    {
        public Coordonnees coordonnees { get; set; }
        //public int forces { get; set; }
        public int index { get; set; }
        public OutilFusion(Coordonnees coordonnees, int index)
        {
            this.coordonnees = coordonnees;
          //  this.forces = forces;
            this.index = index;
        }
    }
}

