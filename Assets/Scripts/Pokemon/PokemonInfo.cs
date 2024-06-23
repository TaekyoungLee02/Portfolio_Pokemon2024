using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelUpSkill
{
    public int level;
    public int skillNum;
}

[CreateAssetMenu(fileName = "", menuName = "Pokemon Info")]
public class PokemonInfo : ScriptableObject
{
    [SerializeField] private int number;
    [SerializeField] private string pokemonName;
    [SerializeField] private string description;

    [SerializeField] private GameObject pokemonBody;

    [SerializeField] private int base_HP;
    [SerializeField] private int base_ATK;
    [SerializeField] int base_DEF;
    [SerializeField] int base_SPD;

    [SerializeField] private TYPE[] types;

    [SerializeField] private List<LevelUpSkill> levelSkills;

    public int Number { get { return number; } }
    public string Name { get { return pokemonName; } }
    public string Description { get { return description; } }
    public GameObject PokemonBody { get { return pokemonBody; } }
    public int Base_HP { get { return base_HP; } }
    public int Base_ATK { get { return base_ATK; } }
    public int Base_DEF { get { return base_DEF; } }
    public int Base_SPD { get { return base_SPD; } }
    public TYPE[] Types { get { return types; } }
    public List<LevelUpSkill> LevelSkills { get { return levelSkills; } }
}
