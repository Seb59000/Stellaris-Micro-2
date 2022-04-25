using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class AlerteAttaque : MonoBehaviour
{
    // Start is called before the first frame update
    public void MessageRecu()
    {
        GameStatic.accelerateurTemps = GameStatic.accelerateurTempsPREF;
    }
}
