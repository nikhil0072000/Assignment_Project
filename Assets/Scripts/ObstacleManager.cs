using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;
    
    
    public float obstacleSize = 0.8f;
    public float heightOffset = 0.5f;

    private GameObject[] obstacleObjects;
   
    void Start()
    {
       

        if (obstacleData == null)
            return;

        obstacleObjects = new GameObject[obstacleData.gridWidth * obstacleData.gridHeight];
        GenerateObstacles();
    }

    // Generate visual obstacles from data
    public void GenerateObstacles()
    {
        ClearAllObstacles();

        bool[,] obstacleGrid = obstacleData.GetObstacleGrid();

        for (int x = 0; x < obstacleData.gridWidth; x++)
        {
            for (int z = 0; z < obstacleData.gridHeight; z++)
            {
                if (obstacleGrid[x, z])
                    CreateObstacleAt(x, z);
            }
        }
    }

    // Create visual obstacle at position
    void CreateObstacleAt(int x, int z)
    {
        Vector3 worldPosition = CalculateWorldPosition(x, z);

        GameObject obstacle;

        if (obstaclePrefab != null)
        {
            obstacle = Instantiate(obstaclePrefab, worldPosition, Quaternion.identity, transform);
        }
        else
        {
            obstacle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obstacle.transform.position = worldPosition;
            obstacle.transform.parent = transform;
            obstacle.transform.localScale = Vector3.one * obstacleSize;
        }


        int index = z * obstacleData.gridWidth + x;
        obstacleObjects[index] = obstacle;

        UpdateTileObstacleState(x, z, true);
    }

    // Calculate world position for obstacle
    Vector3 CalculateWorldPosition(int x, int z)
    {
        float tileSize = 1.0f;
        float tileSpacing = 0.1f;

        float worldX = (x - obstacleData.gridWidth / 2.0f) * (tileSize + tileSpacing);
        float worldZ = (z - obstacleData.gridHeight / 2.0f) * (tileSize + tileSpacing);

        Vector3 position = new Vector3(worldX, heightOffset, worldZ);
        return position;
    }

    // Update tile obstacle state
    void UpdateTileObstacleState(int x, int z, bool hasObstacle)
    {
        GameObject tileObject = GameObject.Find($"Tile_{x}_{z}");
        if (tileObject != null)
        {
            TileData tileData = tileObject.GetComponent<TileData>();
            if (tileData != null)
                tileData.SetObstacle(hasObstacle);
        }
    }

    // Clear all obstacle GameObjects
    public void ClearAllObstacles()
    {
        if (obstacleObjects != null)
        {
            for (int i = 0; i < obstacleObjects.Length; i++)
            {
                if (obstacleObjects[i] != null)
                {
                    string[] nameParts = obstacleObjects[i].name.Split('_');
                    if (nameParts.Length >= 3)
                    {
                        int x = int.Parse(nameParts[1]);
                        int z = int.Parse(nameParts[2]);
                        UpdateTileObstacleState(x, z, false);
                    }

                    if (Application.isPlaying)
                        Destroy(obstacleObjects[i]);
                    else
                        DestroyImmediate(obstacleObjects[i]);
                }

                obstacleObjects[i] = null;
            }
        }
    }

    // Refresh obstacles from data
    [ContextMenu("Refresh Obstacles")]
    public void RefreshObstacles()
    {
        if (obstacleData != null)
            GenerateObstacles();
    }

    void OnDestroy()
    {
        ClearAllObstacles();
    }
}
