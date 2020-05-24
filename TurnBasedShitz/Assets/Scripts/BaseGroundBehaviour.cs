using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseGroundBehaviour : MonoBehaviour
{
    public Material grassyMaterial;
    public Material selectedMaterial;

    public BaseTileModel myData;

    public void SendData(BaseTileModel data)
    {
        myData = data;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().material = grassyMaterial;
    }

    public void Unselect()
    {
        this.GetComponent<MeshRenderer>().material = grassyMaterial;
    }

    public void Select()
    {
        this.GetComponent<MeshRenderer>().material = selectedMaterial;
    }
}
