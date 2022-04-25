using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class InfosNavigation
    {
        public List<Coordonnees> SSAtteignables { get; set; }
        public bool rejete { get; set; }
        public bool culdesac { get; set; }


        public InfosNavigation()
        {
            this.culdesac = false;
            this.rejete = false;
            SSAtteignables = new List<Coordonnees>();
        }
    }
}
