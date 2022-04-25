using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class DijkstraPathEnnemi : MonoBehaviour
{
    // partie path finding
    static int cptr = 0;
    public static Itineraire itineraire;
    static List<Coordonnees> ssolAnalysed;
    const int NBTOURS = 10;
    const int CODERETOURPOSITIF = 50;

    public static bool CalculItineraireTroupeEnnemi(Coordonnees sSolOrigine, Coordonnees sSolArrivee, int indexEnnemi)
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
        TrouverMeilleurDirectionEtAvancer(sSolOrigine, sSolArrivee, indexEnnemi);

        if (itineraire.listeCoordonnees.Count > 0)
        {
            // on supprime le premier ssol de l'itineraire
            itineraire.listeCoordonnees.RemoveAt(0);
        }

        // je remet les ssol par lesquels je suis passé comme non rejeté pour future recherche
        RemiseZeroScanSSolAtteignable();

        if (itineraire.listeCoordonnees.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void RemiseZeroScanSSolAtteignable()
    {
        foreach (Coordonnees ssolAna in ssolAnalysed)
        {
            GameStatic.tabloInfosNavigation[ssolAna.indexLigneListSS][ssolAna.indexColonneListSS].rejete = false;
        }
    }

    private static void TrouverMeilleurDirectionEtAvancer(Coordonnees sSolOrigine, Coordonnees sSolArrivee, int indexEnnemi)
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
                Coordonnees bestCandidat = TrouverMeilleurCandidat(lastSsol, sSolArrivee, indexEnnemi);
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
                    TrouverMeilleurDirectionEtAvancer(sSolOrigine, sSolArrivee, indexEnnemi);
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
                        // on arrete la boucle
                        cptr = CODERETOURPOSITIF;
                    }
                    else
                    {
                        // sinon 
                        // je rappel cette methode
                        TrouverMeilleurDirectionEtAvancer(sSolOrigine, sSolArrivee, indexEnnemi);
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
    private static Coordonnees TrouverMeilleurCandidat(Coordonnees ssolOrigine, Coordonnees sSolArrivee, int indexEnnemi)
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
                int note1 = ssolsAtteignables[i].indexLigneListSS - sSolArrivee.indexLigneListSS;

                // on calcul son ecart par rap au ssol d'arrivée sur la dimension col
                int note2 = ssolsAtteignables[i].indexColonneListSS - sSolArrivee.indexColonneListSS;
                int note = Math.Abs(note1) + Math.Abs(note2);
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
                // }
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
}
