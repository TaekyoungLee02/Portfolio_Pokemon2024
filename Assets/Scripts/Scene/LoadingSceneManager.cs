using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public GameObject blackScreen;

    public RawImage playerPortait;
    public RawImage enemyPortait;

    public Text playerName;
    public Text enemyName;

    public Camera playerRenderCamera;
    public Camera enemyRenderCamera;

    public Transform playerPosition;
    public Transform enemyPosition;

    private GameObject player;
    private static GameObject enemy;

    private RenderTexture playerTexture;
    private RenderTexture enemyTexture;

    private AsyncOperation deleteScene;

    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        // Set Render Texture
        playerTexture = new RenderTexture(new RenderTextureDescriptor(1024, 1024));
        enemyTexture = new RenderTexture(new RenderTextureDescriptor(1024, 1024));

        playerRenderCamera.targetTexture = playerTexture;
        enemyRenderCamera.targetTexture = enemyTexture;

        playerPortait.texture = playerTexture;
        enemyPortait.texture = enemyTexture;

        // Set Player Animation
        player.GetComponent<Animator>().SetBool("isBattle", true);
        enemy.GetComponent<Animator>().SetBool("isBattle", true);

        // Set Player Transform
        player.transform.position = playerPosition.position;
        enemy.transform.position = enemyPosition.position;

        player.transform.rotation = playerPosition.rotation;
        enemy.transform.rotation = enemyPosition.rotation;

        // Set Camera
        playerRenderCamera.transform.position = player.transform.GetChild(1).position - new Vector3(0, 0.5f, 2);
        enemyRenderCamera.transform.position = enemy.transform.GetChild(1).position - new Vector3(0, 0.5f, 2);

        playerName.text = player.GetComponent<Player>().Name;
        enemyName.text = enemy.GetComponent<Enemy>().Name;

        deleteScene = SceneManager.UnloadSceneAsync("FieldScene");
        StartCoroutine(LoadBattleScene());
    }


    public static IEnumerator LoadScene(GameObject enemyObject)
    {
        enemy = enemyObject;

        AsyncOperation loadScene = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        yield return new WaitUntil(() => loadScene.isDone);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadingScene"));
    }

    public IEnumerator LoadBattleScene()
    {
        yield return new WaitUntil(() => deleteScene.isDone);
        yield return new WaitForSeconds(2);
        blackScreen.SetActive(true);
        blackScreen.GetComponent<BlackScreen>().Blacken();
        yield return new WaitForSeconds(1);

        AsyncOperation loadScene = SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);

        loadScene.allowSceneActivation = false;

        yield return new WaitUntil(() => loadScene.progress >= 0.9f);
        loadScene.allowSceneActivation = true;
        yield return new WaitUntil(() => loadScene.isDone);

        enemy.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<PlayerGravity>().enabled = true;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("BattleScene"));
        SceneManager.UnloadSceneAsync("LoadingScene");

        BattleManager.Instance.Init(player, enemy);

        yield break;
    }
}