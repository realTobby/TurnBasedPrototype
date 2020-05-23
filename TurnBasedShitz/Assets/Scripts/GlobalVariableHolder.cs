using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalVariableHolder : MonoBehaviour
{
    [SerializeField]
    private List<Material> materialList = new List<Material>();
    public Vector3 PLAYER_POS = new Vector3(0, 0.34f, 0);
    public GameObject PlayerReference;
    public List<BaseTileModel> usedTiles = new List<BaseTileModel>();
    public float playerSpeed = 2f;
    public Material GetMaterial(string inputName)
    {
        return materialList.Where(x => x.name == inputName).FirstOrDefault();
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
        // return false
        // when tile at position isHidden == false

        return usedTiles.Exists(x => x.tilePosition == posInQuestion && x.IsHidden == true);
    }

    public BaseTileModel GetTileAt(Vector3 pos)
    {
        return usedTiles.Where(item => item.tilePosition.x == pos.x && item.tilePosition.z == pos.z && item.isWalkable == true).FirstOrDefault();
    }

    public void UnhideTileAt(Vector3 pos)
    {

        usedTiles.Where(x => x.tilePosition == pos).FirstOrDefault().IsHidden = false;

    }

}
