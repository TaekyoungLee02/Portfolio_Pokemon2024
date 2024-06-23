using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTextSelect : MonoBehaviour
{
    BattleManager battleManager;

    private void Start()
    {
        battleManager = BattleManager.Instance;
    }

    public void Yes()
    {
        battleManager.BattleTextSelect(true);
    }

    public void No()
    {
        battleManager.BattleTextSelect(false);
    }
}
