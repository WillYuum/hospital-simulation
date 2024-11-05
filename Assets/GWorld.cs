using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new();
    private static WorldStates world;

    static GWorld()
    {
        world = new WorldStates();
    }

    private GWorld()
    {
    }

    public static GWorld Instance
    {
        get
        {
            return instance;
        }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}