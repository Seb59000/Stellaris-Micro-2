using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class CreationSSolPhysique : MonoBehaviour
{
    public GameObject ssolPrefab;
    public float limiteGauche = -13f;
    public float limiteHaut = -2.2f;
    public float espacementLargeur = 1.7f;
    public float espacementHauteur = 1.1f;
    public float limiteProfondeur = -0.5f;
    public float limitePlafond = 0.5f;
    public float aleaPosition = 0.4f;

    public void CreationSSolDebutPartie()
    {
        // on crée les Systsolaire Physiques dans le tableau
        CreationTableauSSolPhysiques();

        // on place les ssolaires alignés en lignes et colonnes régulières
        PlacementSSolOrtho();

        // on applique des aléas dans la repartition de chaque SSol
        AleasPlaceSSol();
    }

    private void CreationTableauSSolPhysiques()
    {
        if (GameStatic.tableauSystemeSolaire.Count>0)
        {
            RemiseAZerotabloSSol(GameStatic.tableauSystemeSolaire.Count);
        }
        //Je crée les lignes puis les colonnes dans le tableau des syst solaires physiques
        for (int i = 0; i < GalaxieRenderer.nbLignes; i++)
        {
            List<GameObject> ligneSSolPhy = new List<GameObject>();
            // pour chaque ligne je remplie les colonnes de prefabSSOl
            for (int j = 0; j < GalaxieRenderer.nbColonnes; j++)
            {
                GameObject ssol = Instantiate(ssolPrefab, transform);

                // au passage je rentre l'index de ligne et col sur le script qui controle le click
                ClickSSolManager[] composant = ssol.GetComponentsInChildren<ClickSSolManager>();
                composant[0].ligne = i;
                composant[0].colonne = j;

                ligneSSolPhy.Add(ssol);
            }
            GameStatic.tableauSystemeSolaire.Add(ligneSSolPhy);
        }
    }

    private static void RemiseAZerotabloSSol(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int count2 = GameStatic.tableauSystemeSolaire[i].Count;
            for (int j = 0; j < count2; j++)
            {
                Destroy(GameStatic.tableauSystemeSolaire[i][j].gameObject);
            }
        }
        GameStatic.tableauSystemeSolaire.Clear();
    }

    void PlacementSSolOrtho()
    {
        int indexLigneTraite = 0;
        foreach (List<GameObject> ligneSSol in GameStatic.tableauSystemeSolaire)
        {
            CreationLigneOrtho(indexLigneTraite, ligneSSol);
            indexLigneTraite++;
        }
    }

    void CreationLigneOrtho(int indexLigneTraite, List<GameObject> listSSol)
    {
        for (int i = 0; i < GalaxieRenderer.nbColonnes; i++)
        {
            //pour chaque ligne je déplace les ssol sur la gauche 
            //calcul de la position theorique a appliquer
            Vector3 vectorOrthoTemp = new Vector3((limiteHaut + (espacementHauteur * indexLigneTraite)), -1, (limiteGauche + (espacementLargeur * i)));

            //application du deplacement
            listSSol[i].transform.position = vectorOrthoTemp;
        }
    }

    void AleasPlaceSSol()
    {
        foreach (var ligneSSol in GameStatic.tableauSystemeSolaire)
        {
            traitementAleasLigne(ligneSSol);
        }
    }

    void traitementAleasLigne(List<GameObject> listSSol)
    {
        foreach (var ssol in listSSol)
        {
            //  calcul d'alea
            Vector3 vectorTemp = ssol.gameObject.transform.position;
            vectorTemp.x = vectorTemp.x + UnityEngine.Random.Range(-aleaPosition, aleaPosition);
            vectorTemp.y = vectorTemp.y + UnityEngine.Random.Range(limiteProfondeur, limitePlafond);
            vectorTemp.z = vectorTemp.z + UnityEngine.Random.Range(-aleaPosition, aleaPosition);
            // modif position
            ssol.gameObject.transform.position = vectorTemp;
        }
    }
}
