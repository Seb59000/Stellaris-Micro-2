using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class StaticEnnemi
    {
        public const int RAPIDITEDEPLACEMENTENNEMI = 40;
       // public const int RAPIDITEDEPLACEMENTTroupeENNEMI = 5;
        public const int RYTHMECOLONISATIONIA = 3;

        // chaque ennemi a un EnnemiData
        public static List<EnnemiData> listEnnemiData = new List<EnnemiData>();

        // tous les ennemis se déplacent à la meme vitesse
       // public static int cptrDeplacementEnnemi = 0;
       // public static int cptrDeplacementTroupesEnnemi = 0;
    }
}


