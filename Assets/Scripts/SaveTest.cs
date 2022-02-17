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
    private string _format;
    SaveJson _savejson = new SaveJson();

    public void SaveButton()
    {
        var so = CreateDataObject();
        _savejson.Save(so, _format);
    }

    public void LoadButton()
    {
        so = _savejson.Load(this, _format);
        if (so != null)
        {
            LoadDataObject();
        }
    }

    public void ChangeSaveName()
    {
        if (dropdownFormat.value == 0)
        {
            _format = ".txt";
        }
        else
        {
            _format = ".png";
        }
        _savejson.SaveName(savename.text, _format) ;
    }

    private SaveObject CreateDataObject()
    {
        so = new SaveObject();
        so.m_hauteur = GridManager.instance.height;
        so.m_largeur = GridManager.instance.width;
        so.m_vitesse = GridManager.instance.timer;
        so.m_bordure = GridManager.instance.rule;

        so.m_grid = new List<int>();
        foreach (var item in GridManager.instance._grid)
        {
            if (item.GetComponent<CellAttributs>().alive)
            {
                so.m_grid.Add(1);
            }
            else
            {
                so.m_grid.Add(0);
            }
        }
        return so;
    }
    private void LoadDataObject()
    {
        GridManager.instance.height = so.m_hauteur;
        GridManager.instance.width = so.m_largeur;
        GridManager.instance.timer = so.m_vitesse;
        GridManager.instance.rule = so.m_bordure;

        Height.value = so.m_hauteur;
        Width.value = so.m_largeur;
        Speed.value = so.m_vitesse;
        Boucle.value = so.m_bordure;

        if (so.m_bordure == 0) { 
            if (so != null)
            {
                foreach (Transform Cell in CellContainer.transform)
                {
                    Destroy(Cell.gameObject);
                }
                GridManager.instance._grid = new GameObject[so.m_hauteur + 19, so.m_largeur + 19];
                int a = 0;
                for (int i = 0; i < so.m_hauteur + 19; i++)
                {
                    for (int o = 0; o < so.m_largeur + 19; o++)
                    {
                        GameObject clone = Instantiate(Cell, new Vector3(o, i, 0), Quaternion.identity);
                        clone.name = (i.ToString() + "," + o.ToString());
                        clone.transform.SetParent(CellContainer.transform);                        

                        if (so.m_grid[a] == 1)
                        {
                            clone.GetComponent<CellAttributs>().alive = true;
                        }
                        else
                        {
                            clone.GetComponent<CellAttributs>().alive = false;
                        }

                        if (o < 19 / 2 || i < 19 / 2 || o >= so.m_largeur + 19 / 2 || i >= so.m_hauteur + 19 / 2)
                            clone.SetActive(false);

                        GridManager.instance._grid[i, o] = clone;
                        a++;
                    }
                }
            }
        }
        else
        {
            if (so != null)
            {
                foreach (Transform Cell in CellContainer.transform)
                {
                    Destroy(Cell.gameObject);
                }
                GridManager.instance._grid = new GameObject[so.m_hauteur, so.m_largeur];
                for (int i = 0; i < so.m_hauteur; i++)
                {
                    for (int o = 0; o < so.m_largeur; o++)
                    {
                        int a = o + (so.m_largeur * i);

                        GameObject clone = Instantiate(Cell, new Vector3(o, i, 0), Quaternion.identity);
                        clone.name = (i.ToString() + "," + o.ToString());
                        clone.transform.SetParent(CellContainer.transform);

                        if (so.m_grid[a] == 1)
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
        }
    }
}
