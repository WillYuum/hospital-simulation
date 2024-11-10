using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal
{
    // Dictionary to store our goals
    public Dictionary<string, int> sGoals;
    // Bool to store if goal should be removed after it has been achieved
    public bool remove;
    // Constructor
    public SubGoal(string name, int priority, bool removeAfterDone)
    {

        sGoals = new Dictionary<string, int>();
        sGoals.Add(name, priority);
        remove = removeAfterDone;
    }
}

public class GAgent : MonoBehaviour
{
    // Store our list of actions
    public List<GAction> actions = new List<GAction>();
    // Dictionary of subgoals
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    // Our inventory
    public GInventory inventory = new GInventory();
    // Our beliefs
    public WorldStates beliefs = new WorldStates();

    // Access the planner
    GPlanner planner;
    // Action Queue
    Queue<GAction> actionQueue;
    // Our current action
    public GAction currentAction;
    // Our subgoal
    SubGoal currentGoal;

    // Out target destination for the office
    Vector3 destination = Vector3.zero;

    // Start is called before the first frame update
    public void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);
    }


    bool invoked = false;
    //an invoked method to allow an agent to be performing a task
    //for a set location
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    void LateUpdate()
    {
        if (IsCurrentActionRunning())
        {
            HandleRunningAction();
            return;
        }

        if (planner == null || actionQueue == null)
        {
            CreateNewPlan();
        }

        if (IsActionQueueComplete())
        {
            HandleCompletedQueue();
        }

        if (HasPendingActions())
        {
            ExecuteNextAction();
        }
    }

    private bool IsCurrentActionRunning()
    {
        return currentAction != null && currentAction.running;
    }

    private void HandleRunningAction()
    {
        float distanceToTarget = Vector3.Distance(destination, transform.position);

        if (distanceToTarget < 2f && !invoked)
        {
            Invoke(nameof(CompleteAction), currentAction.duration);
            invoked = true;
        }
    }

    private void CreateNewPlan()
    {
        planner = new GPlanner();
        var sortedGoals = goals.OrderByDescending(entry => entry.Value);

        foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
        {
            actionQueue = planner.plan(actions, sg.Key.sGoals, beliefs);

            if (actionQueue != null)
            {
                currentGoal = sg.Key;
                break;
            }
        }
    }

    private bool IsActionQueueComplete()
    {
        return actionQueue != null && actionQueue.Count == 0;
    }

    private void HandleCompletedQueue()
    {
        bool removeGoalAfterDone = currentGoal.remove == true;
        if (currentGoal != null && removeGoalAfterDone)
        {
            goals.Remove(currentGoal);
        }
        planner = null;
    }

    private bool HasPendingActions()
    {
        return actionQueue != null && actionQueue.Count > 0;
    }

    private void ExecuteNextAction()
    {
        currentAction = actionQueue.Dequeue();

        if (currentAction.PrePerform())
        {
            SetCurrentActionTarget();
            StartCurrentAction();
        }
        else
        {
            actionQueue = null;
        }
    }

    private void SetCurrentActionTarget()
    {
        if (currentAction.target == null && !string.IsNullOrEmpty(currentAction.targetTag))
        {
            currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
        }
    }

    private void StartCurrentAction()
    {
        if (currentAction.target != null)
        {
            currentAction.running = true;
            destination = GetDestinationPosition(currentAction.target);
            currentAction.agent.SetDestination(destination);
        }
    }

    Vector3 GetDestinationPosition(GameObject target)
    {
        Transform dest = target.transform.Find("Destination");
        return dest != null ? dest.position : target.transform.position;
    }

}
