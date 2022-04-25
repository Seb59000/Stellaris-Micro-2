using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class SsolAtteignable
    {
        public bool rejete { get; set; }
        public Coordonnees localisation;

        public SsolAtteignable(int indexLigne, int indexColonne)
        {
            this.rejete = false;
            this.localisation = new Coordonnees(indexLigne, indexColonne);
        }

        public SsolAtteignable()
        {
            this.rejete = false;
            this.localisation = new Coordonnees();
        }
    }
}


