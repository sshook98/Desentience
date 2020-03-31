using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public int seed;
    public int width = 20; // x dim
    public int length = 20; // z dim
    public int gridSize = 5;
    public int minGapBetweenHallways = 3;
    public int maxGapBetweenHallways = 10;
    [Range(0.0f, 1.0f)]
    public float hallwaySkipChance;
    [Range(0.0f, 1.0f)]
    public float roomFillInChance;
    public int minHallwaySkip;
    public int maxHallwaySkip;
    private int hallwaySkip;
    public bool ensureFullConnectivity = true;
    public bool removeDeadEnds = true;

    private int[,] floor;
    private int iteration = 1;

    #region Prefabs
    public GameObject squarePrefab;
    public float squareSpawnHeight = 0;
    public GameObject wallPrefab;
    public float wallSpawnHeight = 0;
    #endregion

    #region Parent Gameobjects
    private GameObject floorParent;
    private GameObject squareParent;
    private GameObject wallParent;

    #endregion

    #region Keys
    private int emptyKey = 0;
    private int squareKey = 1;
    private int wallKey = 2;
    #endregion

    void Start()
    {
        floorParent = new GameObject();
        squareParent = new GameObject();
        wallParent = new GameObject();
        squareParent.name = "Squares";
        wallParent.name = "Walls";
        squareParent.transform.parent = floorParent.transform;
        wallParent.transform.parent = floorParent.transform;

        hallwaySkip = minHallwaySkip;

        GenerateFloor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateFloor();
        } else if (Input.GetKeyDown(KeyCode.Return))
        {
#if UNITY_EDITOR
            string localPath = "Assets/" + floorParent.name + ".prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(floorParent, localPath, InteractionMode.UserAction);
#endif
        }
    }

    private void GenerateFloor()
    {
        CleanFloor();

        CreateHallways();

        CreateWalls();

        FillRooms();

        if (ensureFullConnectivity)
        {
            EnsureConnectivity();
        }
        if (removeDeadEnds)
        {
            RemoveDeadEnds();
        }

        InterpretFloor();
        iteration++;
    }

    private void CleanFloor()
    {
        floor = new int[width, length];

        GameObject[] obs = GameObject.FindGameObjectsWithTag("Square");
        for (int i = 0; i < obs.Length; i++)
        {
            Destroy(obs[i]);
        }

        obs = GameObject.FindGameObjectsWithTag("Wall");
        for (int i = 0; i < obs.Length; i++)
        {
            Destroy(obs[i]);
        }
        floorParent.name = "Floor" + seed + "_" + iteration;
    }

    private void CreateHallways()
    {
        int curZ = Random.Range(minGapBetweenHallways, maxGapBetweenHallways);
        while (curZ < width - minGapBetweenHallways)
        {
            int startingX = 1;
            int hallwayLength = length - 1;
            for (int i = 0; i < hallwayLength; i++)
            {
                floor[startingX + i, curZ] = squareKey;
                if (i % hallwaySkip == 0 && Random.Range(0, 1f) < hallwaySkipChance)
                {
                    i += hallwaySkip;
                    hallwaySkip = Random.Range(minHallwaySkip, maxHallwaySkip);
                }
                
            }
            curZ += Random.Range(minGapBetweenHallways, maxGapBetweenHallways);
        }

        int curX = Random.Range(minGapBetweenHallways, maxGapBetweenHallways);
        while (curX < length - minGapBetweenHallways)
        {
            int startingZ = 1;
            int hallwayLength = width - 1;
            for (int i = 0; i < hallwayLength; i++)
            {
                floor[curX, startingZ + i] = squareKey;
                if (i % hallwaySkip == 0 && Random.Range(0, 1.0f) < hallwaySkipChance)
                {
                    i += hallwaySkip;
                    hallwaySkip = Random.Range(minHallwaySkip, maxHallwaySkip);
                }
            }
            curX += Random.Range(minGapBetweenHallways, maxGapBetweenHallways);
        }
    }

    private void CreateWalls()
    {
        int[,] floorCopy = new int[width, length];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == length - 1)
                {
                    floorCopy[i, j] = wallKey;
                } else if (floor[i, j] == squareKey)
                {
                    floorCopy[i, j] = squareKey;
                } else
                {
                    var neighbors = GetNeighbors(new Vector2Int(i, j));
                    foreach (Vector2Int pos in neighbors)
                    {
                        if (floor[pos.x, pos.y] == squareKey)
                        {
                            floorCopy[i, j] = wallKey;
                            break;
                        }
                    }

                }
            }
        }
        floor = floorCopy;

        // fill in spots that have 3 or more wall neighbors as walls too
        // check each neighbor if 
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                CheckFillIn(new Vector2Int(i, j), wallKey);
            }
        }
    }

    private void CheckFillIn(Vector2Int pos, int fillKey)
    {
        int x = pos.x;
        int z = pos.y;

        int key = floor[x, z];
        if (key == emptyKey || key == squareKey)
        {
            List<Vector2Int> adj = GetAdjacent(pos);
            int count = 0;
            for (int i = 0; i < adj.Count; i++)
            {
                Vector2Int p = adj[i];
                if (floor[p.x, p.y] == fillKey)
                {
                    count++;
                }
            }

            if (count >= 3)
            {
                floor[x, z] = wallKey;
                for (int i = 0; i < adj.Count; i++)
                {
                    CheckFillIn(adj[i], fillKey);
                }
            }
        }        
    }

    private void FillRooms()
    {
        int numRooms = 1000;
        int numDoors;
        int minNumDoors = 2;
        Vector2Int cur;
        Vector2Int pos;
        List<Vector2Int> fringe;
        List<Vector2Int> doorOptions;

        for (int i = 0; i < numRooms; i++)
        {
            pos = new Vector2Int(Random.Range(1, width), Random.Range(1, length));            
            if (floor[pos.x, pos.y] == emptyKey)
            {
                fringe = new List<Vector2Int>();
                doorOptions = new List<Vector2Int>();
                fringe.Add(pos);

                while (fringe.Count > 0)
                {
                    cur = fringe[0];
                    fringe.RemoveAt(0);

                    if (floor[cur.x, cur.y] == emptyKey)
                    {
                        floor[cur.x, cur.y] = squareKey;
                        fringe.AddRange(GetAdjacent(cur));
                    } else if (floor[cur.x, cur.y] == wallKey)
                    {
                        if (cur.x != 0 && cur.x != width - 1 && cur.y != 0 && cur.y != length - 1)
                        {
                            doorOptions.Add(cur);
                        }
                    }
                }

                numDoors = Random.Range(minNumDoors, doorOptions.Count / 20 + minNumDoors);
                for (int k = 0; k < numDoors && doorOptions.Count > 0; k++)
                {
                    Vector2Int door = doorOptions[Random.Range(0, doorOptions.Count)];
                    floor[door.x, door.y] = squareKey;
                }
            }
        }

        pos = new Vector2Int(Random.Range(1, width), Random.Range(1, length));
        CheckFillIn(pos, squareKey);
    }

    private void EnsureConnectivity()
    {
        List<Vector2Int> allspots = new List<Vector2Int>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                allspots.Add(new Vector2Int(i, j));
            }
        }

        Vector2Int startingPoint = new Vector2Int(1, 1);
        bool foundStartingPoint = false;
        while (!foundStartingPoint)
        {
            int x = Random.Range(1, width-1);
            int z = Random.Range(1, length-1);
            startingPoint = new Vector2Int(x, z);

            foundStartingPoint = floor[x, z] == squareKey;
        }

        List<Vector2Int> connected = new List<Vector2Int>();
        List<Vector2Int> fringe = new List<Vector2Int>();
        Vector2Int cur;
        fringe.Add(startingPoint);

        while (fringe.Count > 0)
        {
            cur = fringe[0];
            fringe.RemoveAt(0);

            if (floor[cur.x, cur.y] == squareKey && !connected.Contains(cur))
            {
                connected.Add(cur);
                List<Vector2Int> neighbors = GetAdjacent(cur);
                foreach (Vector2Int pos in neighbors)
                {
                    if (!fringe.Contains(pos) && !connected.Contains(pos))
                    {
                        fringe.Add(pos);
                    }
                }
            }
        }

        int filledCount = 0;
        for (int i = 0; i < allspots.Count; i++)
        {
            cur = allspots[i];
            if (!connected.Contains(cur))
            {
                filledCount++;
                floor[cur.x, cur.y] = wallKey;
            }
        }

        Debug.Log("Filled to ensure connectivity: " + filledCount);


    }

    private void RemoveDeadEnds()
    {
        Vector2Int cur;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                cur = new Vector2Int(i, j);
                if (floor[i, j] == squareKey)
                {
                    CheckFillIn(cur, wallKey);
                }
            }
        }
    }
    

    private void InterpretFloor()
    {
        GameObject go;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (floor[i, j] == squareKey)
                {
                    go = Instantiate(squarePrefab);
                    go.transform.position = new Vector3(i, 0, j) * gridSize + new Vector3(0, squareSpawnHeight, 0);
                    go.transform.parent = squareParent.transform;
                } else if (floor[i, j] == wallKey)
                {
                    go = Instantiate(wallPrefab);
                    go.transform.position = new Vector3(i, 0, j) * gridSize + new Vector3(0, wallSpawnHeight, 0);
                    go.transform.parent = wallParent.transform;
                }
            }
        }
    }

    private List<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        int x;
        int z;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i != 0 || j != 0)
                {
                    x = pos.x + i;
                    z = pos.y + j;

                    if (x >= 0 && x < width && z >= 0 && z < length)
                    {
                        neighbors.Add(new Vector2Int(x, z));
                    }
                }
            }
        }

        return neighbors;
    }

    private List<Vector2Int> GetAdjacent(Vector2Int pos)
    {
        List<Vector2Int> adjacent = new List<Vector2Int>();
        int x = pos.x;
        int z = pos.y;

        if (x - 1 >= 0)
        {
            adjacent.Add(new Vector2Int(x - 1, z));
        }
        if (x + 1 < width)
        {
            adjacent.Add(new Vector2Int(x + 1, z));
        }
        if (z - 1 >= 0)
        {
            adjacent.Add(new Vector2Int(x, z - 1));
        }
        if (z + 1 < length)
        {
            adjacent.Add(new Vector2Int(x, z + 1));
        }

        return adjacent;
    }
}
