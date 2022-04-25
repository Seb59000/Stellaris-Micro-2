using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class Coordonnees
    {
        public int indexLigneListSS { get; set; }
        public int indexColonneListSS { get; set; }
        public Coordonnees(int indexLigne, int indexColonne)
        {
            this.indexLigneListSS = indexLigne;
            this.indexColonneListSS = indexColonne;
        }

        public Coordonnees()
        {
            this.indexLigneListSS = 0;
            this.indexColonneListSS = 0;
        }

    }
}

