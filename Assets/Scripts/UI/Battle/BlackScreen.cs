using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    private Image screen;

    private void Awake()
    {
        screen = GetComponent<Image>();
    }

    public void Blacken()
    {
        screen.color = new Color(0, 0, 0, 0);
        StartCoroutine(BlackenCoroutine());
    }

    public void UnBlacken()
    {
        StartCoroutine (UnBlackenCoroutine());
        screen.color = new Color(0, 0, 0);
    }

    private IEnumerator BlackenCoroutine()
    {
        for(int i = 0; i < 50; i++)
        {
            screen.color = new Color(0, 0, 0, 0.02f * i);
            yield return null;
        }
        screen.color = new Color(0, 0, 0);
    }

    private IEnumerator UnBlackenCoroutine()
    {
        for (int i = 0; i < 50; i++)
        {
            screen.color = new Color(0, 0, 0, 1 - (0.02f * i));
            yield return null;
        }
        screen.color = new Color(0, 0, 0, 0);
        gameObject.SetActive(false);
    }
}
