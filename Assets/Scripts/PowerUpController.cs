using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpController : MonoBehaviour
{
    public int iNumProjectile = 20;
    public TextMeshProUGUI guiLabel1;
    public TextMeshProUGUI guiLabel2;
    public TextMeshProUGUI guiLabel3;

    private float fDegreesPerSecond = 90f;
    private float fDegreesPerFrame;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        guiLabel1.text = iNumProjectile.ToString() + "\n*";
        guiLabel2.text = iNumProjectile.ToString() + "\n*";
        guiLabel3.text = iNumProjectile.ToString() + "\n*";
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        fDegreesPerFrame = fDegreesPerSecond * Time.deltaTime;
        transform.Rotate(0f, fDegreesPerFrame, 0f, Space.World);
    }

    // ------------------------------------------------------------------------------------------------

}
