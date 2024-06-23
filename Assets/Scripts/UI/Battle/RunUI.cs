using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunUI : MonoBehaviour
{
    public GameObject commandSelect;
    public GameObject hpDisplay;
    public GameObject battleText;
    public Text text;

    public void Run()
    {
        StartCoroutine(RunCoroutine());
    }

    public IEnumerator RunCoroutine()
    {
        commandSelect.SetActive(false);
        
        if (BattleManager.Instance.isTrainerBattle) 
        {
            hpDisplay.SetActive(false);
            text.text = "�� ��! ��뿡�Լ� ���� ���� ���� ����!";
            battleText.SetActive(true);

            yield return new WaitUntil(() => Input.anyKeyDown);

            battleText.SetActive(false);
            commandSelect.SetActive(true);
            hpDisplay.SetActive(true);
        }
        else
        {
            BattleManager.Instance.SetPlayerCommand(new Command() { type = Command.TYPE.RUN });
        }
    }
}
