using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPokemonChangeCanvas : MonoBehaviour
{
    public GameObject pokemonChangeCanvas;
    public GameObject commandSelect;
    public GameObject hpDisplay;

    public void Call()
    {
        pokemonChangeCanvas.GetComponent<PokemonChangeUI>().PokemonButtonUpdate();
        commandSelect.SetActive(false);
        hpDisplay.SetActive(false);

        pokemonChangeCanvas.GetComponent<Canvas>().enabled = true;
    }
}
