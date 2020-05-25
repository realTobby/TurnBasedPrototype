using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoardGenerator : MonoBehaviour
{
    public List<BaseTileModel> usedTiles = new List<BaseTileModel>();

    public GameObject basePrefab;

    // Start is called before the first frame update
    void Start()
    {
        CalcNoise();

        int middleX = 25 / 2;
        int middleZ = 25 / 2;



        // unhide first tiles ==> in the middle
        for (int z = middleZ - 1; z < middleZ+2; z++)
        {
            for (int x = middleX - 1; x < middleX+2; x++)
            {
                Vector3 pos = new Vector3(x, -0.2F, z);
                UnhideTileAt(pos);
            }
        }

        //Vector3 pos1 = new Vector3(-2, -1, 0);
        //UnhideTileAt(pos1);
        //Vector3 pos2 = new Vector3(0, -1, -2);
        //UnhideTileAt(pos2);
        //Vector3 pos3 = new Vector3(2, -1, 0);
        //UnhideTileAt(pos3);
        //Vector3 pos4 = new Vector3(0, -1, 2);
        //UnhideTileAt(pos4);

    }


    void CalcNoise()
    {
        System.Random rnd = new System.Random();

        var randomSeed = rnd.Next(0, 10000);

        float scale = 6F;

        int width = 25;
        int height = 25;

        float xOrg = 0F;
        float yOrg = 0F;

        float y = 0.0F;

        while (y < height)
        {
            float x = 0.0F;
            while (x < width)
            {
                float xCoord = (xOrg + x  / width)  * scale;
                float yCoord = (yOrg + y  / height)  * scale;
                float sample = Mathf.PerlinNoise(xCoord + randomSeed, yCoord + randomSeed);
                Vector3 pos = new Vector3(x, 0, y);
                if (sample <= 0.35)
                    CreateTileAt(pos, TileType.Water, false);
                else
                    CreateTileAt(pos, TileType.Grass, false);
                x++;
            }
            y++;
        }
    }


    public void GenerateNextTiles(Vector3 positionCenter)
    {
        // left
        Vector3 left = new Vector3(positionCenter.x-1,-0.2F, positionCenter.z);
        if (IsPositionAvailable(left))
            UnhideTileAt(left);
        // up
        Vector3 up = new Vector3(positionCenter.x, -0.2F, positionCenter.z+1);
        if (IsPositionAvailable(up))
            UnhideTileAt(up);
        // right
        Vector3 right = new Vector3(positionCenter.x+1, -0.2F, positionCenter.z);
        if (IsPositionAvailable(right))
            UnhideTileAt(right);
        // down
        Vector3 down = new Vector3(positionCenter.x, -0.2F, positionCenter.z-1);
        if (IsPositionAvailable(down))
            UnhideTileAt(down);
    }

    private void CreateTileAt(Vector3 pos, TileType tileType, bool isHidden)
    {
        BaseTileModel btm = new BaseTileModel(pos, tileType, isHidden);
        AddNewTile(btm);
        if (isHidden == false)
        {
            CreateTileAt(btm.tilePosition);
        }
    }

    private void CreateTileAt(Vector3 pos)
    {
        BaseTileModel tile = GetTileAt(pos);
        var newTile = Instantiate(basePrefab, pos, Quaternion.identity);
        newTile.GetComponent<BaseGroundBehaviour>().SendData(tile);
        newTile.transform.parent = this.gameObject.transform;
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void AddNewTile(BaseTileModel tile)
    {
        usedTiles.Add(tile);
    }

    public List<BaseTileModel> GetGameBoard()
    {
        return usedTiles;
    }

    public bool IsPositionAvailable(Vector3 posInQuestion)
    {
        return usedTiles.Exists(item => item.tilePosition.x == posInQuestion.x && item.tilePosition.z == posInQuestion.z && item.IsHidden == true);
    }

    public BaseTileModel GetTileAt(Vector3 pos)
    {
        return usedTiles.Where(item => item.tilePosition.x == pos.x && item.tilePosition.z == pos.z).FirstOrDefault();
    }

    public void UnhideTileAt(Vector3 pos)
    {
        usedTiles.Where(item => item.tilePosition.x == pos.x && item.tilePosition.z == pos.z).FirstOrDefault().IsHidden = false;
        CreateTileAt(pos);
    }

    public void UnselectTileAt(Vector3 pos)
    {
        // TODO: unselect tile the player already walked on!
    }
}
