using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Tile top, side, bottom;

    public TilePos topPos, sidePos, bottomPos;

    public Block(Tile tile)
    {
        top = side = bottom = tile;
        GetPositions();
    }

    public Block(Tile top, Tile side, Tile bottom)
    {
        this.top = top;
        this.side = side;
        this.bottom = bottom;
        GetPositions();
    }

    void GetPositions()
    {
        topPos = TilePos.tiles[top];
        sidePos = TilePos.tiles[side];
        bottomPos = TilePos.tiles[bottom];
    }


    public static Dictionary<BlockType, Block> blocks = new Dictionary<BlockType, Block>(){
        {BlockType.Stone, new Block(Tile.Stone)},
        {BlockType.DarkStone, new Block(Tile.DarkStone)},
        {BlockType.Autunite, new Block(Tile.Autunite) },
        {BlockType.ShipCore, new Block(Tile.ShipCore) },
    };
}

public enum BlockType { Air, Stone, DarkStone, Autunite, ShipCore}
