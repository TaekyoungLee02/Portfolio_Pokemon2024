using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpNotice : MonoBehaviour
{
    public Text hp;
    public Text hpDiff;

    public Text atk;
    public Text atkDiff;

    public Text def;
    public Text defDiff;

    public Text spd;
    public Text spdDiff;

    public void LevelUp(int maxHP, int atk, int def, int spd, List<int> statDiff)
    {
        hp.text = maxHP.ToString();
        hpDiff.text = "+" + statDiff[0];

        this.atk.text = atk.ToString();
        atkDiff.text = "+" + statDiff[1];

        this.def.text = def.ToString();
        defDiff.text = "+" + statDiff[2];

        this.spd.text = spd.ToString();
        spdDiff.text = "+" + statDiff[3];
    }
}
