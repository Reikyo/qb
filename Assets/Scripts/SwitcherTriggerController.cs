using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitcherTriggerController : MonoBehaviour
{
    public enum trigger {state1to2, state2to1};
    public trigger triggerType;
    private SwitcherController switcherController;

    // ------------------------------------------------------------------------------------------------

    void Start()
    {
        switcherController = transform.parent.GetComponent<SwitcherController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == trigger.state1to2)
        {
            switcherController.Trigger("state1to2", other.gameObject.tag);
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerExit(Collider other)
    {
        if (triggerType == trigger.state2to1)
        {
            switcherController.Trigger("state2to1", other.gameObject.tag);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
