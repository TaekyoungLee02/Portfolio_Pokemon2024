using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonButton : MonoBehaviour
{
    public Slider playerHP;
    public Image playerHPBarColor;

    public void PlayerChangeHPBarColor()
    {
        if (playerHP.value <= 0.2f) playerHPBarColor.color = new Color(1, 0.294f, 0.294f);
        else if (playerHP.value <= 0.5f) playerHPBarColor.color = new Color(1, 0.745f, 0);
        else playerHPBarColor.color = new Color(0, 0.863f, 0);
    }
}
