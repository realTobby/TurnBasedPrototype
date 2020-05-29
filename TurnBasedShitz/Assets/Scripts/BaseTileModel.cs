using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Grass,
    Dirt,
    Stone,
    Water,
    Hill,
    Pit
}

public class BaseTileModel
{
    public ItemModel item { get; set; } = null;

    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost { get { return gCost + hCost; } }
    public BaseTileModel parentTile;

    public Vector3 tilePosition { get; set; } = new Vector3();
    public bool isWalkable { get; set; } = false;
    public TileType tileType { get; set; } = TileType.Grass;

    public bool IsHidden { get; set; }

    public BaseTileModel(Vector3 pos, TileType type, bool isHidden)
    {
        tileType = type;
        tilePosition = pos;
        IsHidden = isHidden;

        switch(tileType)
        {
            default:
                isWalkable = true;
                break;
            case TileType.Water:
                isWalkable = false;
                break;
            case TileType.Hill:
                isWalkable = false;
                break;
            case TileType.Pit:
                isWalkable = false;
                break;
        }

    }

    public override string ToString()
    {
        return "X: " + tilePosition.x + " Y: " + tilePosition.y + " Z: " + tilePosition.z;
    }

    public void SetItem(ItemModel item)
    {
        this.item = item;
    }

}
