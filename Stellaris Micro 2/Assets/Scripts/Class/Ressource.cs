using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class Ressource
    {

        /*
        le code pour les ressources est le suivant:
        0 energie
        1 minerais
        2 alliages
        3 sc phy    !!!
        4 planete !!!
        5 particules volatiles
        6 sc so
        7 cristaux rares  !!!
        8 ingenierie
        9 commerce   
        10 gaz rare !!!
        11 anomalie
        */
        public int codeRessource { get; set; }
        public int quantite { get; set; }
        public Ressource(int codeRessource, int quantite)
        {
            this.codeRessource = codeRessource;
            this.quantite = quantite;
        }
    }
}

