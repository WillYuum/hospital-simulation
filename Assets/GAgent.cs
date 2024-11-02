using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GAgent : MonoBehaviour
{

    public List<GAction> actions = new();
    public Dictionary<SubGoal, int> goals = new();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (GAction a in acts)
        {
            actions.Add(a);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }
}


public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sgoals = new Dictionary<string, int>();
        sgoals.Add(s, i);
        remove = r;
    }
}
