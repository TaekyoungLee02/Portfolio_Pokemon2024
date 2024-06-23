using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Trainer Info")]
public class TrainerInfo : ScriptableObject
{
    public string trainerName;
    public string defeatComment;
    public int enemyNum;
    public List<string> encounterComment;
    public List<TrainerPokemonInfo> trainerPokemonInfos;
    public List<TrainerBagInfo> trainerBagInfos;
    public GameObject TrainerGameObject;
}

[Serializable]
public struct TrainerPokemonInfo
{
    public int pokemonNum;
    public int level; 
}

[Serializable]
public struct TrainerBagInfo
{
    public int itemNum;
    public int amount;
}
