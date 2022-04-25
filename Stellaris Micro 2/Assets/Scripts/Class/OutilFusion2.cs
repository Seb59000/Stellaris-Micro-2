using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class OutilFusion2
    {
        public Coordonnees localisation { get; set; }
        public int forcesFinales { get; set; }

        public OutilFusion2(Coordonnees localisation, int forcesFinales)
        {
            this.localisation = localisation;
            this.forcesFinales = forcesFinales;
        }


    }
}
