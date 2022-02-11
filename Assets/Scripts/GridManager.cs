using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Cell;
    public Material alive;
    public Material Dead;
    private GameObject[,] _grid;
    private bool[,] toChange;
    public int hauteur = 5;
    public int largeur = 5;
    private int ajout = 0;
    public float timer = 1.0f;
    private float _timer;
    private bool playLoop = false;
    void Start()
    {
        _timer = timer;
        _grid = new GameObject[hauteur, largeur];
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                GameObject clone = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity);
                clone.name = (x.ToString() + "," + y.ToString());
                _grid[x, y] = clone;
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
                toChange[x, y] = _grid[x, y].GetComponent<cellAttributs>().alive;
                switch (checkNeighbour(x, y))
                {
                    case 0:
                        toChange[x, y] = false;
                        break;
                    case 1:
                        toChange[x, y] = false;
                        break;
                    case 3:
                        toChange[x, y] = true;
                        break;
                    case 4:
                        toChange[x, y] = false;
                        break;
                    case 5:
                        toChange[x, y] = false;
                        break;
                    case 6:
                        toChange[x, y] = false;
                        break;
                    case 7:
                        toChange[x, y] = false;
                        break;
                    case 8:
                        toChange[x, y] = false;
                        break;
                }
            }
        }
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                _grid[x, y].GetComponent<cellAttributs>().alive = toChange[x, y];
            }
        }
    }

    void Update()
    {
        Vector3 worldPosition;
        if (Input.GetMouseButtonDown(0))
        {
            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)Mathf.Floor(worldPosition.x + 0.5f);
            int y = (int)Mathf.Floor(worldPosition.y + 0.5f);
            _grid[x, y].GetComponent<cellAttributs>().alive = !_grid[x, y].GetComponent<cellAttributs>().alive;
        }
        if (Input.GetKeyDown(KeyCode.K)) 
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
