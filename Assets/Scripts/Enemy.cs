using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : CharacterMovement
{
    public EnemyAI enemyAI;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InitializeUnit();
        SetGridPosition(gridX, gridZ);

        if (enemyAI == null)
        {
            enemyAI = GetComponent<EnemyAI>();
            if (enemyAI == null)
                enemyAI = gameObject.AddComponent<EnemyAI>();
        }

       
    }

   
    public override IEnumerator MoveAlongPath(List<Vector2Int> path)
    {
        if (animator != null)
            animator.SetBool("isRunning", true);

        yield return StartCoroutine(base.MoveAlongPath(path));

        if (animator != null)
            animator.SetBool("isRunning", false);
    }

    public void NotifyMovementComplete()
    {
        if (enemyAI != null)
            enemyAI.OnMovementComplete();
    }

    public void ExecuteAI()
    {
        if (enemyAI != null && enemyAI.CanAct())
            enemyAI.ExecuteAI();
    }

    public void SetAITarget(Transform target)
    {
        if (enemyAI != null)
            enemyAI.SetTarget(target);
    }

    public bool CanAct()
    {
        return !IsMoving() && (enemyAI != null ? enemyAI.CanAct() : false);
    }

    [ContextMenu("Update Position to Grid Coordinates")]
    public void UpdatePositionToGrid()
    {
        SetGridPosition(gridX, gridZ);
    }
}
