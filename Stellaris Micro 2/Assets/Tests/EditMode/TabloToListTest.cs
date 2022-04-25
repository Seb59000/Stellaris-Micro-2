using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using classDuJeu;

namespace Tests
{
    public class TestTabloToListIndex
    {
        [Test]
        public void Zero()
        {
            // compare
            int result = CreationGalaxieInfo.CoordTabloToListIndex(new Coordonnees(0, 0));
            Assert.AreEqual(1, result);
        }

        [Test]
        public void EndFirstLine()
        {
            // compare
            int result = CreationGalaxieInfo.CoordTabloToListIndex(new Coordonnees(0, GalaxieRenderer.nbColonnes - 1));
            Assert.AreEqual(GalaxieRenderer.nbColonnes, result);
        }

        [Test]
        public void BeginningSecondLine()
        {
            // compare
            int result = CreationGalaxieInfo.CoordTabloToListIndex(new Coordonnees(1, 0));
            Assert.AreEqual(GalaxieRenderer.nbColonnes + 1, result);
        }

        [Test]
        public void EndSecondLine()
        {
            // compare
            int result = CreationGalaxieInfo.CoordTabloToListIndex(new Coordonnees(1, GalaxieRenderer.nbColonnes - 1));
            Assert.AreEqual((GalaxieRenderer.nbColonnes * 2), result);
        }

        [Test]
        public void BeginningThirdLine()
        {
            // compare
            int result = CreationGalaxieInfo.CoordTabloToListIndex(new Coordonnees(2, 0));
            Assert.AreEqual((GalaxieRenderer.nbColonnes * 2) + 1, result);
        }

        [Test]
        public void Last()
        {
            // compare
            int result = CreationGalaxieInfo.CoordTabloToListIndex(new Coordonnees(GalaxieRenderer.nbLignes-1, GalaxieRenderer.nbColonnes-1));
            Assert.AreEqual(((GalaxieRenderer.nbColonnes) * GalaxieRenderer.nbLignes), result);
        }
    }
}
