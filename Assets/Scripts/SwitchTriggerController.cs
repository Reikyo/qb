using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTriggerController : MonoBehaviour
{
    public enum trigger {state1to2, state2to1};
    public trigger triggerType;
    private SwitchController switchController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        switchController = transform.parent.GetComponent<SwitchController>();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (triggerType == trigger.state1to2)
        {
            switchController.Trigger("state1to2", other.gameObject.tag);
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerExit(Collider other)
    {
        if (triggerType == trigger.state2to1)
        {
            switchController.Trigger("state2to1", other.gameObject.tag);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
