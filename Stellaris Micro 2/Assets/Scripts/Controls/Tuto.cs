using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class Tuto : MonoBehaviour
{
    public GameObject panelTuto1;
    bool dontShowAgain = false;
    public void OnToggleChanged(bool newValue)
    {
        Debug.Log("OnToggleChanged " + newValue);
        dontShowAgain = newValue;
    }

    public void SauvegarderPrefAffichTuto()
    {
        if (dontShowAgain)
        {
            PlayerPrefs.SetInt("masquageTuto", 1);
        }
        else
        {
            PlayerPrefs.SetInt("masquageTuto", 0);
        }
        GameStatic.accelerateurTemps = 1;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("masquageTuto")==1)
        {
            panelTuto1.SetActive(false);
        }
        else
        {
            panelTuto1.SetActive(true);
            GameStatic.accelerateurTemps = 0;
        }
    }
}
