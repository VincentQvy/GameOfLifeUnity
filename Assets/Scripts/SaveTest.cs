using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveObject so;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var so = CreateDataObject();
            SaveJson.Save(so);
            print("Save");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            so = SaveJson.Load(this);
            print("Load");
        }
    }

    private SaveObject CreateDataObject()
    {
        so = new SaveObject();
        so.Hauteur = GridManager.instance.hauteur;
        so.Largeur = GridManager.instance.largeur;
        so.Vitesse = GridManager.instance.timer;
        so.Bordure = GridManager.instance.rule;

        so.Grid = new List<int>();
        foreach (var item in GridManager.instance._grid)
        {
            if (item.GetComponent<CellAttributs>().alive)
            {
                so.Grid.Add(1);
            }
            else
            {
                so.Grid.Add(0);
            }
        }
        return so;
    }
}
