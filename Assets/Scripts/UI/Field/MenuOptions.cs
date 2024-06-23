using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    public GameObject pokemonCanvas;
    public GameObject bagCanvas;
    public GameObject menu;

    private Player player;
    private PlayerMovement playerMovement;
    private List<Item> bag;
    private List<GameObject> trainerPokemons;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        bag = player.Bag;
        trainerPokemons = player.TrainerPokemons;
        playerMovement = player.GetComponent<PlayerMovement>();

        pokemonCanvas.GetComponent<FieldPokemonUI>().Init(trainerPokemons, playerMovement);
        bagCanvas.GetComponent<FieldBagUI>().Init(trainerPokemons, bag, playerMovement);
    }

    public void CallPokemonCanvas()
    {
        pokemonCanvas.GetComponent<FieldPokemonUI>().PokemonButtonUpdate();
        pokemonCanvas.GetComponent<Canvas>().enabled = true;
        gameObject.SetActive(false);
        playerMovement.enabled = false;
    }

    public void CallBagCanvas()
    {
        bagCanvas.GetComponent<FieldBagUI>().BagUIUPdate();
        bagCanvas.GetComponent<Canvas>().enabled = true;
        gameObject.SetActive(false);
        playerMovement.enabled = false;
    }

    public void ReturnButton()
    {
        menu.SetActive(true);
        gameObject.SetActive(false);
    }
}
