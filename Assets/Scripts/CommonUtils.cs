using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonUtils
{
    static int gridSizeX;
    static int gridSizeY;
    static GridUnit[,] grid;

    public static void Initialize(GridUnit[,] _grid, int sizeX, int sizeY)
    {
        grid = _grid;
        gridSizeX = sizeX;
        gridSizeY = sizeY;
    }

    public static GridUnit CircularCheck(int x, int y, int level)
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
