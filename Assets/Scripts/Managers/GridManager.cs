using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Resources")]
    [SerializeField] string gridPrefabPath;


    [Header("Parameter")]
    [SerializeField] Vector2 gridStartPos;
    [SerializeField] Vector2 offsetPerGrid;
    public int radius = 8;
    public int dangerAreaSize = 5;

    private GridCtrl[] grids;

    [Header("Display")]
    public int playerAt = 0;
    public int enemyAt = 0;

    private void Awake()
    {
        Instance = this;
        int gridCounts = radius * 2 + dangerAreaSize * 2 + 1;
        grids = new GridCtrl[gridCounts];
    }



    GridCtrl InstantiateGrid()
    {
        GameObject prefab = Resources.Load<GameObject>(gridPrefabPath);
        GameObject gridins = Instantiate(prefab);
        return gridins.GetComponent<GridCtrl>();
    }



    public void InitGrids()
    {
        for(int i = 0; i < grids.Length; i++)
        {
            grids[i] = InstantiateGrid();
            grids[i].gameObject.transform.position = gridStartPos + offsetPerGrid * i;
        }


    }


    public int GetRealPos(int coord, int offset)
    {
        return (((coord + offset) % grids.Length) + grids.Length) % grids.Length;
    }

    public void MovePlayer(int dir)
    {
        
    }

    public void MoveEnemy(int dir)
    {
        
    }


    public void SetupGrid()
    {
        int idx = 0;
        int dir = 0;
        Vector2 enemyPos = grids[GetRealPos(enemyAt, 0)].gameObject.transform.position;

        for (int i = 0; i < radius; ++i)
        {
            dir = i + 1;
            idx = GetRealPos(enemyAt, dir);
            grids[idx].SetDanger(0);
            grids[idx].SetPosition(enemyPos + offsetPerGrid * dir);

            idx = GetRealPos(enemyAt, -dir);
            grids[idx].SetDanger(0);
            grids[idx].SetPosition(enemyPos - offsetPerGrid * dir);
        }

        for(int i = 0; i < dangerAreaSize; ++i)
        {
            dir = radius + i + 1;
            idx = GetRealPos(enemyAt, dir);
            grids[idx].SetDanger(10);
            grids[idx].SetPosition(enemyPos + offsetPerGrid * dir);

            idx = GetRealPos(enemyAt, -dir);
            grids[idx].SetDanger(10);
            grids[idx].SetPosition(enemyPos - offsetPerGrid * dir);
        }
    }

    public void ResetGrid()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        InitGrids();
    }

    // Update is called once per frame
    void Update()
    {
        SetupGrid();



    }
}
