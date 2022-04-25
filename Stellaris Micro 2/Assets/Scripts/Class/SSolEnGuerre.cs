using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class SSolEnGuerre
    {
        public Coordonnees localisation { get; set; }
        public int codeAttaquant { get; set; }
        public int codeDefenseur { get; set; }
        /// <summary>
        /// le code est zero pour le joueur
        /// (ou proprietaire -1)
        /// et indexEnnemi +1 
        /// </summary>
        /// <param name="localisation"></param>
        /// <param name="codeAttaquant"></param>
        /// <param name="codeDefenseur"></param>
        public SSolEnGuerre(Coordonnees localisation, int codeAttaquant, int codeDefenseur)
        {
            this.localisation = localisation;
            this.codeAttaquant = codeAttaquant;
            this.codeDefenseur = codeDefenseur;
        }
    }
}

