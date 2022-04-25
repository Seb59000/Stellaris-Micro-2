using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu{
    public class Connexion
    {
            public Vector3 position1 { get; set; }
            public Vector3 position2 { get; set; }
            public Connexion(Vector3 position1, Vector3 position2)
            {
                this.position1 = position1;
                this.position2 = position2;
            }
    }
}

