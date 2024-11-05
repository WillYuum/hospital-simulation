using UnityEngine;

public class GetTreated : GAction
{
    public override bool PostPerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;


        return true;
    }

    public override bool PrePerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Treated", 1);

        inventory.RemoveItem(target);

        return true;
    }
}
