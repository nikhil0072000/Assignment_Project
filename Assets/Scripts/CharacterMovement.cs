using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CharacterMovement : MonoBehaviour
{
    public int gridX = 0;
    public int gridZ = 0;
    public float moveSpeed = 3.0f;
    public float unitHeight = 0.5f;

    private TileData currentTile;
    private bool isMoving = false;
    private GridPathfinding pathfinding;

    // Call this to start the unit
    public void InitializeUnit()
    {
        pathfinding = FindFirstObjectByType<GridPathfinding>();
        StartCoroutine(WaitForGridAndInitialize());
    }

    // Wait for the grid to be ready, then put unit on it
    IEnumerator WaitForGridAndInitialize()
    {
        int maxRetries = 50;
        int retries = 0;

        while (retries < maxRetries)
        {
            GameObject tileObject = GameObject.Find($"Tile_{gridX}_{gridZ}");
            if (tileObject != null)
            {
                SetGridPosition(gridX, gridZ);
                yield break;
            }

            retries++;
            yield return new WaitForSeconds(0.1f);
        }

        // If grid not ready, just put unit at calculated spot
        Vector3 worldPos = CalculateWorldPosition(gridX, gridZ);
        transform.position = worldPos;
    }

    // Put unit at a specific grid spot
    public virtual void SetGridPosition(int x, int z)
    {
        if (currentTile != null)
            currentTile.SetOccupied(false);

        gridX = x;
        gridZ = z;

        Vector3 worldPos = CalculateWorldPosition(x, z);
        transform.position = worldPos;

        GameObject tileObject = GameObject.Find($"Tile_{x}_{z}");
        if (tileObject != null)
        {
            currentTile = tileObject.GetComponent<TileData>();
            if (currentTile != null)
                currentTile.SetOccupied(true);
        }
    }

    // Make unit move to a target spot
    public virtual void MoveToPosition(int targetX, int targetZ)
    {
        if (isMoving || pathfinding == null)
            return;

        List<Vector2Int> path = pathfinding.FindPath(gridX, gridZ, targetX, targetZ);

        if (path != null && path.Count > 0)
            StartCoroutine(MoveAlongPath(path));
    }

    // Move unit step by step along the path
    public virtual IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        isMoving = true;

        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int pathPoint = path[i];
            Vector3 targetWorldPos = CalculateWorldPosition(pathPoint.x, pathPoint.y);

            Vector3 direction = (targetWorldPos - transform.position).normalized;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);

            yield return StartCoroutine(MoveToWorldPosition(targetWorldPos));
            UpdateGridPositionSilent(pathPoint.x, pathPoint.y);
        }

        isMoving = false;
        OnMovementComplete();
    }

    // Update grid info while moving (don't trigger events)
   public virtual void UpdateGridPositionSilent(int x, int z)
    {
        if (currentTile != null)
            currentTile.SetOccupied(false);

        gridX = x;
        gridZ = z;

        GameObject tileObject = GameObject.Find($"Tile_{x}_{z}");
        if (tileObject != null)
        {
            currentTile = tileObject.GetComponent<TileData>();
            if (currentTile != null)
                currentTile.SetOccupied(true);
        }
    }

    // Called when unit finishes moving
   public virtual void OnMovementComplete()
    {
        // Tell player or enemy that movement is done
        if (this is Player player)
            player.NotifyMovementComplete();
        else if (this is Enemy Enemy)
            Enemy.NotifyMovementComplete();
    }

    // Smoothly move unit from one spot to another
    public virtual IEnumerator MoveToWorldPosition(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float journeyTime = 0f;
        float journeyLength = Vector3.Distance(startPos, targetPos);
        float totalTime = journeyLength / moveSpeed;

        while (journeyTime < totalTime)
        {
            journeyTime += Time.deltaTime;
            float fractionComplete = journeyTime / totalTime;

            float currentX = Mathf.Lerp(startPos.x, targetPos.x, fractionComplete);
            float currentZ = Mathf.Lerp(startPos.z, targetPos.z, fractionComplete);

            float currentSurfaceHeight = GetTileSurfaceHeight(currentX, currentZ);
            float currentY = currentSurfaceHeight + unitHeight;

            transform.position = new Vector3(currentX, currentY, currentZ);
            yield return null;
        }

        transform.position = targetPos;
    }

    // Convert grid numbers to world position
    public virtual Vector3 CalculateWorldPosition(int x, int z)
    {
        float tileSize = 1.0f;
        float tileSpacing = 0.1f;
        int gridWidth = 10;
        int gridHeight = 10;

        float worldX = (x - gridWidth / 2.0f) * (tileSize + tileSpacing);
        float worldZ = (z - gridHeight / 2.0f) * (tileSize + tileSpacing);

        float surfaceHeight = GetTileSurfaceHeight(worldX, worldZ);
        return new Vector3(worldX, surfaceHeight + unitHeight, worldZ);
    }

    // Find how high the tile surface is
   public virtual float GetTileSurfaceHeight(float worldX, float worldZ)
    {
        Vector3 rayStart = new Vector3(worldX, 10f, worldZ);
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(rayStart, rayDirection, out RaycastHit hit, 20f))
        {
            if (hit.collider.GetComponent<TileData>() != null)
                return hit.point.y;
        }

        return 0f;
    }

    public virtual bool IsMoving() => isMoving;
    public virtual Vector2Int GetGridPosition() => new Vector2Int(gridX, gridZ);
}
