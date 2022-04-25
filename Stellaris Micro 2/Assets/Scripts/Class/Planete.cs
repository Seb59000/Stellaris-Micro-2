using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using classDuJeu;

public class Planete
{
    public Coordonnees localisation { get; set; }
    public int emplacement0 { get; set; }
    public int emplacement1 { get; set; }
    public int emplacement2 { get; set; }
    public int emplacement3 { get; set; }
    public int emplacement4 { get; set; }
    public int emplacement5 { get; set; }
    public int emplacement6 { get; set; }
    public int emplacement7 { get; set; }
    public int emplacement8 { get; set; }

    public Planete(Coordonnees localisation)
    {
        this.localisation = localisation;
        this.emplacement0 = 0;
        this.emplacement1 = 0;
        this.emplacement2 = 0;
        this.emplacement3 = 0;
        this.emplacement4 = 0;
        this.emplacement5 = 0;
        this.emplacement6 = 0;
        this.emplacement7 = 0;
        this.emplacement8 = 0;
    }
}


