using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseGroundBehaviour : MonoBehaviour
{
    private Material grassyMaterial;
    private Material selectedMaterial;
    private Material waterMaterial;

    public BaseTileModel myData;

    public bool isSelected = false;

    public void Start()
    {
        
    }

    public void CollectItem()
    {
        myData.item = null;
        // todo inventory stuff
        Destroy(this.gameObject.transform.GetChild(0).gameObject);
    }


    public void SendData(BaseTileModel data)
    {
        grassyMaterial = Resources.Load<Material>("Materials/grassyMaterial") as Material;
        selectedMaterial = Resources.Load<Material>("Materials/selectedMaterial") as Material;
        waterMaterial = Resources.Load<Material>("Materials/waterMaterial") as Material;

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
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 0.5f, this.gameObject.transform.position.z);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Update()
    {
        if(myData != null && isSelected == false)
            UpdateMaterial();
    }

    public void Unselect()
    {
        UpdateMaterial();
        isSelected = false;
    }

    public void Select()
    {
        isSelected = true;
        this.GetComponent<MeshRenderer>().material = selectedMaterial;
    }
}
