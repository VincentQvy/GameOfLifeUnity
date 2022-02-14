using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject Cell;
    public Material alive;
    public Material Dead;
    public GameObject CellContainer;
    private GameObject[,] _grid;
    private bool[,] toChange;
    public int hauteur = 5;
    public int largeur = 5;
    public bool checkStart = false;
    public bool checkGen = false;
    public float timer = 1.0f;
    private float _timer;
    private bool playLoop = false;
    void Start()
    {
        _timer = timer;
    }
    public void PlayStart()
    {
        checkStart = !checkStart;
    }

    public void ChangeHeight()
    {
        hauteur = (int)GameObject.Find("Slider Hauteur").GetComponent<Slider>().value;
    }
    public void ChangeWidth()
    {
        largeur = (int)GameObject.Find("Slider Largeur").GetComponent<Slider>().value;
    }
    public void ChangeSpeed()
    {
        timer = GameObject.Find("Slider Vitesse").GetComponent<Slider>().value;
    }

    void posCam()
    {
        int cameraSize = 0;
        if (hauteur < largeur)
        {
            cameraSize = largeur;
        }
        else
        {
            cameraSize = hauteur;
        }
        Camera.main.orthographicSize = cameraSize / 2;
        Vector3 position = new Vector3(0, hauteur / 2 - 0.5f, -10);
        Camera.main.transform.position = position;
    }

    public void GenMap()
    {
        posCam();
        checkGen = true;
        foreach (Transform Cell in CellContainer.transform)
        {
            Destroy(Cell.gameObject);
        }
        _grid = new GameObject[hauteur, largeur];
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                GameObject clone = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity);
                clone.name = (x.ToString() + "," + y.ToString());
                clone.transform.SetParent(CellContainer.transform);
                _grid[y, x] = clone;
            }
        }
    }

    int checkNeighbour(int x, int y)
    {
        int comptage = 0;

        if (x - 1 >= 0 && y + 1 < hauteur)
            comptage += _grid[x - 1, y + 1].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (y + 1 < hauteur)
            comptage += _grid[x, y + 1].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (x + 1 < largeur && y + 1 < hauteur)
            comptage += _grid[x + 1, y + 1].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (x - 1 >= 0)
            comptage += _grid[x - 1, y].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (x + 1 < largeur)
            comptage += _grid[x + 1, y].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (x - 1 >= 0 && y - 1 >= 0)
            comptage += _grid[x - 1, y - 1].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (y - 1 >= 0)
            comptage += _grid[x, y - 1].GetComponent<cellAttributs>().alive ? 1 : 0;

        if (x + 1 < largeur && y - 1 >= 0)
            comptage += _grid[x + 1, y - 1].GetComponent<cellAttributs>().alive ? 1 : 0;

        return comptage;
    }
    void PlayLoop()
    {
        toChange = new bool[largeur, hauteur];
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                toChange[y, x] = _grid[y,x].GetComponent<cellAttributs>().alive;
                switch (checkNeighbour(x, y))
                {
                    case 0:
                        toChange[y, x] = false;
                        break;
                    case 1:
                        toChange[y, x] = false;
                        break;
                    case 3:
                        toChange[y, x] = true;
                        break;
                    case 4:
                        toChange[y, x] = false;
                        break;
                    case 5:
                        toChange[y, x] = false;
                        break;
                    case 6:
                        toChange[y, x] = false;
                        break;
                    case 7:
                        toChange[y, x] = false;
                        break;
                    case 8:
                        toChange[y, x] = false;
                        break;
                }
            }
        }
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                _grid[y,x].GetComponent<cellAttributs>().alive = toChange[y, x];
            }
        }
    }

    void Update()
    {
        Vector3 worldPosition;
        if (checkGen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)Mathf.Floor(worldPosition.x + 0.5f);
                int y = (int)Mathf.Floor(worldPosition.y + 0.5f);
                _grid[y, x].GetComponent<cellAttributs>().alive = !_grid[y, x].GetComponent<cellAttributs>().alive;
            }
            if (checkStart)
            {
                playLoop = !playLoop;
            }
            if (playLoop && _timer <= 0)
            {
                PlayLoop();
                _timer = timer;
            }
            _timer -= Time.deltaTime;
        }
    }
}


