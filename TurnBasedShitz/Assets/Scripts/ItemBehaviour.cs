using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    private float speed = 150f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        this.transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.Self);


    }
}
