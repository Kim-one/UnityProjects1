using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public GameObject HealthObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(HealthObject, new Vector3(2.35f,0f, -6.31f), Quaternion.identity);
        if(HealthObject != null) 
        { 
            Destroy(HealthObject);
        }
        
    }
}
