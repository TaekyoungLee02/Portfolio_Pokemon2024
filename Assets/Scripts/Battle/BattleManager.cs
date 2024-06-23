using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;

    private BattleProcessor processor;
    private PokemonManager pokemonManager;
    private SkillManager skillManager;
    private CameraManager cameraManager;
    private PokemonEffects effectManager;

    public GameObject mainCamera;

    public GameObject player;
    public GameObject enemy;
    public GameObject wild;
    public Transform playerPos;
    public Transform enemyPos;
    public Transform wildPos;
    private Transform playerBallPos;
    private Transform enemyBallPos;

    private PlayableDirector animDirector;

    // Opening Timelines
    public GameObject playerOpeningDirector;
    public GameObject enemyOpeningDirector;
    public GameObject playerDefeatDirector;
    public GameObject enemyDefeatDirector;

    public SkillUI skillUI;
    public GameObject commandSelect;
    public GameObject skillSelect;
    public GameObject skillButton;
    public GameObject returnButton;
    private Command playerCommand;
    private Command enemyCommand = new Command() { type = Command.TYPE.SKILL, number = 0 };
    private bool isCommandSelected = false;

    public GameObject hpDisplayCanvas;
    private HPDisplayManager hpDisplayManager;
    public GameObject levelUpNotice;
    public GameObject blackScreen;

    public GameObject virtualCameras;
    public GameObject playerSceneCamera;
    public Transform playerCloseup;
    public Transform enemyCloseup;

    public GameObject pokemonEffects;
    private ParticleSystem effect;


    Queue<BattleAction> actionQueue;

    public GameObject battleTextObject;
    public Text battleText;
    private bool battleTextSelect;

    public GameObject skillLearnCanvas;
    public GameObject pokemonChangeCanvas;
    public GameObject bagCanvas;
    private int delSkillIndex;

    private int nextPokeIndex;

    public bool isTrainerBattle;
    public PokemonCatch pokemonCatch;
    private bool isCatchSuccess;
    private int leavingPokemon;

    public static BattleManager Instance { get { return instance; } }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init(GameObject player, GameObject enemy)
    {
        this.player = player;
        this.enemy = enemy;

        player.transform.SetPositionAndRotation(playerPos.position, playerPos.rotation);
        enemy.transform.SetPositionAndRotation(enemyPos.position, enemyPos.rotation);

        playerBallPos = FindBallPosition(player.transform);
        enemyBallPos = FindBallPosition(enemy.transform);

        processor = BattleProcessor.Instance;
        pokemonManager = PokemonManager.Instance;
        skillManager = SkillManager.Instance;
        cameraManager = virtualCameras.GetComponent<CameraManager>();
        effectManager = pokemonEffects.GetComponent<PokemonEffects>();
        hpDisplayManager = hpDisplayCanvas.GetComponent<HPDisplayManager>();

        pokemonManager.CreateTrainerPokemons(enemy.GetComponent<Trainer>());

        var playerTrainer = player.GetComponent<Trainer>();

        pokemonChangeCanvas.GetComponent<PokemonChangeUI>().Init(playerTrainer.TrainerPokemons);
        bagCanvas.GetComponent<BagUI>().Init(playerTrainer.TrainerPokemons, playerTrainer.Bag);

        processor.Init(player.GetComponent<Trainer>(), enemy.GetComponent<Trainer>());

        isTrainerBattle = true;

        actionQueue = processor.BattleOpening();
        blackScreen.GetComponent<BlackScreen>().UnBlacken();
        StartCoroutine(BattleLoop(actionQueue));
    }

    public void InitWildBattle(GameObject player, GameObject wildPokemon)
    {
        this.player = player;
        this.enemy = new GameObject();
        this.wild = wildPokemon;

        player.transform.SetPositionAndRotation(playerPos.position, playerPos.rotation);
        wild.transform.SetPositionAndRotation(new Vector3(13, 7, -1), Quaternion.Euler(new Vector3(0, -70, 0)));
        playerBallPos = FindBallPosition(player.transform);
        wild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        processor = BattleProcessor.Instance;
        pokemonManager = PokemonManager.Instance;
        skillManager = SkillManager.Instance;
        cameraManager = virtualCameras.GetComponent<CameraManager>();
        effectManager = pokemonEffects.GetComponent<PokemonEffects>();
        hpDisplayManager = hpDisplayCanvas.GetComponent<HPDisplayManager>();

        var playerTrainer = player.GetComponent<Trainer>();

        foreach(var i in playerTrainer.Bag) Debug.Log(i.itemName + " : " + i.amount);
        pokemonChangeCanvas.GetComponent<PokemonChangeUI>().Init(playerTrainer.TrainerPokemons);
        bagCanvas.GetComponent<BagUI>().Init(playerTrainer.TrainerPokemons, playerTrainer.Bag);

        var enemyComp = enemy.AddComponent<Enemy>();
        enemyComp.Init(ScriptableObject.CreateInstance<TrainerInfo>());
        enemyComp.TrainerPokemons.Add(wild);

        processor.Init(player.GetComponent<Trainer>(), enemy.GetComponent<Trainer>());

        isTrainerBattle = false;
        pokemonCatch.Init(wild);

        actionQueue = processor.WildBattleOpening();
        blackScreen.GetComponent<BlackScreen>().UnBlacken();
        StartCoroutine(BattleLoop(actionQueue));
    }


    IEnumerator BattleLoop(Queue<BattleAction> actionQueue)
    {
        while (true)
        {
            if(actionQueue.Count == 0) yield break;

            player.transform.SetPositionAndRotation(playerPos.position, playerPos.rotation);

            Debug.Log(actionQueue.Peek().type);

            var action = actionQueue.Dequeue();

            BattleDisplay(action);

            switch(action.type)
            {
                case ACTION.POKEMON_ENTRANCE:
                    yield return new WaitUntil(() => animDirector.state == PlayState.Paused);
                    playerSceneCamera.SetActive(true);
                    yield return new WaitForSeconds(Time.deltaTime);
                    playerSceneCamera.SetActive(false);
                    break;
                
                case ACTION.POKEMON_EXIT:

                    if(action.target == TARGET.ENEMY && !isTrainerBattle) break;

                    yield return new WaitUntil(() => animDirector.state == PlayState.Paused);
                    animDirector.gameObject.GetComponent<PokemonDefeatTimeline>().TimelineEnd();

                    List<GameObject> pks;
                    int z = 0;

                    if(action.target == TARGET.ME)
                    {
                        pks = player.GetComponent<Trainer>().TrainerPokemons;
                    }
                    else
                    {
                        pks = enemy.GetComponent<Trainer>().TrainerPokemons;
                        z = 10;
                    }

                    for (int i = 0; i < pks.Count; i++)
                    {
                        if (action.exitPokemon.Equals(pks[i]))
                        {
                            action.exitPokemon.transform.position = new Vector3(i * 10, -10, z);
                            action.exitPokemon.transform.rotation = Quaternion.Euler(Vector3.zero);
                            action.exitPokemon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                            action.exitPokemon.SetActive(false);
                        }
                    }

                    playerSceneCamera.SetActive(true);
                    yield return new WaitForSeconds(Time.deltaTime);
                    playerSceneCamera.SetActive(false);
                    break;

                case ACTION.SKILL_ANIM:
                    yield return new WaitUntil(() => animDirector.state == PlayState.Paused);
                    Destroy(animDirector.gameObject);
                    playerSceneCamera.SetActive(true);
                    yield return new WaitForSeconds(Time.deltaTime);
                    playerSceneCamera.SetActive(false);

                    break;

                
                case ACTION.POKEMON_CHANGE:

                    if (action.catched)
                    {
                        if (battleTextSelect)
                        {
                            yield return new WaitUntil(() => isCommandSelected);
                            isCommandSelected = false;

                            if (leavingPokemon == -1) break;

                            else
                            {
                                var pokemons = player.GetComponent<Trainer>().TrainerPokemons;
                                var leavingPoke = pokemons[leavingPokemon].GetComponent<Pokemon>();
                                var leavingTempQueue = processor.LeavingPokemon(leavingPoke, wild.GetComponent<Pokemon>());

                                pokemons.Remove(leavingPoke.gameObject);
                                pokemons.Add(wild);
                                Destroy(leavingPoke.gameObject);

                                foreach (var act in actionQueue) leavingTempQueue.Enqueue(act);
                                actionQueue = leavingTempQueue;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (action.target == TARGET.ME)
                        {
                            yield return new WaitUntil(() => isCommandSelected);
                            isCommandSelected = false;

                            actionQueue = processor.SetNextPoke(TARGET.ME, nextPokeIndex);
                        }
                        else
                        {
                            actionQueue = processor.SetNextPoke(TARGET.ENEMY, enemy.GetComponent<Enemy>().GetNextPoke());
                        }
                    }

                    break;

                case ACTION.BATTLE_TEXT:

                    if(action.textSelect)
                    {
                        yield return new WaitUntil(() => isCommandSelected);
                        isCommandSelected = false;
                        battleTextObject.transform.GetChild(2).gameObject.SetActive(false);
                    }
                    else if (action.waitSec > 0)
                        yield return new WaitForSeconds(action.waitSec);
                    else
                    {
                        yield return new WaitForSeconds(0.5f);
                        yield return new WaitUntil(() => Input.anyKeyDown);
                        yield return new WaitForSeconds(Time.deltaTime);
                    }


                    break;

                case ACTION.POKEMON_EXP_UPDATE:
                case ACTION.POKEMON_HP_UPDATE:
                    yield return new WaitUntil(() => hpDisplayManager.valueChangeFinished);
                    break;

                case ACTION.POKEMON_SE_UPDATE:
                    yield return new WaitForSeconds(2);
                    break;

                case ACTION.POKEMON_SKILL_TRYTOLEARN:
                    var tempqueue = processor.SkillDelLearn(action.learningSkill, action.pokemon);

                    foreach(var act in actionQueue) tempqueue.Enqueue(act);
                    actionQueue = tempqueue;

                    break;
                case ACTION.POKEMON_SKILL_LEARN:

                    if(battleTextSelect)
                    {
                        battleTextObject.SetActive(false);
                        hpDisplayCanvas.SetActive(false);
                        skillLearnCanvas.GetComponent<SkillLearnUI>().CreateSkillBtn(action.pokemon.Skills, action.learningSkill);
                        skillLearnCanvas.GetComponent<SkillLearnUI>().PokemonInfoUpdate(action.pokemon);

                        skillLearnCanvas.GetComponent<Canvas>().enabled = true;


                        yield return new WaitUntil(() => isCommandSelected);
                        isCommandSelected = false;
                        skillLearnCanvas.GetComponent<Canvas>().enabled = false;

                        if(delSkillIndex == 4)
                        {
                            var nonlearnQueue = processor.SkillDelLearn(action.learningSkill.SkillName, action.pokemon.Name);
                            foreach (var act in actionQueue) nonlearnQueue.Enqueue(act);
                            actionQueue = nonlearnQueue;
                        }
                        else
                        {
                            var skilldelQueue = processor.SkillDelLearn(action.pokemon.Skills[delSkillIndex].SkillName, action.learningSkill.SkillName, action.pokemon);

                            action.pokemon.Skills[delSkillIndex] = action.learningSkill;

                            foreach (var act in actionQueue) skilldelQueue.Enqueue(act);
                            actionQueue = skilldelQueue;
                        }
                    }
                    else
                    {
                        var nonlearnQueue = processor.SkillDelLearn(action.learningSkill.SkillName, action.pokemon.Name);
                        foreach (var act in actionQueue) nonlearnQueue.Enqueue(act);
                        actionQueue = nonlearnQueue;
                    }

                    break;

                case ACTION.RANKUP_ANIM:
                case ACTION.RANKDOWN_ANIM:
                case ACTION.STATUS_EFFECT_ANIM:
                case ACTION.HEAL_ANIM:
                    yield return new WaitUntil(() => effect.isStopped);
                    playerSceneCamera.SetActive(true);
                    yield return new WaitForSeconds(Time.deltaTime);
                    playerSceneCamera.SetActive(false);
                    break;

                case ACTION.POKEMON_LEVELUP:
                    yield return new WaitForSeconds(1);
                    break;

                case ACTION.POKEMON_LEVELUP_NOTICE:
                    yield return new WaitForSeconds(2);
                    levelUpNotice.SetActive(false);
                    break;

                case ACTION.BATTLE_END:
                case ACTION.RUN:

                    processor.GetPlayerPokemon().Rank_Refresh();

                    player.GetComponent<Animator>().SetBool("isBattle", false);
                    player.GetComponent<Player>().EnablePlayerMovementWhenSceneLoaded();
                    yield return new WaitForSeconds(1);


                    blackScreen.SetActive(true);
                    battleTextObject.SetActive(false);
                    blackScreen.GetComponent<BlackScreen>().Blacken();
                    yield return new WaitForSeconds(2);


                    Destroy(enemy);
                    if(!isTrainerBattle) Destroy(wild);

                    pokemonManager.BattleEnd(player.GetComponent<Trainer>());
                    SceneManager.LoadScene("FieldScene");

                    break;

                case ACTION.CATCH:

                    processor.GetPlayerPokemon().Rank_Refresh();

                    player.GetComponent<Animator>().SetBool("isBattle", false);
                    player.GetComponent<Player>().EnablePlayerMovementWhenSceneLoaded();
                    yield return new WaitForSeconds(1);


                    blackScreen.SetActive(true);
                    battleTextObject.SetActive(false);
                    blackScreen.GetComponent<BlackScreen>().Blacken();
                    yield return new WaitForSeconds(2);

                    Destroy(enemy);

                    pokemonManager.BattleEnd(player.GetComponent<Trainer>());
                    SceneManager.LoadScene("FieldScene");

                    break;

                case ACTION.CATCH_ANIMATION:

                    yield return new WaitUntil(() => isCommandSelected);
                    isCommandSelected = false;

                    if(isCatchSuccess)
                    {
                        wild.transform.position = new Vector3((player.GetComponent<Player>().TrainerPokemons.Count - 1) * 10, -10, 0); //fix later
                        wild.transform.rotation = Quaternion.Euler(Vector3.zero);
                        wild.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        wild.SetActive(true);
                    }

                    break;

                default:
                    yield return null;
                    break;
            }


            if(actionQueue.Count == 0) 
            {
                processor.PokemonDebug();
                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(true);
                commandSelect.SetActive(true);


                cameraManager.StartCameraLoop();


                yield return new WaitUntil(() => isCommandSelected);
                isCommandSelected = false;

                Debug.Log(enemyCommand.type + ", " + enemyCommand.number);

                cameraManager.StopCameraLoop();

                enemyCommand = enemy.GetComponent<Enemy>().GetEnemyCommand();
                actionQueue = processor.GetBattleActionQueue(playerCommand, enemyCommand);

                Debug.Log("Queue returned");
            }
        }
    }

    public void SetPlayerCommand(Command command)
    {
        playerCommand = command;
        isCommandSelected = true;
    }
    public void SelectLearningSkill(int index)
    {
        delSkillIndex = index;
        isCommandSelected = true;
    }
    public void BattleTextSelect(bool select)
    {
        battleTextSelect = select;
        isCommandSelected = true;
    }
    public void SelectNextPokemon(int index)
    {
        nextPokeIndex = index;
        isCommandSelected = true;
    }
    public void CatchSuccessed(bool success)
    {
        isCatchSuccess = success;
        isCommandSelected = true;
    }
    public void SetLeavingPokemon(int index)
    {
        leavingPokemon = index;
        isCommandSelected = true;
    }


    private void BattleDisplay(BattleAction battleAction)
    {
        switch (battleAction.type)
        {
            case ACTION.SET_CAMERA:

                if (battleAction.camera == CAMERA.PLAYER_CLOSEUP)
                    mainCamera.transform.SetPositionAndRotation(playerCloseup.position, playerCloseup.rotation);
                else if (battleAction.camera == CAMERA.ENEMY_CLOSEUP)
                    mainCamera.transform.SetPositionAndRotation(enemyCloseup.position, enemyCloseup.rotation);
                else
                    cameraManager.enemyPokemonCloseup();

                break;

            case ACTION.POKEMON_ENTRANCE:


                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(false);

                if (battleAction.target == TARGET.ME)
                {
                    battleAction.entrancePokemon.SetActive(false);
                    battleAction.entrancePokemon.transform.position = new Vector3(-5, 7, 3);
                    battleAction.entrancePokemon.transform.rotation = Quaternion.Euler(new Vector3(0, 110, 0));
                    animDirector = playerOpeningDirector.GetComponent<PlayableDirector>();
                    playerOpeningDirector.GetComponent<BallThrowTimeline>().pokemon = battleAction.entrancePokemon;
                    playerOpeningDirector.GetComponent<BallThrowTimeline>().ballPosition = playerBallPos;
                    playerOpeningDirector.GetComponent<BallThrowTimeline>().Init(player);
                }
                else
                {
                    battleAction.entrancePokemon.SetActive(false);
                    battleAction.entrancePokemon.transform.position = new Vector3(13, 7, -1);
                    battleAction.entrancePokemon.transform.rotation = Quaternion.Euler(new Vector3(0, -70, 0));
                    animDirector = enemyOpeningDirector.GetComponent<PlayableDirector>();
                    enemyOpeningDirector.GetComponent<BallThrowTimeline>().pokemon = battleAction.entrancePokemon;
                    enemyOpeningDirector.GetComponent<BallThrowTimeline>().ballPosition = enemyBallPos;
                    enemyOpeningDirector.GetComponent<BallThrowTimeline>().Init(enemy);
                }

                animDirector.Play();


                break;

            case ACTION.POKEMON_EXIT:

                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(false);


                if (battleAction.target == TARGET.ME)
                {
                    animDirector = playerDefeatDirector.GetComponent<PlayableDirector>();
                    playerDefeatDirector.GetComponent<PokemonDefeatTimeline>().Init(battleAction.exitPokemon, player);
                    playerDefeatDirector.GetComponent<PokemonDefeatTimeline>().ballPosition = playerBallPos;
                    playerDefeatDirector.GetComponent<PokemonDefeatTimeline>().TimelineStart();
                }
                else
                {
                    if (!isTrainerBattle) break;

                    animDirector = enemyDefeatDirector.GetComponent<PlayableDirector>();
                    enemyDefeatDirector.GetComponent<PokemonDefeatTimeline>().Init(battleAction.exitPokemon, enemy);
                    enemyDefeatDirector.GetComponent<PokemonDefeatTimeline>().ballPosition = enemyBallPos;
                    enemyDefeatDirector.GetComponent<PokemonDefeatTimeline>().TimelineStart();
                }

                animDirector.Play();

                break;


            case ACTION.POKEMON_UISET:

                if(battleAction.target == TARGET.ME)
                {
                    hpDisplayManager.SetPlayerPokemonName(battleAction.pokemon.Name);
                    hpDisplayManager.SetPlayerPokemonLevel(battleAction.pokemon.Level);
                    hpDisplayManager.SetPlayerPokemonHP(battleAction.curHP, battleAction.maxHP);
                    hpDisplayManager.SetPlayerHPBar(battleAction.curHP, battleAction.maxHP);
                    hpDisplayManager.SetPlayerPokemonEXP(battleAction.pokemon.RequiredExpUntilNextLevel(), battleAction.pokemon.Exp);
                    hpDisplayManager.SetPlayerStatusEffect(battleAction.statusEffect);
                }
                else
                {
                    hpDisplayManager.SetEnemyPokemonName(battleAction.pokemon.Name);
                    hpDisplayManager.SetEnemyPokemonLevel(battleAction.pokemon.Level);
                    hpDisplayManager.SetEnemyHPBar(battleAction.curHP, battleAction.maxHP);
                    hpDisplayManager.SetEnemyStatusEffect(battleAction.statusEffect);
                }

                break;


            case ACTION.POKEMON_EXP_UPDATE:

                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(true);

                hpDisplayManager.PlayerExpGain(battleAction.exp, battleAction.maxExp);

                break;

            case ACTION.POKEMON_LEVELUP:

                hpDisplayManager.SetPlayerPokemonEXP(battleAction.maxExp);
                hpDisplayManager.SetPlayerPokemonLevel(battleAction.level);
                hpDisplayManager.SetPlayerPokemonHP(battleAction.curHP, battleAction.maxHP);
                hpDisplayManager.SetPlayerHPBar(battleAction.curHP, battleAction.maxHP);

                break;

            case ACTION.POKEMON_LEVELUP_NOTICE:

                levelUpNotice.GetComponent<LevelUpNotice>().LevelUp(battleAction.maxHP, battleAction.atk, battleAction.def, battleAction.spd, battleAction.statDifference);
                levelUpNotice.SetActive(true);

                break;

            case ACTION.POKEMON_SKILL_LEARN:

                break;

            case ACTION.POKEMON_CHANGE:

                if (battleAction.catched)
                {
                    if (battleTextSelect)
                    {
                        battleTextObject.SetActive(false);
                        hpDisplayCanvas.SetActive(false);
                        pokemonChangeCanvas.GetComponent<PokemonChangeUI>().isPokemonCatched = true;
                        pokemonChangeCanvas.GetComponent<PokemonChangeUI>().PokemonButtonUpdate();
                        pokemonChangeCanvas.GetComponent<Canvas>().enabled = true;
                        break;
                    }
                    else break;
                }

                if (battleAction.target == TARGET.ME)
                {
                    battleTextObject.SetActive(false);
                    hpDisplayCanvas.SetActive(false);
                    pokemonChangeCanvas.GetComponent<PokemonChangeUI>().isPokemonDefeted = true;
                    pokemonChangeCanvas.GetComponent<PokemonChangeUI>().PokemonButtonUpdate();
                    pokemonChangeCanvas.GetComponent<Canvas>().enabled = true;
                }

                break;

            case ACTION.POKEMON_SKILLSET:

                if (battleAction.target == TARGET.ENEMY) break;

                skillUI.SkillBtnInit();
                skillUI.CreateSkillBtn(battleAction.pokemon.Skills);

                break;

            case ACTION.SKILL_PP_UPDATE:

                if (battleAction.target == TARGET.ENEMY) break;

                skillUI.SkillPPUpdate(battleAction.pokemon.Skills);

                break;

            case ACTION.POKEMON_HP_UPDATE:

                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(true);

                if (battleAction.target == TARGET.ENEMY)
                {
                    hpDisplayManager.EnemyHPValueChange(battleAction.curHP, battleAction.maxHP);
                }
                else
                {
                    hpDisplayManager.PlayerHPValueChange(battleAction.curHP, battleAction.maxHP);
                }

                break;

            case ACTION.POKEMON_SE_UPDATE:

                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(true);

                if (battleAction.target == TARGET.ENEMY)
                {
                    hpDisplayManager.SetEnemyStatusEffect(battleAction.statusEffect);
                }
                else
                {
                    hpDisplayManager.SetPlayerStatusEffect(battleAction.statusEffect);
                }

                break;

            case ACTION.SKILL_ANIM:

                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(false);


                if (battleAction.target == TARGET.ME)
                {
                    player.GetComponent<Player>().SkillUseAction();
                    animDirector = skillManager.GetSkillAnimDirector(battleAction.skill.PlayerAnim, battleAction.pokemon.gameObject, battleAction.defender.gameObject, mainCamera.GetComponent<CinemachineBrain>());
                }
                else
                {
                    if (isTrainerBattle) enemy.GetComponent<Enemy>().SkillUseAction();
                    animDirector = skillManager.GetSkillAnimDirector(battleAction.skill.EnemyAnim, battleAction.pokemon.gameObject, battleAction.defender.gameObject, mainCamera.GetComponent<CinemachineBrain>());
                }
                animDirector.Play();


                break;

            case ACTION.ITEM_ANIM:

                battleTextObject.SetActive(false);
                hpDisplayCanvas.SetActive(false);

                break;

            case ACTION.RANKUP_ANIM:

                battleTextObject.SetActive(false);

                if (battleAction.target == TARGET.ME) cameraManager.playerPokemonCloseup();
                else cameraManager.enemyPokemonCloseup();

                effect = effectManager.PlayEffects(PokemonEffects.TYPE.BUFF, battleAction.target);

                break;

            case ACTION.RANKDOWN_ANIM:

                battleTextObject.SetActive(false);

                if (battleAction.target == TARGET.ME) cameraManager.playerPokemonCloseup();
                else cameraManager.enemyPokemonCloseup();

                effect = effectManager.PlayEffects(PokemonEffects.TYPE.DEBUFF, battleAction.target);

                break;

            case ACTION.STATUS_EFFECT_ANIM:

                battleTextObject.SetActive(false);

                if (battleAction.target == TARGET.ME) cameraManager.playerPokemonCloseup();
                else cameraManager.enemyPokemonCloseup();

                switch (battleAction.statusEffect)
                {
                    case STATUSEFFECT.POISON:
                    case STATUSEFFECT.SUPERPOISON:
                        effect = effectManager.PlayEffects(PokemonEffects.TYPE.POISON, battleAction.target);
                        break;

                    case STATUSEFFECT.BURN:
                        effect = effectManager.PlayEffects(PokemonEffects.TYPE.BURN, battleAction.target);
                        break;

                    case STATUSEFFECT.PARALYSIS:
                        effect = effectManager.PlayEffects(PokemonEffects.TYPE.PARALYSIS, battleAction.target);
                        break;

                    case STATUSEFFECT.SLEEP:
                        effect = effectManager.PlayEffects(PokemonEffects.TYPE.SLEEP, battleAction.target);
                        break;

                    case STATUSEFFECT.FREEZE:
                        effect = effectManager.PlayEffects(PokemonEffects.TYPE.FREEZE, battleAction.target);
                        break;
                }

                break;

            case ACTION.HEAL_ANIM:

                battleTextObject.SetActive(false);

                if (battleAction.target == TARGET.ME) cameraManager.playerPokemonCloseup();
                else cameraManager.enemyPokemonCloseup();

                effect = effectManager.PlayEffects(PokemonEffects.TYPE.HEAL, battleAction.target);

                break;

            case ACTION.BATTLE_TEXT:

                hpDisplayCanvas.SetActive(false);

                battleText.text = battleAction.battleText;

                if (battleTextObject.activeSelf != true) battleTextObject.SetActive(true); 
                if(battleAction.textSelect) battleTextObject.transform.GetChild(2).gameObject.SetActive(true);

                break;

            case ACTION.BATTLE_END:

                if(battleAction.winner == ACTION.PLAYER_WIN)
                {
                    if (!isTrainerBattle) break;

                    enemy.GetComponent<Enemy>().DefeatAction();
                    SceneUpdater.Instance.AddEnemyDefededInfo(new EnemyDefeted(enemy.GetComponent<Enemy>().enemyNum, true));
                }
                else
                {
                    Application.Quit();
                }

                break;

            case ACTION.CATCH_ANIMATION:

                pokemonCatch.Play(battleAction.tick, battleAction.catched);

                break;
        }
    }
    
    public Queue<BattleAction> getBattleActionQueue(Command a, Command b)
    {
        return processor.GetBattleActionQueue(a, b);

    }

    private Transform FindBallPosition(Transform trainer)
    {
        Transform pos = trainer.transform.Find("BallPosition");

        if(pos != null) return pos;
        else
        {
            foreach (Transform t in trainer.transform)
            {
                Transform recpos = FindBallPosition(t);
                if(recpos != null) return recpos;
            }
            return pos;
        }
    }
}