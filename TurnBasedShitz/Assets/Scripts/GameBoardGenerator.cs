﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardGenerator : MonoBehaviour
{
    public GameObject basePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // width
        // length
        int width = 25;
        int height = 25;

        for(int y = -(height/2); y < height; y++)
        {
            for (int x = -(width / 2); x < width; x++)
            {
                Vector3 pos = new Vector3(x, -1, y);
                CreateTileAt(pos, TileType.Grass, true);
                
            }
        }

        // unhide first tiles
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 pos = new Vector3(x, -1, y);
                UnhideTileAt(pos);
            }
        }

        Vector3 pos1 = new Vector3(-2, -1, 0);
        UnhideTileAt(pos1);
        Vector3 pos2 = new Vector3(0, -1, -2);
        UnhideTileAt(pos2);
        Vector3 pos3 = new Vector3(2, -1, 0);
        UnhideTileAt(pos3);
        Vector3 pos4 = new Vector3(0, -1, 2);
        UnhideTileAt(pos4);

    }

    public void GenerateNextTiles(Vector3 positionCenter)
    {
        // left
        Vector3 left = new Vector3(positionCenter.x-1,positionCenter.y, positionCenter.z);
        if (this.GetComponent<GlobalVariableHolder>().IsPositionAvailable(left))
            UnhideTileAt(left);
        // up
        Vector3 up = new Vector3(positionCenter.x, positionCenter.y, positionCenter.z+1);
        if (this.GetComponent<GlobalVariableHolder>().IsPositionAvailable(up))
            UnhideTileAt(up);
        // right
        Vector3 right = new Vector3(positionCenter.x+1, positionCenter.y, positionCenter.z);
        if (this.GetComponent<GlobalVariableHolder>().IsPositionAvailable(right))
            UnhideTileAt(right);
        // down
        Vector3 down = new Vector3(positionCenter.x, positionCenter.y, positionCenter.z-1);
        if (this.GetComponent<GlobalVariableHolder>().IsPositionAvailable(down))
            UnhideTileAt(down);
    }

    private void CreateTileAt(Vector3 pos, TileType tileType, bool isHidden)
    {
        BaseTileModel btm = new BaseTileModel(pos, tileType, isHidden);
        this.GetComponent<GlobalVariableHolder>().AddNewTile(btm);
        if (isHidden == false)
        {
            CreateTileAt(btm.tilePosition);
        }
    }

    private void CreateTileAt(Vector3 pos)
    {
        BaseTileModel tile = this.GetComponent<GlobalVariableHolder>().GetTileAt(pos);
        var newTile = Instantiate(basePrefab, pos, Quaternion.identity);
        newTile.GetComponent<BaseGroundBehaviour>().SendData(tile);
        newTile.transform.parent = this.gameObject.transform;
    }


    // Update is called once per frame
    void Update()
    {
    }

    private void UnhideTileAt(Vector3 pos)
    {
        this.GetComponent<GlobalVariableHolder>().UnhideTileAt(pos);
        CreateTileAt(pos);

    }
}
