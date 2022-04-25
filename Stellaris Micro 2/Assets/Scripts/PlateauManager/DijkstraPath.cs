using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace classDuJeu
{
    public class DijkstraPath : MonoBehaviour
    {
        // partie path finding
        int cptr = 0;
        Itineraire itineraire;
        List<Coordonnees> ssolAnalysed;
        const int NBTOURS = 8;
        //!!! ↓il doit etre supp↑ a celui ci
        const int CODERETOURPOSITIF = 50;
        public Material transparentMaterial;
        public Material arrowMaterial;

        public void CalculItineraireTroupe(Coordonnees sSolOrigine, Coordonnees sSolArrivee)
        {
            // je reinit l'itineraire avec la coordonnée de départ comme 1er item
            itineraire = new Itineraire();
            itineraire.listeCoordonnees.Add(sSolOrigine);

            // j ajoute la coord du ssol de depart a ssolAnalysed pour la reinit de scan sur ssol atteign
            ssolAnalysed = new List<Coordonnees>();
            ssolAnalysed.Add(sSolOrigine);

            // je met le ssol de depart comme rejeté pour eviter les retours en arriere
            GameStatic.tabloInfosNavigation[sSolOrigine.indexLigneListSS][sSolOrigine.indexColonneListSS].rejete = true;

            // je reinit le cptr de tours
            cptr = 0;
            TrouverMeilleurDirectionEtAvancer(sSolOrigine, sSolArrivee);

            if (cptr >= CODERETOURPOSITIF)
            {
                MettreDirectionsSsolDansBonSens();
            }

            // je remet les ssol par lesquels je suis passé comme non rejeté pour future recherche
            RemiseZeroScanSSolAtteignable();
        }

        private void MettreDirectionsSsolDansBonSens()
        {
            // pour chaque coord de l'itineraire
            int count = itineraire.listeCoordonnees.Count;
            for (int i = 0; i < count - 1; i++)
            {
                // on affiche la fleche
                AffichageFleche(itineraire.listeCoordonnees[i], itineraire.listeCoordonnees[i + 1]);

                // on change la direction sur le systeme origine du tableau de sauvegardeStatic
                GameStatic.tabloInfosSystemeSolaire[itineraire.listeCoordonnees[i].indexLigneListSS][itineraire.listeCoordonnees[i].indexColonneListSS]
                    .direction = itineraire.listeCoordonnees[i + 1];
            }

            // pour le dernier ssol de l'itineraire
            // on verifie que le systeme d'arrivé ne renvoie pas vers le ssol d'origine
            Coordonnees directionDernier = GameStatic.tabloInfosSystemeSolaire[itineraire.listeCoordonnees[count - 1].indexLigneListSS][itineraire.listeCoordonnees[count - 1].indexColonneListSS].direction;

            // si c'est le cas (direction du dernier = avant dernier)
            if (directionDernier.indexLigneListSS == itineraire.listeCoordonnees[count - 2].indexLigneListSS && directionDernier.indexColonneListSS == itineraire.listeCoordonnees[count - 2].indexColonneListSS)
            {
                // on cache la fleche
                CacherFleche(itineraire.listeCoordonnees[count - 1]);

                // on change la direction sur le systeme dans le tableau sauvegardeStatic (on le renvoie vers lui meme)
                GameStatic.tabloInfosSystemeSolaire[itineraire.listeCoordonnees[count - 1].indexLigneListSS][itineraire.listeCoordonnees[count - 1].indexColonneListSS]
                    .direction = itineraire.listeCoordonnees[count - 1];
            }
        }

        private void RemiseZeroScanSSolAtteignable()
        {
            foreach (Coordonnees ssolAna in ssolAnalysed)
            {
                GameStatic.tabloInfosNavigation[ssolAna.indexLigneListSS][ssolAna.indexColonneListSS].rejete = false;
            }
        }

        private void TrouverMeilleurDirectionEtAvancer(Coordonnees sSolOrigine, Coordonnees sSolArrivee)
        {
            // jusqu'a ce que on arrive au ssolArrivee ou que cptr éloigné de + de 5 
            while ((cptr < NBTOURS))
            {
                cptr++;
                if (itineraire.listeCoordonnees.Count > 0)
                {
                    // je recupere la derniere coordonnée de l'itineraire
                    Coordonnees lastSsol = itineraire.listeCoordonnees[itineraire.listeCoordonnees.Count - 1];

                    // avec je trouve le meilleur candidat
                    Coordonnees bestCandidat = TrouverMeilleurCandidat(lastSsol, sSolArrivee);
                    // si il n'y en a pas
                    if (bestCandidat.indexLigneListSS == -1)
                    {
                        // le dernier on le marque comme rejeté (dans listSSOLAtteignable du ssol d'avant) comme ca on ira plus dans cette direction là
                        // pour ça on recupere les coord du ssol d'avant
                        Coordonnees ssolAvant = itineraire.listeCoordonnees[itineraire.listeCoordonnees.Count - 1];
                        GameStatic.tabloInfosNavigation[ssolAvant.indexLigneListSS][ssolAvant.indexColonneListSS].rejete = true;

                        // je retourne en arriere en le supprimant de l'itineraire
                        itineraire.listeCoordonnees.RemoveAt(itineraire.listeCoordonnees.Count - 1);

                        //et je rappel cette methode et continue d'avancer
                        TrouverMeilleurDirectionEtAvancer(sSolOrigine, sSolArrivee);
                    }
                    else
                    {// si j'en trouve un 
                     // je l'ajoute a l'itineraire 
                        itineraire.listeCoordonnees.Add(bestCandidat);
                        // je note sa coord pour la reinit de scan sur ssol atteign
                        ssolAnalysed.Add(bestCandidat);

                        // si le ssol d'arrivée est trouvé 
                        if (bestCandidat.indexLigneListSS == sSolArrivee.indexLigneListSS && bestCandidat.indexColonneListSS == sSolArrivee.indexColonneListSS)
                        {
                            // Debug.Log("ssol trouvé!!!");
                            // on arrete la boucle
                            cptr = CODERETOURPOSITIF;
                        }
                        else
                        {
                            // sinon 
                            // je rappel cette methode
                            TrouverMeilleurDirectionEtAvancer(sSolOrigine, sSolArrivee);
                        }
                    }
                    // s'il n'y a pas de systeme avant on termine la recherche
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ssolOrigine"></param>
        /// <returns> retourne new Coordonnees(-1,-1) si pas de ssol trouvé</returns>
        private Coordonnees TrouverMeilleurCandidat(Coordonnees ssolOrigine, Coordonnees sSolArrivee)
        {
            Coordonnees meilleurCandidat = new Coordonnees(-1, -1);
            // je recupere la liste des ssol atteignables depuis sSolOrigine
            List<Coordonnees> ssolsAtteignables = GameStatic.tabloInfosNavigation[ssolOrigine.indexLigneListSS][ssolOrigine.indexColonneListSS].SSAtteignables;

            // on attribue une note a chaque ssol en fonction de son eloignement de l'arrivée
            int noteMin = 150;
            int indexNoteMin = -1;
            int count = ssolsAtteignables.Count;
            for (int i = 0; i < count; i++)
            {
                // si non rejeté (voir↓)plus bas
                if (GameStatic.tabloInfosNavigation[ssolsAtteignables[i].indexLigneListSS][ssolsAtteignables[i].indexColonneListSS].rejete == false)
                {
                    // calcul de la note
                    // si il va trop en haut a gauche ou a droite par rapport a la cible la note augmente
                    // on calcul son ecart par rap au ssol d'arrivée sur la dimension ligne
                    int note1 = ssolsAtteignables[i].indexLigneListSS - sSolArrivee.indexLigneListSS;

                    // on calcul son ecart par rap au ssol d'arrivée sur la dimension col
                    int note2 = ssolsAtteignables[i].indexColonneListSS - sSolArrivee.indexColonneListSS;
                    int note = Math.Abs(note1) + Math.Abs(note2);
                    //    Debug.Log("note " + note);
                    //  si la note est inf a noteMin d'avant
                    if (note < noteMin)
                    {
                        // on sauvegarde la note
                        noteMin = note;

                        // on sauvegarde la localisation
                        meilleurCandidat = ssolsAtteignables[i];

                        // on sauvegarde son index dans la list SSolAtteignable pour un eventuel retour en arriere
                        indexNoteMin = i;
                    }
                }
            }
            // s il existe
            if (indexNoteMin > -1)
            {
                // le meilleur on le marque comme rejeté comme ca on peut pas faire de retour en arriere
                // et si on fait un retour en arriere plus tard on ira plus dans cette direction là
                GameStatic.tabloInfosNavigation[ssolOrigine.indexLigneListSS][ssolOrigine.indexColonneListSS].rejete = true;
            }
            return meilleurCandidat;
        }

        private void AffichageFleche(Coordonnees SSolOrigine, Coordonnees SSolArrivee)
        {
            // on affiche la fleche
            MeshRenderer[] allRendererChildren = GameStatic.tableauSystemeSolaire[SSolOrigine.indexLigneListSS][SSolOrigine.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>();
            allRendererChildren[4].material = arrowMaterial;

            // on l'oriente vers le ssol visé
            allRendererChildren[4].transform.LookAt(GameStatic.tableauSystemeSolaire[SSolArrivee.indexLigneListSS][SSolArrivee.indexColonneListSS].gameObject.transform.position);
        }

        private void CacherFleche(Coordonnees SSol)
        {
            // on cache la fleche 
            MeshRenderer allRendererChildren = GameStatic.tableauSystemeSolaire[SSol.indexLigneListSS][SSol.indexColonneListSS].gameObject.transform.GetComponentsInChildren<MeshRenderer>()[4];
            allRendererChildren.material = transparentMaterial;
        }
    }
}

