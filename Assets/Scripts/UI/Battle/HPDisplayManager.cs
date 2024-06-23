using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplayManager : MonoBehaviour
{
    public Text playerPokemonName;
    public Text enemyPokemonName;

    public Text playerPokemonLevel;
    public Text enemyPokemonLevel;

    public Text playerPokemonHP;

    public Slider playerHP;
    public Slider enemyHP;
    public Slider playerPokemonEXP;

    public Image playerHPBarColor;
    public Image enemyHPBarColor;

    public Image playerStatusEffect;
    public Image enemyStatusEffect;

    public bool valueChangeFinished = false;
    IEnumerator sliderUpdater;

    public List<Sprite> statusEffects;

    private float barSpeed = 0.01f;

    public void SetPlayerPokemonName(string pokemonName)
    {
        this.playerPokemonName.text = pokemonName;
    }

    public void SetEnemyPokemonName(string pokemonName)
    {
        this.enemyPokemonName.text = pokemonName;
    }

    public void SetPlayerPokemonLevel(int level)
    {
        this.playerPokemonLevel.text = "Lv." + level;
    }

    public void SetEnemyPokemonLevel(int level)
    {
        this.enemyPokemonLevel.text = "Lv." + level;
    }

    public void SetPlayerPokemonHP(int curHP, int maxHP)
    {
        this.playerPokemonHP.text = curHP + "/" + maxHP;
    }

    public void SetPlayerPokemonEXP(int maxExp, int curExp = 0)
    {
        playerPokemonEXP.value = (float)curExp / maxExp;
    }

    public void SetPlayerHPBar(int curHP, int maxHP)
    {
        playerHP.value = (float)curHP / maxHP;
    }

    public void SetEnemyHPBar(int curHP, int maxHP)
    {
        enemyHP.value = (float)curHP / maxHP;
    }

    public void PlayerHPValueChange(int curHP, int maxHP)
    {
        valueChangeFinished = false;
        sliderUpdater = null;

        if (playerHP.value > (float)curHP / maxHP)
        {
            sliderUpdater = PlayerHPDamaged(playerHP, playerPokemonHP, curHP, maxHP);
        }
        else if (playerHP.value < (float)curHP / maxHP)
        {
            sliderUpdater = PlayerHPHealed(playerHP, playerPokemonHP, curHP, maxHP);
        }

        if(sliderUpdater != null)
            StartCoroutine(sliderUpdater);
    }

    public void EnemyHPValueChange(int curHP, int maxHP)
    {
        valueChangeFinished = false;
        sliderUpdater = null;

        if (enemyHP.value > (float)curHP / maxHP)
        {
            sliderUpdater = EnemyHPDamaged(enemyHP, curHP, maxHP);
        }
        else if (enemyHP.value < (float)curHP / maxHP)
        {
            sliderUpdater = EnemyHPHealed(enemyHP, curHP, maxHP);
        }

        if (sliderUpdater != null)
            StartCoroutine(sliderUpdater);
    }

    public void PlayerExpGain(int exp, int maxExp)
    {
        valueChangeFinished = false;
        sliderUpdater = null;

        sliderUpdater = ExpGain(exp, maxExp);
        StartCoroutine(sliderUpdater);
    }

    IEnumerator PlayerHPDamaged(Slider slider, Text hpText, int curHP, int maxHP)
    {
        while (slider.value > (float)curHP / maxHP)
        {
            slider.value -= barSpeed;

            if((int)(slider.value * maxHP) >= curHP)                
                hpText.text = (int)(slider.value * maxHP) + "/" + maxHP;

            yield return null;
        }

        slider.value = (float)curHP / maxHP;
        hpText.text = curHP + "/" + maxHP;
        yield return new WaitForSeconds(2);

        valueChangeFinished = true;
        yield break;
    }

    IEnumerator PlayerHPHealed(Slider slider, Text hpText, int curHP, int maxHP)
    {
        while (slider.value < (float)curHP / maxHP)
        {
            slider.value += barSpeed;

            if ((int)(slider.value * maxHP) <= curHP)
                hpText.text = (int)(slider.value * maxHP) + "/" + maxHP;

            yield return null;
        }

        slider.value = (float)curHP / maxHP;
        hpText.text = curHP + "/" + maxHP;
        yield return new WaitForSeconds(2);

        valueChangeFinished = true;
        yield break;
    }

    IEnumerator EnemyHPDamaged(Slider slider, int curHP, int maxHP)
    {
        while (slider.value > (float)curHP / maxHP)
        {
            slider.value -= barSpeed;

            yield return null;
        }

        slider.value = (float)curHP / maxHP;
        yield return new WaitForSeconds(2);

        valueChangeFinished = true;
        yield break;
    }

    IEnumerator EnemyHPHealed(Slider slider, int curHP, int maxHP)
    {
        while (slider.value < (float)curHP / maxHP)
        {
            slider.value += barSpeed;

            yield return null;
        }

        slider.value = (float)curHP / maxHP;
        yield return new WaitForSeconds(2);

        valueChangeFinished = true;
        yield break;
    }

    IEnumerator ExpGain(int exp, int maxExp)
    {
        while(playerPokemonEXP.value < (float)exp / maxExp && playerPokemonEXP.value < 1)
        {
            playerPokemonEXP.value += barSpeed;

            yield return null;
        }

        playerPokemonEXP.value = (float)exp / maxExp;

        if (playerPokemonEXP.value == 1) playerPokemonEXP.value = 0;
        yield return new WaitForSeconds(1);

        valueChangeFinished = true;
        yield break;
    }


    public void EnemyChangeHPBarColor()
    {
        if (enemyHP.value <= 0.2f) enemyHPBarColor.color = new Color(1, 0.294f, 0.294f);
        else if (enemyHP.value <= 0.5f) enemyHPBarColor.color = new Color(1, 0.745f, 0);
        else enemyHPBarColor.color = new Color(0, 0.863f, 0);
    }

    public void PlayerChangeHPBarColor()
    {
        if (playerHP.value <= 0.2f) playerHPBarColor.color = new Color(1, 0.294f, 0.294f);
        else if (playerHP.value <= 0.5f) playerHPBarColor.color = new Color(1, 0.745f, 0);
        else playerHPBarColor.color = new Color(0, 0.863f, 0);
    }

    public void SetPlayerStatusEffect(STATUSEFFECT statusEffect)
    {
        playerStatusEffect.sprite = statusEffects[(int)statusEffect];
    }

    public void SetEnemyStatusEffect(STATUSEFFECT statusEffect)
    {
        enemyStatusEffect.sprite = statusEffects[(int)statusEffect];
    }

}
