using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int gridSizeX;
    [SerializeField] int gridSizeY;
    [SerializeField] GridUnit prefab;
    [SerializeField] Spawner spawnerPrefab;

    Spawner spawner;
    GridInfo gridInfo;
    GridUnit[,] grid;

    public GridInfo GridInfo { get { return gridInfo; } set { gridInfo = value; } }
    public Spawner Spawner { get { return spawner; } set { spawner = value; } }

    private void Awake()
    {
        Initialize();
        Application.targetFrameRate = 60;
    }

    void Initialize()
    {
        LoadGridInfo(Application.dataPath + "/GrindInfo.txt");
        grid = new GridUnit[GridInfo.sizeX, GridInfo.sizeY];
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

        CommonUtils.Initialize(grid, gridSizeX, gridSizeY);

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
        GridUnit gridUnit = CommonUtils.CircularCheck(midX, midY, 0);
        spawner.transform.position = new Vector3(gridUnit.XPosition, gridUnit.YPosition, -3);
        spawner.SetCurrentGridUnit(gridUnit);
    }

    void LoadGridInfo(string path)
    {
        if (!File.Exists(path))
        {
            gridInfo = new GridInfo(gridSizeX, gridSizeY);
            string jsonData = JsonUtility.ToJson(gridInfo);
            File.WriteAllText(path, jsonData);
        }
        else
        {
            string jsonData = File.ReadAllText(path);
            gridInfo = JsonUtility.FromJson<GridInfo>(jsonData);
        }

        if (gridInfo.sizeX != gridSizeX || gridInfo.sizeY != gridSizeY)
        {
            gridInfo = new GridInfo(gridSizeX, gridSizeY);
            string jsonData = JsonUtility.ToJson(gridInfo);
            File.WriteAllText(path, jsonData);
        }
    }
}

[System.Serializable]
public class GridInfo
{

    public GridInfo(int _sizeX, int _sizeY)
    {
        sizeX = _sizeX;
        sizeY = _sizeY;
    }
    public int sizeX;
    public int sizeY;
}
