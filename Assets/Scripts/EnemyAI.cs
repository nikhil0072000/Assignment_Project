using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, IAI
{
    public int targetDistance = 1;
    public float decisionDelay = 0.5f;

    private Transform currentTarget;
    private Enemy enemy;
    private bool isProcessing = false;
    private bool waitingForPlayerMove = false;
    private Vector2Int lastPlayerPosition;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Check if player moved and react
    void Update()
    {
        if (waitingForPlayerMove && currentTarget != null)
        {
            Player Player = currentTarget.GetComponent<Player>();
            if (Player != null)
            {
                if (HasPlayerMoved() && !Player.IsMoving())
                {
                    waitingForPlayerMove = false;
                    lastPlayerPosition = Player.GetGridPosition();
                    Invoke("ExecuteAI", decisionDelay);
                }
            }
        }
    }

    // Make AI think and decide what to do
    public void ExecuteAI()
    {
        if (!CanAct() || currentTarget == null || enemy == null)
            return;

        isProcessing = true;

        Player Player = currentTarget.GetComponent<Player>();
        if (Player == null)
        {
            isProcessing = false;
            return;
        }

        Vector2Int playerPos = Player.GetGridPosition();
        Vector2Int enemyPos = enemy.GetGridPosition();

        lastPlayerPosition = playerPos;

        int distanceToPlayer = Mathf.Abs(playerPos.x - enemyPos.x) + Mathf.Abs(playerPos.y - enemyPos.y);

       
        if (distanceToPlayer <= targetDistance)
        {
            waitingForPlayerMove = true;
            isProcessing = false;
            return;
        }

        
        Vector2Int bestTarget = FindBestAdjacentTile(playerPos, enemyPos);

        if (bestTarget.x != -1)
            enemy.MoveToPosition(bestTarget.x, bestTarget.y);
        else
            isProcessing = false;
    }

    // Find best spot next to player
    Vector2Int FindBestAdjacentTile(Vector2Int playerPos, Vector2Int enemyPos)
    {
        Vector2Int[] adjacentOffsets = {
            new Vector2Int(0, 1), new Vector2Int(1, 0),
            new Vector2Int(0, -1), new Vector2Int(-1, 0)
        };

        Vector2Int bestTarget = new Vector2Int(-1, -1);
        float shortestDistance = float.MaxValue;

        foreach (Vector2Int offset in adjacentOffsets)
        {
            Vector2Int adjacentPos = playerPos + offset;

            if (IsValidTarget(adjacentPos))
            {
                float distance = Vector2Int.Distance(enemyPos, adjacentPos);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestTarget = adjacentPos;
                }
            }
        }

        return bestTarget;
    }

    // Check if we can move to this spot
    bool IsValidTarget(Vector2Int targetPos)
    {
        if (targetPos.x < 0 || targetPos.x >= 10 || targetPos.y < 0 || targetPos.y >= 10)
            return false;

        GameObject tileObject = GameObject.Find($"Tile_{targetPos.x}_{targetPos.y}");
        if (tileObject != null)
        {
            TileData tileData = tileObject.GetComponent<TileData>();
            if (tileData != null)
                return tileData.IsWalkable();
        }

        return false;
    }

    // Check if player moved to a new spot
    bool HasPlayerMoved()
    {
        if (currentTarget == null) return false;

        Player Player = currentTarget.GetComponent<Player>();
        if (Player == null) return false;

        Vector2Int currentPlayerPos = Player.GetGridPosition();
        return currentPlayerPos != lastPlayerPosition;
    }

    // Check if AI can do something right now
    public bool CanAct()
    {
        return !isProcessing && !waitingForPlayerMove && 
               (enemy != null && !enemy.IsMoving());
    }

    // Tell AI who to chase
    public void SetTarget(Transform target)
    {
        currentTarget = target;

        if (target != null)
        {
            Player Player = target.GetComponent<Player>();
            if (Player != null)
                lastPlayerPosition = Player.GetGridPosition();
        }
    }

    // Get who AI is chasing
    public Transform GetTarget()
    {
        return currentTarget;
    }

    // Reset AI back to start
    public void ResetAI()
    {
        isProcessing = false;
        waitingForPlayerMove = false;
        lastPlayerPosition = Vector2Int.zero;
    }

    // Called when enemy finishes moving
    public void OnMovementComplete()
    {
        isProcessing = false;

        if (currentTarget != null && enemy != null)
        {
            Player Player = currentTarget.GetComponent<Player>();
            if (Player != null)
            {
                Vector2Int playerPos = Player.GetGridPosition();
                Vector2Int enemyPos = enemy.GetGridPosition();
                int distance = Mathf.Abs(playerPos.x - enemyPos.x) + Mathf.Abs(playerPos.y - enemyPos.y);

                if (distance <= targetDistance)
                    waitingForPlayerMove = true;
            }
        }
    }
}
