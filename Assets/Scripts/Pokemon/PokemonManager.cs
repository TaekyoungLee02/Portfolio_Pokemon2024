using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PokemonManager : MonoBehaviour
{
    public static PokemonManager Instance { get; private set; }
    public List<PokemonInfo> PokemonInfos { get; private set; }

    public Trainer player;
    public Trainer enemy;

    private static RenderTextureDescriptor rtd;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        PokemonInfos = new List<PokemonInfo>();
        rtd = new RenderTextureDescriptor(128, 128);
        UnityEngine.Object[] tempInfos = Resources.LoadAll("Pokemons");

        foreach (PokemonInfo info in tempInfos)
        {
            PokemonInfos.Add(info);
        }
    }

    public void CreatePlayerPokemon(Player player)
    {
        for (int i = 0; i < player.TrainerPokemonInfos.Count; i++)
        {
            var info = player.TrainerPokemonInfos[i];
            player.TrainerPokemons.Add(InstantiatePokemon(PokemonInfos[info.pokemonNum], new Vector3(10 * i, -10, 0), Quaternion.identity, info.level));
            player.TrainerPokemons[i].SetActive(false);
            DontDestroyOnLoad(player.TrainerPokemons[i]);
        }
    }

    public void CreateTrainerPokemons(Trainer trainer)
    {
        int posz;

        if (trainer is Player)
        {
            posz = 0;
        }
        else
        {
            posz = 10;
        }

        for (int i = 0; i < trainer.TrainerPokemonInfos.Count; i ++)
        {
            var info = trainer.TrainerPokemonInfos[i];

            trainer.TrainerPokemons.Add(InstantiatePokemon(PokemonInfos[info.pokemonNum], new Vector3(10 * i, -10, posz), Quaternion.identity, info.level));
        }
    }

    public void BattleEnd(Trainer trainer)
    {
        var playerTrainer = trainer.GetComponent<Trainer>();
        foreach (var i in playerTrainer.Bag) Debug.Log(i.itemName + " : " + i.amount);

        for (int i = 0; i < trainer.TrainerPokemons.Count; i++)
        {
            var pokemon = trainer.TrainerPokemons[i];

            pokemon.SetActive(false);
            pokemon.transform.SetPositionAndRotation(new Vector3(10 * i, -10, 0), Quaternion.identity);
            pokemon.GetComponent<Pokemon>().isBattle = false;
            pokemon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private GameObject InstantiatePokemon(GameObject pokemon, PokemonInfo pokemonInfo, int level = 1)
    {
        var poComponent = pokemon.GetComponent<Pokemon>();
        if (poComponent == null) { poComponent = pokemon.AddComponent<Pokemon>(); }
        poComponent.Init(pokemonInfo, level);

        var porigid = pokemon.GetComponent<Rigidbody>();
        if (porigid == null) { porigid = pokemon.AddComponent<Rigidbody>(); }
        porigid.constraints = RigidbodyConstraints.FreezeAll;

        var texture = new RenderTexture(rtd);
        pokemon.GetComponentInChildren<Camera>().targetTexture = texture;
        poComponent.PokemonTexture = texture;

        return pokemon;
    }
    public GameObject InstantiatePokemon(PokemonInfo pokemonInfo, Vector3 position, Quaternion rotation, int level = 1)
    {
        GameObject pokemon = Instantiate(pokemonInfo.PokemonBody, position, rotation);
        
        return InstantiatePokemon(pokemon, pokemonInfo, level);
    }
    public GameObject InstantiatePokemon(PokemonInfo pokemonInfo, Transform parent, int level = 1)
    {
        GameObject pokemon = Instantiate(pokemonInfo.PokemonBody, parent);

        return InstantiatePokemon(pokemon,pokemonInfo, level);
    }
    public GameObject InstantiatePokemon(int pokemonNum, Vector3 position, Quaternion rotation, int level = 1)
    {
        GameObject pokemon = Instantiate(PokemonInfos[pokemonNum].PokemonBody, position, rotation);

        return InstantiatePokemon(pokemon, PokemonInfos[pokemonNum], level);
    }
    public GameObject InstantiatePokemon(int pokemonNum, Transform parent, int level = 1)
    {
        GameObject pokemon = Instantiate(PokemonInfos[pokemonNum].PokemonBody, parent);

        return InstantiatePokemon(pokemon, PokemonInfos[pokemonNum], level);
    }

    public GameObject Testinst()
    {
        return InstantiatePokemon(PokemonInfos[1], Vector3.zero, Quaternion.identity, 5);
    }
}