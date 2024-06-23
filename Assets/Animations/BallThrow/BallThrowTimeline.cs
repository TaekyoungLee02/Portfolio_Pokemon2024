using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using DG.Tweening;
using UnityEditor;

public class BallThrowTimeline : MonoBehaviour
{
    public GameObject pokemon;
    public GameObject trainer;
    public GameObject pokeball;
    public Transform ballPosition;
    public Transform ballArrivalPosition;
    public ParticleSystem ballEffect;

    private PlayableDirector director;

    IEnumerator ballinHand;

    IEnumerator ballinHandCoroutine()
    {
        while (true)
        {
            pokeball.transform.position = ballPosition.transform.position;
            yield return null;
        }
    }

    public void Init(GameObject trainer)
    {
        director = GetComponent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;

        foreach (var track in timeline.GetOutputTracks())
        {
            switch (track.name)
            {
                case "Trainer":
                    director.SetGenericBinding(track, trainer);
                    break;

                default:
                    break;
            }
        }
    }

    public void BallThrow(float duration)
    {
        StopCoroutine(ballinHand);
        pokeball.transform.DOMove(ballArrivalPosition.position, duration).SetEase(Ease.Linear).OnComplete(BallEffect);
    }

    public void BallEffect()
    {
        pokeball.SetActive(false);
        ballEffect.Play();
        pokemon.SetActive(true);
        pokemon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }


    public void TimelineStart()
    {
        ballinHand = ballinHandCoroutine();
        pokeball.SetActive(true);
        StartCoroutine(ballinHand);
    }
}
