using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseGroundBehaviour : MonoBehaviour
{
    public Material grassyMaterial;
    public Material selectedMaterial;
    public Material waterMaterial;

    public BaseTileModel myData;

    public void SendData(BaseTileModel data)
    {
        myData = data;
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if(myData != null)
        {
            switch(myData.tileType)
            {
                case TileType.Grass:
                    this.GetComponent<MeshRenderer>().material = grassyMaterial;
                    break;
                case TileType.Water:
                    this.GetComponent<MeshRenderer>().material = waterMaterial;
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -0.4f, this.gameObject.transform.position.z);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Unselect()
    {
        UpdateMaterial();
    }

    public void Select()
    {
        this.GetComponent<MeshRenderer>().material = selectedMaterial;
    }
}
