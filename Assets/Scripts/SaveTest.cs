using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveTest : MonoBehaviour
{
    public SaveObject so;
    public GameObject Cell;
    public GameObject CellContainer;
    public Slider Height;
    public Slider Width;
    public Slider Speed;
    public Slider Boucle;
    public TextMeshProUGUI savename;
    public TMP_Dropdown dropdownFormat;
    private string format;
    SaveJson savejson = new SaveJson();

    public void SaveButton()
    {
        var so = CreateDataObject();
        savejson.Save(so, format);
        print("Save");
    }

    public void LoadButton()
    {
        so = savejson.Load(this, format);
        if (so != null)
        {
            LoadDataObject();
            print("Load");
        }
    }

    public void changeSaveName()
    {
        if (dropdownFormat.value == 0)
        {
            format = ".txt";
        }
        else
        {
            format = ".png";
        }
        savejson.saveName(savename.text, format) ;
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
        GridManager.instance.hauteur = so.Hauteur;
        GridManager.instance.largeur = so.Largeur;
        GridManager.instance.timer = so.Vitesse;
        GridManager.instance.rule = so.Bordure;

        Height.value = so.Hauteur;
        Width.value = so.Largeur;
        Speed.value = so.Vitesse;
        Boucle.value = so.Bordure;

        if (so.Bordure == 0) { 
            so = savejson.Load(this);
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
            so = savejson.Load(this);
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
        GridManager.instance.posCam(so.Largeur, so.Hauteur);
        return so;
    }
}
