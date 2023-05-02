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
    Vector3 mousePositionOffset;
    bool spawnObjects;

    public bool SpawnObjects { get { return spawnObjects; } set { spawnObjects = value; } }
    public void Initialize(GridUnit[,] _grid, GridUnit unit, int _gridX, int _gridY)
    {
        curretUnit = unit;
        grid = _grid;
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
        GridUnit temp = CommonUtils.CircularCheck(curretUnit.XIndex, curretUnit.YIndex, 0);

        if (temp != null)
        {
            curretUnit = temp;
            previousUnit.Occupied = false;
            previousUnit = null;
        }

        transform.position = new Vector3(curretUnit.XPosition, curretUnit.YPosition, -3);
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

        GridUnit temp = CommonUtils.CircularCheck(curretUnit.XIndex, curretUnit.YIndex, 1);
        if (temp == null)
        {
            Debug.Log("end");
        }
        else
        {
            temp.Occupied = true;
            SimpleObject newObject = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);
            temp.SimpleObject = newObject;
            StartCoroutine(newObject.MoveToTarget(new Vector3(temp.XPosition, temp.YPosition, 0)));
        }
    }

    public void Clear()
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
}
