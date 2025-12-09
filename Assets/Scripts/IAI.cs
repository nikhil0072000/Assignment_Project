using UnityEngine;

public interface IAI
{
    void ExecuteAI(); // Make AI think and act
    bool CanAct(); // Check if AI can do something
    void SetTarget(Transform target); // Tell AI who to chase
    Transform GetTarget(); // Get who AI is chasing
    void ResetAI(); // Reset AI back to start
}
