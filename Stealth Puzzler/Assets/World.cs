using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A singleton to hold an array of all the hiding spots in the game environment
public sealed class World
{
    private static readonly World instance = new World();
    private static GameObject[] hidingSpots;

    //construct the singleton
    static World()
    {
        //populate hiding spot array with objects in the environment
        //that match the tag
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    private World() { }

    public static World Instance
    {
        get { return instance; }
    }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}
