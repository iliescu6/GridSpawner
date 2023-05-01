using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int gridSizeX;
    [SerializeField] int gridSizeY;
    [SerializeField] GridUnit prefab;
    [SerializeField] Spawner spawnerPrefab;

    Spawner spawner;
    GridUnit[,] grid;

    private void Awake()
    {
        Initialize();
        Application.targetFrameRate = 60;
    }

    void Initialize()
    {
        grid = new GridUnit[gridSizeX, gridSizeY];

        int colorIndex = 0;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                GridUnit temp = Instantiate(prefab, new Vector3(1 * i, 1 * j, 0), Quaternion.identity, transform);
                if (colorIndex % 2 == 0)
                {
                    temp.Initialize(Color.red, i, j);
                }
                else
                {
                    temp.Initialize(Color.white, i, j);
                }
                colorIndex++;
                grid[i, j] = temp;
            }
        }

        int midX;
        int midY;

        if (gridSizeX > 2)
        {
            midX = gridSizeX % 2 == 0 ? gridSizeX / 2 + 1 : gridSizeX / 2;
        }
        else
        {
            midX = 0;
        }

        if (gridSizeY > 2)
        {
            midY = gridSizeY % 2 == 0 ? gridSizeY / 2 + 1 : gridSizeY / 2;
        }
        else
        {
            midY = 0;
        }

        Spawner newSpawner = Instantiate(spawnerPrefab, new Vector3(grid[midX, midY].XPosition, grid[midX, midY].XPosition, 0), Quaternion.identity, transform);
        spawner = newSpawner;
       

        spawner.Initialize(grid, grid[midX, midY], gridSizeX, gridSizeY);
        GridUnit gridUnit = spawner.CircularCheck(midX, midY, 0);
        spawner.transform.position = new Vector3(gridUnit.XPosition, gridUnit.YPosition, 0);
        spawner.SetCurrentGridUnit(gridUnit);
    }
}
