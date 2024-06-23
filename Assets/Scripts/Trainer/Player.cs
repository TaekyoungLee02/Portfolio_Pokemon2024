using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Trainer
{
    public static Player Instance { get; private set; }

    public GameObject playerPos;

    private void Awake()
    {
        if(Instance == null)
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);

            playerPos = new GameObject();
            playerPos.name = "PlayerPosition";
            playerPos.transform.SetPositionAndRotation(transform.position, transform.rotation);
            DontDestroyOnLoad(playerPos);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        TrainerInfo playerInfo = Resources.Load("Trainers/Player/PlayerInfo") as TrainerInfo;
        Init(playerInfo);
        Bag = ItemManager.Instance.CreateBagFromTrainerBagInfo(playerInfo.trainerBagInfos);

        PokemonManager.Instance.CreatePlayerPokemon(this);
    }

    public void EnablePlayerMovementWhenSceneLoaded()
    {
        GetComponent<PlayerGravity>().enabled = false;
        transform.SetLocalPositionAndRotation(playerPos.transform.position, playerPos.transform.rotation);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerGravity>().enabled = true;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent<WildPokemon>(out var poke))
        {
            EnemyBattleEncounter.Instance.WildPokemonEncounter(hit.gameObject);
        }
    }

    private void OnDestroy()
    {
        Destroy(playerPos);
    }
}
