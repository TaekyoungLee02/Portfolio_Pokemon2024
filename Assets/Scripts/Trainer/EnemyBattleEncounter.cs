using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LAYER
{
    PLAYER = 128,
    ENEMY = 8,
    POKEMON = 64,
}

public class EnemyBattleEncounter : MonoBehaviour
{
    public static EnemyBattleEncounter Instance { get; private set; }

    public float encounterRadius;
    public GameObject trainerComment;

    private GameObject player;
    private Text commentText;

    private Collider[] enemyColliders;

    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera betweenCamera;
    public CinemachineVirtualCamera enemyCloseupCamera;

    public EventSystem eventSystem;
    public AudioListener audioListener;
    
    private bool isEncounter = false;

    private LayerMask layerMask;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        commentText = trainerComment.GetComponentInChildren<Text>();
        player = GameObject.FindWithTag("Player");
        layerMask = (int)LAYER.ENEMY;
    }

    private void Update()
    {
        enemyColliders = Physics.OverlapSphere(player.transform.position, encounterRadius, layerMask);

        for(int i = 0; i < enemyColliders.Length; i++)
        {
            var collider = enemyColliders[i].gameObject;

            if (!collider.GetComponent<Enemy>().isDefeted && !isEncounter)
            {
                isEncounter = true;
                StartCoroutine(EnemyEncounter(collider));
                break;
            }
        }
    }

    public void WildPokemonEncounter(GameObject wild)
    {
        if(!isEncounter)
        {
            StartCoroutine(WildEncounter(wild));
            isEncounter = true;
        }
    }

    private IEnumerator EnemyEncounter(GameObject enemy)
    {
        // Get Enemy LookAt
        var lookAt = enemy.transform.GetChild(0).transform;

        // Make Player Stop
        player.GetComponent<PlayerMovement>().enabled = false;

        // Set Enemy Close Up VC
        betweenCamera.LookAt = playerCamera.LookAt;
        betweenCamera.transform.SetLocalPositionAndRotation(playerCamera.transform.position, playerCamera.transform.rotation);
        betweenCamera.Priority = 11;
        yield return new WaitForSeconds(Time.deltaTime * 5);

        enemyCloseupCamera.Follow = lookAt;
        enemyCloseupCamera.LookAt = lookAt;
        enemyCloseupCamera.Priority = 12;

        // Set Player Forward
        player.transform.forward = (enemy.transform.position - player.transform.position).normalized;

        // Set Enemy Forward
        DontDestroyOnLoad(enemy);
        enemy.transform.forward = (player.transform.position - enemy.transform.position).normalized;

        yield return new WaitForSeconds(1);

        enemy.GetComponent<Enemy>().EncounterAction();

        trainerComment.SetActive(true);
        foreach (string comment in enemy.GetComponent<Enemy>().encounterComment)
        {
            commentText.text = comment;

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        enemy.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponent<PlayerGravity>().enabled = false;
        StartCoroutine(LoadingSceneManager.LoadScene(enemy));
    }

    private IEnumerator WildEncounter(GameObject wild)
    {
        // Make Player Stop
        player.GetComponent<PlayerMovement>().enabled = false;

        wild.GetComponent<WildPokemon>().StopChasing();
        Destroy(wild.GetComponent<WildPokemon>());
        Destroy(wild.GetComponent<NavMeshAgent>());
        DontDestroyOnLoad(wild);

        eventSystem.enabled = false;
        audioListener.enabled = false;
        AsyncOperation loadScene = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        loadScene.allowSceneActivation = false;

        yield return new WaitUntil(() => loadScene.progress >= 0.9f);
        loadScene.allowSceneActivation = true;
        yield return new WaitUntil(() => loadScene.isDone);

        wild.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<PlayerGravity>().enabled = true;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BattleScene"));
        SceneManager.UnloadSceneAsync("FieldScene");

        player.GetComponent<Animator>().SetBool("isBattle", true);
        BattleManager.Instance.InitWildBattle(player, wild);

        yield break;
    }
}
