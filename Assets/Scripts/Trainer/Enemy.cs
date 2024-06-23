using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Trainer
{
    public bool isDefeted = false;
    public string defeatComment;
    public List<string> encounterComment;

    public int enemyNum;

    public override void Init(TrainerInfo trainerInfo)
    {
        base.Init(trainerInfo);

        this.defeatComment = trainerInfo.defeatComment;
        this.encounterComment = trainerInfo.encounterComment;
        this.enemyNum = trainerInfo.enemyNum;
    }

    public int GetNextPoke()
    {
        for (int i = 0; i < TrainerPokemons.Count; i++)
        {
            if (!TrainerPokemons[i].GetComponent<Pokemon>().Defeat) return i;
        }
        return -1;
    }

    public Command GetEnemyCommand()
    {
        Pokemon pokemon = BattleProcessor.Instance.GetEnemyPokemon();

        int skillAmount = 0;
        for (int i = 0; i < 4; i++)
        {
            if (pokemon.Skills[i] != null) skillAmount++;
            else break;
        }

        int rand = Random.Range(0, skillAmount);

        return new Command { type = Command.TYPE.SKILL, number = rand };
    }
}
