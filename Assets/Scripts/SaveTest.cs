using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveObject so;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SaveJson.Save(so);
            print("Save");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            so = SaveJson.Load(this);
            print("Load");
        }
    }
}
