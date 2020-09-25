using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for GetComponent<Image>()

public class ButtonImageController : MonoBehaviour
{
    public KeyCode keyCode1;
    public KeyCode keyCode2;

    private Image imgBackground;
    private Color colKeyDown = new Color(255f/255f, 54f/255f, 98f/255f, 255f/255f);
    private Color colKeyUp;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        imgBackground = GetComponent<Image>();
        colKeyUp = imgBackground.color;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(keyCode1) && !Input.GetKey(keyCode2))
        ||  (Input.GetKeyDown(keyCode2) && !Input.GetKey(keyCode1)))
        {
            imgBackground.color = colKeyDown;
        }
        else if ((Input.GetKeyUp(keyCode1) && !Input.GetKey(keyCode2))
        ||       (Input.GetKeyUp(keyCode2) && !Input.GetKey(keyCode1)))
        {
            imgBackground.color = colKeyUp;
        }
    }

    // ------------------------------------------------------------------------------------------------

}
