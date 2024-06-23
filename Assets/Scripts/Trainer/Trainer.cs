using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class Trainer : MonoBehaviour
{
    [SerializeField] protected string trainerName;

    protected Animator playerAnimator;

    public string Name { get { return trainerName; } }

    public List<GameObject> TrainerPokemons { get; protected set; }
    public List<TrainerPokemonInfo> TrainerPokemonInfos { get; set; }
    public List<Item> Bag { get; set; }

    private int encounterID;
    private int skillUseID;
    private int defeatID;
    private int battleStartID;

    public virtual void Init(TrainerInfo trainerInfo)
    {
        trainerName = trainerInfo.trainerName;
        TrainerPokemonInfos = trainerInfo.trainerPokemonInfos;
        TrainerPokemons = new List<GameObject>();
        Bag = new List<Item>();
        playerAnimator = gameObject.GetComponent<Animator>();

        AnimatorParameterInit();
    }

    public void AnimatorParameterInit()
    {
        encounterID = Animator.StringToHash("Encounter");
        skillUseID = Animator.StringToHash("SkillUse");
        defeatID = Animator.StringToHash("Defeat");
        battleStartID = Animator.StringToHash("BattleStart");
    }

    public void EncounterAction()
    {
        playerAnimator.SetTrigger(encounterID);
    }

    public void SkillUseAction()
    {
        playerAnimator.SetTrigger(skillUseID);
    }

    public void DefeatAction()
    {
        playerAnimator.SetTrigger(defeatID);
    }

    public void BattleStart()
    {
        playerAnimator.SetTrigger(battleStartID);
    }
}
