using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveObject so;
    public GameObject Cell;
    public GameObject CellContainer;

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
            LoadDataObject();
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
    private SaveObject LoadDataObject()
    {
        
        if(so.Bordure == 0) { 
            so = SaveJson.Load(this);
            foreach (Transform Cell in CellContainer.transform)
            {
                Destroy(Cell.gameObject);

            }
            GridManager.instance._grid = new GameObject[so.Hauteur+20, so.Largeur + 20];
            for (int i=0; i<so.Hauteur + 20; i++)
            {
                for (int o = 0; o < so.Largeur + 20; o++)
                {
                    int a = o + ((so.Largeur+20) * i);
                
                    GameObject clone = Instantiate(Cell, new Vector3(o, i, 0), Quaternion.identity);
                    clone.name = (i.ToString() + "," + o.ToString());
                    clone.transform.SetParent(CellContainer.transform);

                    if (so.Grid[a] == 1)
                    {
                        clone.GetComponent<CellAttributs>().alive = true;
                    }
                    else
                    {
                        clone.GetComponent<CellAttributs>().alive = false;
                    }
                    if (o < 10 || i < 10 || o >= so.Largeur + 10 || i >= so.Hauteur + 10)
                        clone.SetActive(false);
                    GridManager.instance._grid[i, o] = clone;
                }

            }
        }
        else
        {
            so = SaveJson.Load(this);
            foreach (Transform Cell in CellContainer.transform)
            {
                Destroy(Cell.gameObject);
            }
            GridManager.instance._grid = new GameObject[so.Hauteur , so.Largeur ];
            for (int i = 0; i < so.Hauteur; i++)
            {
                for (int o = 0; o < so.Largeur; o++)
                {
                    int a = o + (so.Largeur * i);

                    GameObject clone = Instantiate(Cell, new Vector3(o, i, 0), Quaternion.identity);
                    clone.name = (i.ToString() + "," + o.ToString());
                    clone.transform.SetParent(CellContainer.transform);

                    if (so.Grid[a] == 1)
                    {
                        clone.GetComponent<CellAttributs>().alive = true;
                    }
                    else
                    {
                        clone.GetComponent<CellAttributs>().alive = false;
                    }
                    GridManager.instance._grid[i, o] = clone;
                }

            }
        }
        return so;
    }
}
