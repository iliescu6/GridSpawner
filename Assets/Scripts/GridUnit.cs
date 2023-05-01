using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteColor;
    bool blocked;
    bool occupied;
    bool markedForClearing;
    int xIndex;
    int yIndex;
    float xPosition;
    float yPosition;
    SimpleObject simpleObject;

    public bool Blocked { get { return blocked; } set { blocked = value; } }
    public bool Occupied { get { return occupied; } set { occupied = value; } }
    public bool MarkedForClearing { get { return markedForClearing; } set { markedForClearing = value; } }
    public int XIndex { get { return xIndex; } set { xIndex = value; } }
    public int YIndex { get { return yIndex; } set { yIndex = value; } }
    public float XPosition { get { return xPosition; } set { xPosition = value; } }
    public float YPosition { get { return yPosition; } set { yPosition = value; } }
    public SimpleObject SimpleObject { get { return simpleObject; } set { simpleObject = value; } }

    public void Initialize(Color color, int _xIndex, int _yIndex)
    {
        int blockProbability = Random.Range(0, 4);

        //value needed to make it black
        if (blockProbability == 3)
        {
            spriteColor.color = Color.black;
            blocked = true;
        }
        else
        {
            spriteColor.color = color;
        }

        xPosition = transform.position.x;
        yPosition = transform.position.y;
        xIndex = _xIndex;
        yIndex = _yIndex;
    }

    public bool CheckSameNeighbourColor(GridUnit unit)
    {
        if (unit.simpleObject == null)
        {
            return false;
        }
        return unit.Occupied && unit.SimpleObject.GetColor() == this.simpleObject.GetColor();
    }

    public bool CheckNeighbours(GridUnit[,] grid)
    {
        if (simpleObject == null)
        {
            return false;
        }

        //check right
        if (xIndex + 1 < grid.GetLength(0) && CheckSameNeighbourColor(grid[xIndex + 1, yIndex]))
        {
            grid[xIndex + 1, yIndex].markedForClearing = true;
            markedForClearing = true;
        }

        //check above
        if (yIndex + 1 < grid.GetLength(1) && CheckSameNeighbourColor(grid[xIndex, yIndex + 1]))
        {
            grid[xIndex, yIndex + 1].markedForClearing = true;
            markedForClearing = true;
        }

        //check below
        if (yIndex - 1 > 0 && CheckSameNeighbourColor(grid[xIndex, yIndex - 1]))
        {
            grid[xIndex, yIndex - 1].markedForClearing = true;
            markedForClearing = true;
        }

        //check left
        if (xIndex - 1 > 0 && CheckSameNeighbourColor(grid[xIndex - 1, yIndex]))
        {
            grid[xIndex - 1, yIndex].markedForClearing = true;
            markedForClearing = true;
        }

        return markedForClearing;
    }
}
