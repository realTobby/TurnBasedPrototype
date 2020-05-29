using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameBoardGenerator : MonoBehaviour
{
    public List<BaseTileModel> usedTiles = new List<BaseTileModel>();

    private GameObject basePrefab;
    private GameObject itemPrefab;

    private int worldLength = 30;


    // Start is called before the first frame update
    void Start()
    {
        basePrefab = Resources.Load<GameObject>("Prefabs/BaseGround") as GameObject;
        itemPrefab = Resources.Load<GameObject>("Prefabs/BaseItem") as GameObject;

        CalcNoise();

        int middleX = worldLength / 2;
        int middleZ = worldLength / 2;

        this.GetComponent<GameBoardInteraction>().SendPlayerToPos(new Vector3(middleX, 0, middleZ));

        // unhide first tiles ==> in the middle
        for (int z = middleZ - 1; z < middleZ+2; z++)
        {
            for (int x = middleX - 1; x < middleX+2; x++)
            {
                Vector3 pos = new Vector3(x, -0.2F, z);
                UnhideTileAt(pos);
            }
        }

    }


    void CalcNoise()
    {
        System.Random rnd = new System.Random();

        var randomSeed = rnd.Next(0, 10000);

        float scale = 6F;

        int width = worldLength;
        int height = worldLength;

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
                    CreateTileAt(pos, TileType.Water, true);
                else
                    CreateTileAt(pos, TileType.Grass, true);
                x++;
            }
            y++;
        }
    }


    public void GenerateNextTiles(Vector3 positionCenter)
    {
        // Tile Positions
        Vector3 tile1 = new Vector3(positionCenter.x - 1, -0.2F, positionCenter.z - 1);
        Vector3 tile2 = new Vector3(positionCenter.x, -0.2F, positionCenter.z - 1);
        Vector3 tile3 = new Vector3(positionCenter.x + 1, -0.2F, positionCenter.z - 1);
        Vector3 tile4 = new Vector3(positionCenter.x - 1, -0.2F, positionCenter.z);
        Vector3 tile5 = new Vector3(positionCenter.x + 1, -0.2F, positionCenter.z);
        Vector3 tile6 = new Vector3(positionCenter.x - 1, -0.2F, positionCenter.z + 1);
        Vector3 tile7 = new Vector3(positionCenter.x, -0.2F, positionCenter.z + 1);
        Vector3 tile8 = new Vector3(positionCenter.x + 1, -0.2F, positionCenter.z + 1);
        Vector3 tile9 = new Vector3(positionCenter.x, -0.2F, positionCenter.z - 2);
        Vector3 tile10 = new Vector3(positionCenter.x - 2, -0.2F, positionCenter.z);
        Vector3 tile11 = new Vector3(positionCenter.x + 2, -0.2F, positionCenter.z);
        Vector3 tile12 = new Vector3(positionCenter.x, -0.2F, positionCenter.z + 2);
        UnhideTileAt(tile1);
        UnhideTileAt(tile2);
        UnhideTileAt(tile3);
        UnhideTileAt(tile4);
        UnhideTileAt(tile5);
        UnhideTileAt(tile6);
        UnhideTileAt(tile7);
        UnhideTileAt(tile8);
        UnhideTileAt(tile9);
        UnhideTileAt(tile10);
        UnhideTileAt(tile11);
        UnhideTileAt(tile12);
    }

    private void CreateTileAt(Vector3 pos, TileType tileType, bool isHidden)
    {
        BaseTileModel btm = new BaseTileModel(pos, tileType, isHidden);

        if (btm.tileType != TileType.Water)
        {
            var magicNumber = UnityEngine.Random.Range(0, 100);
            if (magicNumber >= 98)
            {
                btm.item = CreateItem();
            }
        }
        

        AddNewTile(btm);
        if (isHidden == false)
        {
            CreateWorldTile(btm.tilePosition);
        }
    }

    private ItemModel CreateItem()
    {
        ItemModel newItem = new ItemModel("DEBUG", "This is a test item :)", new Dictionary<string, Stat>());
        return newItem;
    }


    private void CreateWorldTile(Vector3 pos)
    {
        #region Create World Tile
        BaseTileModel tile = GetTileAt(pos);
        var newTile = Instantiate(basePrefab, pos, Quaternion.identity);
        newTile.GetComponent<BaseGroundBehaviour>().SendData(tile);
        newTile.transform.parent = this.gameObject.transform;
        #endregion
        #region Create Item on Tile

        if(tile.item != null)
        {
            // do stuff
            Vector3 itemPos = new Vector3(pos.x, 1f, pos.z);
            Instantiate(itemPrefab, itemPos, Quaternion.identity, newTile.transform);
        }

        #endregion

        if (this.GetComponent<GameBoardInteraction>().currentPath.Exists(item => item == tile))
        {
            SelectPathAt(pos);
        }

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

    public BaseTileModel IsPositionOccupied(Vector3 posInQuestion)
    {
        return usedTiles.Where(item => item.tilePosition.x == posInQuestion.x && item.tilePosition.z == posInQuestion.z && item.IsHidden == true).FirstOrDefault();
    }

    public BaseTileModel GetTileAt(Vector3 pos)
    {
        return usedTiles.Where(item => item.tilePosition.x == pos.x && item.tilePosition.z == pos.z).FirstOrDefault();
    }

    public void UnhideTileAt(Vector3 pos)
    {
        BaseTileModel matchedTile = IsPositionOccupied(pos);

        if(matchedTile != null)
        {
            usedTiles.Where(item => item.tilePosition.x == pos.x && item.tilePosition.z == pos.z).FirstOrDefault().IsHidden = false;
            CreateWorldTile(pos);
        }
    }

    public void SelectPath()
    {
        int childCount = 0;
        childCount = this.gameObject.transform.childCount;
        foreach (BaseTileModel segment in this.GetComponent<GameBoardInteraction>().currentPath)
        {
            for (int i = 0; i < childCount; i++)
            {
                BaseTileModel segmentTile = this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().myData;
                if (segmentTile.tilePosition == segment.tilePosition)
                {
                    this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Select();
                }
            }
        }
    }

    public void SelectPathAt(Vector3 pos)
    {
        int childCount = 0;
        childCount = this.gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            BaseTileModel segmentTile = this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().myData;
            if (segmentTile.tilePosition.x == pos.x && segmentTile.tilePosition.z == pos.z)
            {
                this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Select();
            }
        }
    }

    public void UnselectTileAt(Vector3 pos)
    {
        GameObject ob = GetBaseTileAt(pos);
        if(ob != null)
        {
            ob.GetComponent<BaseGroundBehaviour>().Unselect();
        }
    }

    public void UnselectAllTiles()
    {
        if (this.GetComponent<GameBoardInteraction>().currentPath != null)
        {
            this.GetComponent<GameBoardInteraction>().currentPath.Clear();
            this.GetComponent<GameBoardInteraction>().currentPath = new List<BaseTileModel>();
        }

        int childCount = 0;
        childCount = this.gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.gameObject.transform.GetChild(i).GetComponent<BaseGroundBehaviour>().Unselect();
        }
    }

    public GameObject GetBaseTileAt(Vector3 pos)
    {
        int childCount = 0;
        childCount = this.gameObject.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject tileInQuestion = this.gameObject.transform.GetChild(i).gameObject;

            if(tileInQuestion.transform.position.x == pos.x && tileInQuestion.transform.position.z == pos.z)
            {
                return tileInQuestion;
            }
        }
        return null;
    }

}
