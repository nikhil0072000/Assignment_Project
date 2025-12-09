using UnityEngine;

public class TileData : MonoBehaviour
{
    private int gridX;
    private int gridZ;
    private bool isOccupied = false;
    private bool hasObstacle = false;
    public Material defaultMaterial;
    public Material hoverMaterial;

    private Renderer tileRenderer;
    private bool isCurrentlyHovered = false;

    // Set up this tile with its grid position
    public void InitializeTile(int x, int z)
    {
        gridX = x;
        gridZ = z;

        tileRenderer = GetComponent<Renderer>();
        gameObject.name = $"Tile_{x}_{z}";
    }

    // Get tile position as text
    public string GetPositionString()
    {
        return $"Grid Position: ({gridX}, {gridZ})";
    }

    public int GetGridX() => gridX;
    public int GetGridZ() => gridZ;

    // Check if units can walk on this tile
    public bool IsWalkable()
    {
        return !hasObstacle && !isOccupied;
    }

    // Set if this tile has an obstacle
    public void SetObstacle(bool hasObstacle)
    {
        this.hasObstacle = hasObstacle;
    }

    // Set if a unit is standing on this tile
    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }

    // Change tile color when mouse hovers over it
    public void OnMouseHover()
    {
        if (!isCurrentlyHovered && hoverMaterial != null)
        {
            isCurrentlyHovered = true;
            tileRenderer.material = hoverMaterial;
        }
    }

    // Change tile color back when mouse leaves
    public void OnMouseExit()
    {
        if (isCurrentlyHovered && defaultMaterial != null)
        {
            isCurrentlyHovered = false;
            tileRenderer.material = defaultMaterial;
        }
    }
}
