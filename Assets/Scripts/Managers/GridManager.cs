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
    [SerializeField] Vector3 gridStartPos;
    [SerializeField] Vector3 offsetPerGrid;

    private GridCtrl[] grids;
    
    



    [SerializeField]
    private int radius;

    private void Awake()
    {
        Instance = this;
        int gridCounts = radius * 2 + 7;
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






    public int playerPosRelay2Enemy = -8;
    
    public void MovePlayer(int dir)
    {
        playerPosRelay2Enemy += dir;
    }

    public void MoveEnemy(int dir)
    {
        playerPosRelay2Enemy -= dir;
    }




    public void SetupGrid()
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
        
    }
}
