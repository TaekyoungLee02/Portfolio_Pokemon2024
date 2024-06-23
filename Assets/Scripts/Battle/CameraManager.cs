using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public List<GameObject> cineTracks;

    public GameObject playerPokemonCloseupCamera;
    public GameObject enemyPokemonCloseupCamera;


    IEnumerator cameraLoop;

    public void StartCameraLoop()
    {
        cameraLoop = WatingCameraLoop();
        StartCoroutine(cameraLoop);
    }

    public void StopCameraLoop()
    {
        StartCoroutine(StoppingCameraLoop());
        StopCoroutine(cameraLoop);
    }

    public void playerPokemonCloseup()
    {
        StartCoroutine(PokemonCloseUp(playerPokemonCloseupCamera));
    }

    public void enemyPokemonCloseup()
    {
        StartCoroutine(PokemonCloseUp(enemyPokemonCloseupCamera));
    }

    IEnumerator WatingCameraLoop()
    {
        int i = 0;

        while (true)
        {
            var track = cineTracks[i++];

            if (track.GetComponentInChildren<CinemachineDollyCart>() == null) 
            {
                track.SetActive(true);
                yield return new WaitForSeconds(2);
                track.SetActive(false);
            }
            else
            {
                var cart = track.GetComponentInChildren<CinemachineDollyCart>();
                Vector3 start = track.GetComponent<CinemachineSmoothPath>().m_Waypoints[0].position;

                cart.transform.localPosition = start;
                cart.m_Position = 0;
                yield return new WaitForSeconds(1);

                track.SetActive(true);

                yield return new WaitUntil(() => cart.m_Position == 1);

                track.SetActive(false);
                yield return new WaitForSeconds(1);
            }

            if (i >= 6) i = 0;
        }
    }

    IEnumerator StoppingCameraLoop()
    {
        foreach (var track in cineTracks)
        {
            track.SetActive(false);
        }
        yield return new WaitForSeconds(Time.deltaTime);
        cineTracks[0].SetActive(true);
        yield return new WaitForSeconds(Time.deltaTime);
        cineTracks[0].SetActive(false);
    }

    IEnumerator PokemonCloseUp(GameObject cinemachine)
    {
        cinemachine.SetActive(true);
        yield return new WaitForSeconds(Time.deltaTime);
        cinemachine.SetActive(false);
    }
}
