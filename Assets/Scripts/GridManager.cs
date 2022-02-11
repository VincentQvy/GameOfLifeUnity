using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public InputManager inputManager;
    public GameObject Cell;
    public Material Alive;
    public Material Dead;
    private GameObject[,] _grid; 
    public int hauteur = 5;
    public int largeur = 5;
    void Start()
    {
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
    }
}
