using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Crimson Tactics/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    public int gridWidth = 10;
    public int gridHeight = 10;

    private bool[] obstacleGrid;

    private void OnEnable()
    {
        if (obstacleGrid == null || obstacleGrid.Length != gridWidth * gridHeight)
            InitializeGrid();
    }

    // Initialize grid with all tiles empty
    private void InitializeGrid()
    {
        obstacleGrid = new bool[gridWidth * gridHeight];

        for (int i = 0; i < obstacleGrid.Length; i++)
            obstacleGrid[i] = false;
    }

    // Set obstacle state for specific tile
    public void SetObstacle(int x, int z, bool hasObstacle)
    {
        if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
            return;

        int index = z * gridWidth + x;
        obstacleGrid[index] = hasObstacle;

        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    // Check if tile has obstacle
    public bool HasObstacle(int x, int z)
    {
        if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
            return false;

        int index = z * gridWidth + x;
        return obstacleGrid[index];
    }

    // Clear all obstacles
    public void ClearAllObstacles()
    {
        for (int i = 0; i < obstacleGrid.Length; i++)
            obstacleGrid[i] = false;

        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    // Get total obstacle count
    public int GetObstacleCount()
    {
        int count = 0;
        for (int i = 0; i < obstacleGrid.Length; i++)
        {
            if (obstacleGrid[i])
                count++;
        }
        return count;
    }

    // Get obstacle grid as 2D array
    public bool[,] GetObstacleGrid()
    {
        bool[,] grid2D = new bool[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                int index = z * gridWidth + x;
                grid2D[x, z] = obstacleGrid[index];
            }
        }

        return grid2D;
    }
}
