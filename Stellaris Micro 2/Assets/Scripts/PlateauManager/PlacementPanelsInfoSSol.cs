using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class PlacementPanelsInfoSSol : MonoBehaviour
    {
        public List<Transform> targetPosition;

        public Transform panelInfoPrefab;

        public Canvas canvas;

        private int nbcolListSSol = 3;
        private int nbligneListSSol = 3;

        int tailleListe;

        void Start()
        {
            //Transform cloneTest = Instantiate<Transform>(panelInfoPrefab, transform.position, transform.rotation, canvas.transform);

            nbligneListSSol = GameStatic.tabloInfosSystemeSolaire.Count;
            GameStatic.panelTomove = new List<List<Transform>>();

            foreach (List<InfoSystemeSolaire> lignessol in GameStatic.tabloInfosSystemeSolaire)
            {
                foreach (InfoSystemeSolaire ssol in lignessol)
                {
                    Transform clone = Instantiate<Transform>(panelInfoPrefab, transform.position, transform.rotation, canvas.transform);
                    GameStatic.panelTomove[ssol.indexLigneListSS].Add(clone);
                }
            }
            Debug.Log("GameStatic.panelTomove.Count " + GameStatic.panelTomove.Count);

            //tailleListe = GameStatic.panelTomove.Count;
        }
    }
}

