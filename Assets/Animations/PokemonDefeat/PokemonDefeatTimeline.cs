using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PokemonDefeatTimeline : MonoBehaviour
{
    public GameObject pokemon;
    public GameObject pokeball;
    public GameObject ballEffect;
    public Transform ballPosition;

    private PlayableDirector director;
    private Vector3 scale;

    IEnumerator pokemonReturn;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }


    public void Init(GameObject pokemon, GameObject trainer)
    {
        this.pokemon = pokemon;

        var timeline = director.playableAsset as TimelineAsset;

        foreach (var track in timeline.GetOutputTracks())
        {
            switch (track.name)
            {
                case "Trainer":
                    director.SetGenericBinding(track, trainer);
                    break;

                case "Pokemon":
                    director.SetGenericBinding(track, pokemon);
                    break;

                default:
                    break;
            }
        }
    }


    IEnumerator PokemonReturnCoroutine()
    {
        scale = pokemon.transform.localScale;

        while (true)
        {
            pokeball.transform.position = ballPosition.transform.position;
            yield return null;
        }
    }

    public void TimelineStart()
    {
        pokeball.SetActive(true);

        pokemonReturn = PokemonReturnCoroutine();
        StartCoroutine(pokemonReturn);
    }

    public void PokemonDisappear()
    {
        pokemon.SetActive(false);
        ballEffect.transform.position = ballPosition.transform.position;
    }

    public void TimelineEnd()
    {
        pokemon.transform.localScale = scale;
        pokeball.SetActive(false);
        StopCoroutine(pokemonReturn);
    }
}
