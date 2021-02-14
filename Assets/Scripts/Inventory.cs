using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    static public Inventory instance = new Inventory();

    static public int huys = 0;

    private uint points = 0;

    private Inventory()
    {

    }

    public void AddPoints(uint numberOfPoints)
    {
        points += numberOfPoints;
    }
}
