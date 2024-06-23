using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallMenu : MonoBehaviour
{
    public GameObject menu;

    public void Call()
    {
        menu.SetActive(true);
        gameObject.SetActive(false);
    }
}
