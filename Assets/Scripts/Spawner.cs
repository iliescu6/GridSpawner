using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour
{
    [SerializeField] SimpleObject prefab;
    GridUnit curretUnit;
    GridUnit previousUnit;
    GridUnit[,] grid;
    List<GridUnit> markedForClearing = new List<GridUnit>();
    int gridSizeX;
    int gridSizeY;
    Vector3 mousePositionOffset;

    public void Initialize(GridUnit[,] _grid, GridUnit unit, int _gridX, int _gridY)
    {
        curretUnit = unit;
        grid = _grid;
        gridSizeY = _gridY;
        gridSizeX = _gridX;
    }

    private void Update()
    {
        if (Input.GetKey("g"))
        {
            Spawn();
        }

        if (Input.GetKeyDown("h"))
        {
            Clear();
        }

    }

    Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        GridUnit temp = CircularCheck(curretUnit.XIndex, curretUnit.YIndex, 0);
        curretUnit = temp;
        previousUnit.Occupied = false;
        previousUnit = null;
        transform.position = new Vector3(curretUnit.XPosition, curretUnit.YPosition, 0);
        curretUnit.Occupied = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (previousUnit == null)
        {
            previousUnit = curretUnit;
        }
        if (collision.gameObject.tag == "GridUnit")
        {
            curretUnit = collision.GetComponent<GridUnit>();
        }
    }

    public void Spawn()
    {
        GridUnit temp = CircularCheck(curretUnit.XIndex, curretUnit.YIndex, 1);
        if (temp == null)
        {
            Debug.Log("end");
        }
        else
        {
            temp.Occupied = true;
            SimpleObject newObject = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            temp.SimpleObject = newObject;
            StartCoroutine(newObject.MoveToTarget(new Vector3(temp.XPosition, temp.YPosition, 0)));
        }
    }

    void Clear()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j].CheckNeighbours(grid))
                {
                    markedForClearing.Add(grid[i, j]);
                }
            }
        }
        for (int i = 0; i < markedForClearing.Count; i++)
        {
            Destroy(markedForClearing[i].SimpleObject.gameObject);
            markedForClearing[i].Occupied = false;
            markedForClearing[i].MarkedForClearing = false;
        }
        markedForClearing = new List<GridUnit>();
    }

    public void SetCurrentGridUnit(GridUnit newUnit)
    {
        curretUnit = newUnit;
        curretUnit.Occupied = true;
    }

    public GridUnit CircularCheck(int x, int y, int level)
    {
        int xLimit = x + level;
        int yLimit = y + level;
        int xMin = x - level;
        int yMin = y - level;
        int currentX;
        int currentY;

        if (xLimit >= gridSizeX && yLimit >= gridSizeY && xMin < 0 && yMin < 0)
        {
            return null;
        }

        while (yLimit > gridSizeY - 1)
        {
            yLimit--;
        }

        while (xLimit > gridSizeX - 1)
        {
            xLimit--;
        }
        currentY = yLimit;
        currentX = x;

        if (!grid[x, yLimit].Occupied && !grid[x, yLimit].Blocked)
        {
            return grid[x, yLimit];
        }
        else
        {
            //mid top to right
            for (int i = currentX; i <= xLimit; i++)
            {
                if (i > gridSizeX - 1)
                {
                    xLimit = currentX = i;
                    break;
                }
                if (!grid[i, yLimit].Occupied && !grid[i, yLimit].Blocked)
                {
                    return grid[i, yLimit];
                }
                currentX = i;
            }

            //top righ to bottom right
            for (int i = currentY; i >= yMin; i--)
            {
                if (i < 0)
                {
                    yMin = 0;
                    break;
                }
                if (!grid[xLimit, i].Occupied && !grid[xLimit, i].Blocked)
                {
                    return grid[xLimit, i];
                }
                currentY = i;
            }

            //bottom right to bottom left
            for (int i = currentX; i >= xMin; i--)
            {
                if (i < 0)
                {
                    xMin = 0;
                    break;
                }
                if (!grid[i, yMin].Occupied && !grid[i, yMin].Blocked)
                {
                    return grid[i, yMin];
                }
                currentX = i;
            }

            //bottom left to top left
            for (int i = currentY; i <= yLimit; i++)
            {
                if (i > gridSizeY)
                {
                    currentY = gridSizeY - 1;
                    break;
                }
                if (!grid[xMin, i].Occupied && !grid[xMin, i].Blocked)
                {
                    return grid[xMin, i];
                }
                currentY = i;
            }

            //top left to top mid
            for (int i = currentX; i < x; i++)
            {
                if (!grid[i, yLimit].Occupied && !grid[i, yLimit].Blocked)
                {
                    return grid[i, yLimit];
                }
            }
            return CircularCheck(x, y, level + 1);
        }
    }
}
