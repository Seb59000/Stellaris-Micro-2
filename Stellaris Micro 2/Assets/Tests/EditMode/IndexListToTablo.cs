using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using classDuJeu;

namespace Tests
{
    public class IndexListToTablo
    {
        [Test]
        public void IndexLigne()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(1);
            Coordonnees resultExpected = new Coordonnees(0, 0);
            Debug.Log("index ligne" +result.indexLigneListSS);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }

        [Test]
        public void IndexColonne()
        {
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(1);
            Coordonnees resultExpected = new Coordonnees(0, 0);
            Debug.Log("index col" + result.indexColonneListSS);

            Assert.AreEqual(resultExpected.indexColonneListSS, result.indexColonneListSS);
        }

        [Test]
        public void DeuxiemeLigne()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(10);
            Coordonnees resultExpected = new Coordonnees(1, 0);
            Debug.Log("index ligne" + result.indexLigneListSS);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }

        [Test]
        public void FinDeuxiemeLigne()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(15);
            Coordonnees resultExpected = new Coordonnees(1, 0);
            Debug.Log("index ligne" + result.indexLigneListSS);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }

        [Test]
        public void TroisiemeLigne()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(17);
            Coordonnees resultExpected = new Coordonnees(2, 0);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }

        [Test]
        public void FinTroisiemeLigne()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(24);
            Coordonnees resultExpected = new Coordonnees(2, 0);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }

        [Test]
        public void DeuxiemeCol()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(10);
            Coordonnees resultExpected = new Coordonnees(1, 1);

            Assert.AreEqual(resultExpected.indexColonneListSS, result.indexColonneListSS);
        }

        [Test]
        public void FinDeuxiemeCol()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(50);
            Coordonnees resultExpected = new Coordonnees(6, 1);

            Assert.AreEqual(resultExpected.indexColonneListSS, result.indexColonneListSS);
        }

        [Test]
        public void TroisiemeCol()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(11);
            Coordonnees resultExpected = new Coordonnees(1, 2);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }

        [Test]
        public void FinTroisiemeCol()
        {
            // compare
            Coordonnees result = CreationGalaxieInfo.IndexListToCoordTablo(51);
            Coordonnees resultExpected = new Coordonnees(6, 2);
            Debug.Log("index ligne" + result.indexLigneListSS);

            Assert.AreEqual(resultExpected.indexLigneListSS, result.indexLigneListSS);
        }
    }
}
