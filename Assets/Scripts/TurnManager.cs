using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public float enemyTurnDelay = 1f;

    private List<Enemy> Enemys = new List<Enemy>();
    private Player Player;
    private bool isPlayerTurn = true;

    void Start()
    {
        Player = FindFirstObjectByType<Player>();
        FindAllEnemys();
        SetupEnemyTargets();
    }

    // Find all enemies in the game
    void FindAllEnemys()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Enemys.Clear();

        foreach (Enemy enemy in enemies)
            Enemys.Add(enemy);
    }

    // Tell all enemies to chase the player
    void SetupEnemyTargets()
    {
        if (Player == null) return;

        foreach (Enemy enemy in Enemys)
            enemy.SetAITarget(Player.transform);
    }

    // Called when player finishes moving
    public void OnPlayerMoveComplete()
    {
        if (!isPlayerTurn) return;

        if (Player != null && Player.IsMoving())
            return;

        isPlayerTurn = false;
        Invoke("ExecuteEnemyTurn", enemyTurnDelay);
    }

    // Make all enemies take their turn
    void ExecuteEnemyTurn()
    {
        foreach (Enemy enemy in Enemys)
        {
            if (enemy != null && enemy.CanAct())
                enemy.ExecuteAI();
        }

        Invoke("CheckEnemyTurnComplete", 0.5f);
    }

    // Check if all enemies finished moving
    void CheckEnemyTurnComplete()
    {
        bool allEnemiesFinished = true;

        foreach (Enemy enemy in Enemys)
        {
            if (enemy != null && enemy.IsMoving())
            {
                allEnemiesFinished = false;
                break;
            }
        }

        if (allEnemiesFinished)
            OnEnemyTurnComplete();
        else
            Invoke("CheckEnemyTurnComplete", 0.2f);
    }

    // Called when all enemies finish their turn
    void OnEnemyTurnComplete()
    {
        isPlayerTurn = true;
    }

    // Check if it's player's turn
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    // Add new enemy to the game
    public void AddEnemy(Enemy enemy)
    {
        if (enemy != null && !Enemys.Contains(enemy))
        {
            Enemys.Add(enemy);
            enemy.SetAITarget(Player != null ? Player.transform : null);
        }
    }

    // Remove enemy from the game
    public void RemoveEnemy(Enemy enemy)
    {
        if (Enemys.Contains(enemy))
            Enemys.Remove(enemy);
    }
}
