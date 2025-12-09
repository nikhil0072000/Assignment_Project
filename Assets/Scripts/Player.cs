using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : CharacterMovement
{
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InitializeUnit();
       
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
        TurnManager turnManager = FindFirstObjectByType<TurnManager>();
        if (turnManager != null)
            turnManager.OnPlayerMoveComplete();
    }
}
