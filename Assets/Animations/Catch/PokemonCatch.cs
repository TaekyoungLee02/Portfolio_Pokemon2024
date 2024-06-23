using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class PokemonCatch : MonoBehaviour
{
    private GameObject pokemon;
    public GameObject pokeBall;

    public PlayableDirector throwDirector;
    public PlayableDirector tickDirector;
    public PlayableDirector catchDirector;
    public PlayableDirector failDirector;

    private int count;
    private bool catched;

    private Vector3 scale;

    public void Init(GameObject catchingPokemon)
    {
        pokemon = catchingPokemon;
        var timeline = throwDirector.playableAsset as TimelineAsset;

        foreach (var track in timeline.GetOutputTracks())
        {
            switch (track.name)
            {
                case "Pokemon":
                    throwDirector.SetGenericBinding(track, catchingPokemon);
                    break;

                default:
                    break;
            }
        }
    }

    public void Play(int tickCount, bool catched)
    {
        this.count = tickCount;
        this.catched = catched;

        scale = pokemon.transform.localScale;
        pokeBall.SetActive(true);
        throwDirector.stopped += CatchBallThrow;
        throwDirector.Play();
    }    

    public void CatchBallThrow(PlayableDirector director)
    {
        pokemon.SetActive(false);
        pokemon.transform.localScale = scale;

        StartCoroutine(PlayCatchAnimation(count, catched));
    }

    private IEnumerator PlayCatchAnimation(int count, bool catched)
    {
        Debug.Log(count);
        Debug.Log(catched);

        for (int i = 0; i < count; i++)
        {
            tickDirector.Play();
            yield return new WaitUntil(() => tickDirector.state == PlayState.Paused);
        }

        if (catched)
        {
            catchDirector.stopped += Successed;
            catchDirector.Play();
        }
        else failDirector.Play();
    }

    public void Successed(PlayableDirector director)
    {
        BattleManager.Instance.CatchSuccessed(true); 
    }

    public void Failed()
    {
        pokemon.transform.position = new Vector3(pokemon.transform.position.x, pokemon.transform.position.y + 1, pokemon.transform.position.z);

        pokemon.SetActive(true);

        

        BattleManager.Instance.CatchSuccessed(false);
    }
}
