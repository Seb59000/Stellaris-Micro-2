using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class Emplacement
{
    public int index { get; set; }
    public int codeBatiment { get; set; }
    public Emplacement(int index, int codeBatiment)
    {
        this.index = index;
        this.codeBatiment = codeBatiment;
    }
}
