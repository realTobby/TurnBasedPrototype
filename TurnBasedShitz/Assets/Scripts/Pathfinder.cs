using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinder
{
    private List<BaseTileModel> currentGameBoard = new List<BaseTileModel>();

    private const int MOVE_STRAIGHT_COST = 10;

    private List<BaseTileModel> openList = new List<BaseTileModel>();
    private List<BaseTileModel> closedList = new List<BaseTileModel>();


    public List<BaseTileModel> FindPath(List<BaseTileModel> gameBoard, Vector3 startPos, Vector3 endPos)
    {
        currentGameBoard = gameBoard;

        BaseTileModel startNode = GetTileAt(startPos);
        BaseTileModel endNode = GetTileAt(endPos);
        openList.Add(startNode);

        for(int i = 0; i < currentGameBoard.Count; i++)
        {
            BaseTileModel tile = currentGameBoard[i];
            tile.gCost = int.MaxValue;
            tile.parentTile = null;
        }

        if (startNode != null)
        {
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);

            while (openList.Count > 0)
            {
                BaseTileModel currentNode = GetLowestFCostTile(openList);

                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (BaseTileModel neighbourNode in GetNeighbours(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;

                    int tGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tGCost < neighbourNode.gCost)
                    {
                        neighbourNode.parentTile = currentNode;
                        neighbourNode.gCost = tGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }
        }
        return null;
    }

    private BaseTileModel GetTileAt(Vector3 pos)
    {
        return currentGameBoard.Where(tile => tile.tilePosition.x == pos.x && tile.tilePosition.z == pos.z && tile.isWalkable == true).FirstOrDefault();
    }

    private List<BaseTileModel> GetNeighbours(BaseTileModel node)
    {
        List<BaseTileModel> neighbours = new List<BaseTileModel>();

        Vector3 leftPos = new Vector3(node.tilePosition.x - 1, node.tilePosition.y, node.tilePosition.z);
        BaseTileModel leftTile = GetTileAt(leftPos);
        if(leftTile != null)
        {
            if (leftTile.isWalkable == true)
                neighbours.Add(leftTile);
        }
        
        Vector3 rightPos = new Vector3(node.tilePosition.x + 1, node.tilePosition.y, node.tilePosition.z);
        BaseTileModel rightTile = GetTileAt(rightPos);
        if(rightTile != null)
        {
            if ( rightTile.isWalkable == true)
                neighbours.Add(rightTile);
        }
        
        Vector3 upPos = new Vector3(node.tilePosition.x, node.tilePosition.y, node.tilePosition.z + 1);
        BaseTileModel upTile = GetTileAt(upPos);
        if(upTile != null)
        {
            if (upTile.isWalkable == true)
                neighbours.Add(upTile);
        }
        

        Vector3 downPos = new Vector3(node.tilePosition.x, node.tilePosition.y, node.tilePosition.z - 1);
        BaseTileModel downTile = GetTileAt(downPos);
        if(downTile != null)
        {
            if (downTile.isWalkable == true)
                neighbours.Add(downTile);
        }
        

        return neighbours;
    }


    private List<BaseTileModel> CalculatePath(BaseTileModel endNode)
    {
        List<BaseTileModel> path = new List<BaseTileModel>();
        path.Add(endNode);
        BaseTileModel currentNode = endNode;
        while(currentNode.parentTile != null)
        {
            path.Add(currentNode.parentTile);
            currentNode = currentNode.parentTile;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(BaseTileModel a, BaseTileModel b)
    {
        int xDistance = Mathf.Abs(Convert.ToInt32(a.tilePosition.x) - Convert.ToInt32(b.tilePosition.x));
        int zDistance = Mathf.Abs(Convert.ToInt32(a.tilePosition.z) - Convert.ToInt32(b.tilePosition.z));
        int remaining = Mathf.Abs(xDistance - zDistance);
        return Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private BaseTileModel GetLowestFCostTile(List<BaseTileModel> currentTiles)
    {
        var lowest = currentTiles
        .OrderBy(item => item.fCost)
        .FirstOrDefault();

        return lowest;
    }

}
