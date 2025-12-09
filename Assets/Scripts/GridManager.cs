using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSize = 1.0f;
    public float tileSpacing = 0.1f;
    public GameObject tilePrefab;
    public Transform gridParent;

    private TileData[,] gridTiles;

    void Start()
    {
        GenerateGrid();
    }

    // Generate the complete grid of tiles
    private void GenerateGrid()
    {
        gridTiles = new TileData[gridWidth, gridHeight];

        if (gridParent == null)
        {
            GameObject parentObj = new GameObject("Grid_Tiles");
            gridParent = parentObj.transform;
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                CreateTileAtPosition(x, z);
            }
        }
    }

    // Create single tile at specified position
    private void CreateTileAtPosition(int gridX, int gridZ)
    {
        float worldX = (gridX - gridWidth / 2.0f) * (tileSize + tileSpacing);
        float worldZ = (gridZ - gridHeight / 2.0f) * (tileSize + tileSpacing);
        Vector3 tilePosition = new Vector3(worldX, 0, worldZ);

        GameObject tileObject;
        if (tilePrefab != null)
        {
            tileObject = Instantiate(tilePrefab, tilePosition, Quaternion.identity, gridParent);
        }
        else
        {
            tileObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tileObject.transform.position = tilePosition;
            tileObject.transform.parent = gridParent;
        }

        TileData tileData = tileObject.GetComponent<TileData>();
        if (tileData == null)
            tileData = tileObject.AddComponent<TileData>();

        tileData.InitializeTile(gridX, gridZ);
        gridTiles[gridX, gridZ] = tileData;
    }

    // Get tile at specified coordinates
    public TileData GetTile(int x, int z)
    {
        if (IsValidGridPosition(x, z))
            return gridTiles[x, z];
        return null;
    }

    // Check if coordinates are within bounds
    private bool IsValidGridPosition(int x, int z)
    {
        return x >= 0 && x < gridWidth && z >= 0 && z < gridHeight;
    }

    // Get tile at world position
    public TileData GetTileAtWorldPosition(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt((worldPosition.x / (tileSize + tileSpacing)) + (gridWidth / 2.0f));
        int gridZ = Mathf.RoundToInt((worldPosition.z / (tileSize + tileSpacing)) + (gridHeight / 2.0f));

        if (IsValidGridPosition(gridX, gridZ))
        {
            TileData tile = GetTile(gridX, gridZ);
            if (tile != null)
                tile.OnMouseHover();
            return tile;
        }

        return null;
    }
}
