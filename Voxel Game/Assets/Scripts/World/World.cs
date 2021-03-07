using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{

    public static readonly World instance = new World();

    private World() { }

    public static World Instance
    {
        get
        {
            return instance;
        }
    }
}
