using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBagCanvas : MonoBehaviour
{
    public GameObject bagCanvas;
    public GameObject commandSelect;
    public GameObject hpDisplay;

    public void Call()
    {
        bagCanvas.GetComponent<BagUI>().BagUIUPdate();
        commandSelect.SetActive(false);
        hpDisplay.SetActive(false);

        bagCanvas.GetComponent<Canvas>().enabled = true;
    }
}
