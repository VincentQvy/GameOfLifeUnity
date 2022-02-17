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
    public GameObject[,] _grid;
    private bool[,] toChange;
    public int height = 20;
    public int width = 20;
    public int rule = 0;
    private int _offsetCompensate = 0;
    public int offset = 0;
    public bool checkStart = false;
    public bool checkGen = false;
    public float timer = 1.0f;
    private float _timer;
    private bool playLoop = false;
    public static GridManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _timer = timer;
        BorderRule();
        GenMap();
    }
    public void PlayStart()
    {
        checkStart = !checkStart;
    }

    public void BorderRule()
    {
        rule = (int)GameObject.Find("GameType").GetComponent<Slider>().value;
        if (rule == 0)
        {
            offset = 20;
            _offsetCompensate = 1;
        }
        else
        {
            offset = 0;
            _offsetCompensate = 0;
        }
    }

    public void ChangeHeight()
    {
        height = (int)GameObject.Find("Slider Hauteur").GetComponent<Slider>().value;
    }
    public void ChangeWidth()
    {
        width = (int)GameObject.Find("Slider Largeur").GetComponent<Slider>().value;
    }
    public void ChangeSpeed()
    {
        timer = GameObject.Find("Slider Vitesse").GetComponent<Slider>().value;
    }

    public void PosCam(int largeur, int hauteur)
    {
        int cameraSize = 0;
        if (hauteur < largeur)
        {
            cameraSize = largeur + offset;
        }
        else
        {
            cameraSize = hauteur + offset;
        }
        Camera.main.orthographicSize = cameraSize / 2;
        Vector3 position = new Vector3((largeur + offset) / 3, (hauteur + offset) / 2 - 0.5f, -10);
        Camera.main.transform.position = position;
    }

    public void GenMap()
    {
        PosCam(width,height);
        checkGen = true;
        foreach (Transform Cell in CellContainer.transform)
        {
            Destroy(Cell.gameObject);
        }
        _grid = new GameObject[height + offset, width + offset];
        for (int y = 0; y < height + offset; y++)
        {
            for (int x = 0; x < width + offset; x++)
            {
                GameObject clone = Instantiate(Cell, new Vector3(x, y, 0), Quaternion.identity);
                clone.name = (y.ToString() + "," + x.ToString());
                clone.transform.SetParent(CellContainer.transform);
                if (x < offset / 2 || y < offset / 2 || x >= width + offset / 2 || y >= height + offset / 2)
                    clone.SetActive(false);
                _grid[y, x] = clone;
            }
        }
    }

    int CheckNeighbour(int x, int y)
    {
        int count = 0;
        int newX = 0;
        int newY = 0;
        if (rule == 1)
        {
            // bas droite
            if (x <= width - 1)
            {
                newX = x + 1;
                newY = y - 1;
                if (y == 0)
                {
                    newY = height - 1;
                }
                if (x == width - 1)
                {
                    newX = 0;
                }
                count += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }

            // droite
            if (x < width - 1)
                count += _grid[y, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
                count += _grid[y, width - x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            //haut droite
            if (x <= width - 1)
            {
                newX = x + 1;
                newY = y + 1;
                if (y == height - 1)
                {
                    newY = 0;
                }
                if (x == width - 1)
                {
                    newX = 0;
                }
                count += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }
            
            //bas
            if (y > 0)
                count += _grid[y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
            {
                count += _grid[height - y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;
            }

            //haut
            if (y < height - 1)
                count += _grid[y + 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
                count += _grid[height - y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;

            //bas gauche
            if (x >= 0)
            {
                newX = x - 1;
                newY = y - 1;
                if (y == 0)
                {
                    newY = height - 1;
                }
                if (x == 0)
                {
                    newX = width - 1;
                }
                count += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }
                           

            //gauche
            if (x > 0)
                count += _grid[y, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;
            else
                count += _grid[y, width - x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            //haut gauche
            if (x >= 0)
            {
                newX = x - 1;
                newY = y + 1;
                if (y == height - 1)
                {
                    newY = 0;
                }
                if (x == 0)
                {
                    newX = width - 1;
                }
                count += _grid[newY, newX].GetComponent<CellAttributs>().alive ? 1 : 0;
            }
        }
        else
        {
            if (y == height + offset || x == width + offset || x == 0 || y == 0)
            {
                _grid[y , x ].GetComponent<CellAttributs>().alive = false;
            }
            if (y - 1 >= 0 && x + 1 < width + offset - _offsetCompensate)
                count += _grid[y - 1, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (x + 1 < width + offset - _offsetCompensate)
                count += _grid[y, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (x + 1 < width + offset - _offsetCompensate && y + 1 < height + offset - _offsetCompensate)
                count += _grid[y + 1, x + 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (y - 1 >= 0)
                count += _grid[y - 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (y + 1 < height + offset - _offsetCompensate)
                count += _grid[y + 1, x].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (x - 1 >= 0 && y - 1 >= 0)
                count += _grid[y - 1, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;
            
            if (x - 1 >= 0)
                count += _grid[y, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

            if (y + 1 < height + offset - _offsetCompensate && x - 1 >= 0)
                count += _grid[y + 1, x - 1].GetComponent<CellAttributs>().alive ? 1 : 0;

        }
        return count;
    }
    void PlayLoop()
    {
        BorderRule();
        toChange = new bool[height + offset, width + offset];
        for (int y = 0; y < height + offset - _offsetCompensate; y++)
        {
            for (int x = 0; x < width + offset - _offsetCompensate; x++)
            {
                toChange[y, x] = _grid[y, x].GetComponent<CellAttributs>().alive;
                switch (CheckNeighbour(x, y))
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
        for (int y = 0; y < height + offset - _offsetCompensate; y++)
        {
            for (int x = 0; x < width + offset - _offsetCompensate; x++)
            {
                _grid[y, x].GetComponent<CellAttributs>().alive = toChange[y, x];
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
                if (x >= 0 && x < width + offset && y >= 0 && y < height + offset)
                {
                    _grid[y, x].GetComponent<CellAttributs>().alive = !_grid[y, x].GetComponent<CellAttributs>().alive;
                }
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
            playLoop = false;
        }
    }
}


