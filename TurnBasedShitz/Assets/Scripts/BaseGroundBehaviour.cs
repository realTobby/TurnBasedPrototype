using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGroundBehaviour : MonoBehaviour
{
    public BaseTileModel myData;

    public void SendData(BaseTileModel data)
    {
        myData = data;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().material = this.GetComponentInParent<GlobalVariableHolder>().GetMaterial("grassyMaterial");


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unselect()
    {
        this.GetComponent<MeshRenderer>().material = this.GetComponentInParent<GlobalVariableHolder>().GetMaterial("grassyMaterial");
    }

    public void Select()
    {
        this.GetComponent<MeshRenderer>().material = this.GetComponentInParent<GlobalVariableHolder>().GetMaterial("selectedMaterial");
    }
}
