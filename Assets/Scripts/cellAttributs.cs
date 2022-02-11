using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cellAttributs : MonoBehaviour
{
    public bool alive = false;
    public Material materialAlive;
    public Material materialDead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            gameObject.GetComponent<MeshRenderer>().material = materialAlive;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = materialDead;
        }
    }
}
