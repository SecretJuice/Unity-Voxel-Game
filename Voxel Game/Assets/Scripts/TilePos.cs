using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePos
{
    int xPos, yPos;

    Vector2[] uvs;

    public TilePos(int xPos, int yPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        uvs = new Vector2[]
        {
            new Vector2(xPos/16f, yPos/16f),
            new Vector2(xPos/16f, (yPos+1)/16f),
            new Vector2((xPos+1)/16f, (yPos+1)/16f),
            new Vector2((xPos+1)/16f, yPos/16f),
        };
    }

    public Vector2[] GetUVs()
    {
        return uvs;
    }


    public static Dictionary<Tile, TilePos> tiles = new Dictionary<Tile, TilePos>()
    {
        
        {Tile.Stone, new TilePos(0,0)},
        {Tile.DarkStone, new TilePos(0, 1)},
        {Tile.Autunite, new TilePos(0, 2) },
        
    };
}

public enum Tile { Stone, DarkStone, Autunite}
