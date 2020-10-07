using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTriggerController : MonoBehaviour
{
    public enum trigger {on, off};
    public trigger trigType;
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
        if ((trigType == trigger.on)
        &&  switchController.bPositionFullyOff
        &&  switchController.slistOkayTriggerCharacters.Contains(other.gameObject.tag))
        {
            switchController.sTriggerCharacter = other.gameObject.tag;
            switchController.StartRotate(-1);
        }
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerExit(Collider other)
    {
        if ((trigType == trigger.off)
        &&  !switchController.bPositionFullyOff
        &&  (other.gameObject.tag == switchController.sTriggerCharacter))
        {
            switchController.StartRotate(1);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
