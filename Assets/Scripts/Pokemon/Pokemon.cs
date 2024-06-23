using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    private int number;
    private string pokemonName;
    private string description;

    private int level;
    private int exp;
    public const int MAX_EXP = 1000000;
    
    private int base_HP;
    private int base_ATK;
    private int base_DEF;
    private int base_SPD;
    
    private int max_HP;
    private int cur_HP;

    private int attack;
    private int defense;
    private int speed;

    private int atk_Rank;
    private int def_Rank;
    private int spd_Rank;

    private TYPE[] types;
    private Skill[] skills;
    private List<LevelUpSkill> levelUpSkills;
    private STATUSEFFECT statusEffect;
    private Item item;

    private bool defeat;
    private RenderTexture pokemonTexture;

    public bool isBattle;

    public int Number { get { return number; } }
    public string Name { get { return pokemonName; } }
    public string Description { get { return description; } }
    public int Level { get { return level; } }
    public int Exp { get { return exp; } set { exp = value; } }
    public int Max_HP { get { return max_HP; } }
    public int Cur_HP { get { return cur_HP; } set { cur_HP = value; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int Speed { get { return speed; } }
    public int Atk_Rank { get { return atk_Rank; } set { atk_Rank = value; } }
    public int Def_Rank { get { return def_Rank; } set { def_Rank = value; } }
    public int Spd_Rank { get { return spd_Rank; } set { spd_Rank = value; } }
    public TYPE[] Types { get { return types; } }
    public Skill[] Skills { get { return skills; } }
    public List<LevelUpSkill> LevelUpSkills { get { return levelUpSkills; } }
    public STATUSEFFECT StatusEffect { get { return statusEffect; } set { statusEffect = value; } }
    public Item Item { get { return item; } }
    public RenderTexture PokemonTexture { get { return pokemonTexture; } set { pokemonTexture = value; } }
    public bool Defeat { get { return defeat; } set { defeat = value; } }

    public void Init(PokemonInfo pokemonInfo, int level)
    {
        this.number = pokemonInfo.Number;

        this.pokemonName = pokemonInfo.Name;
        this.name = Name;

        this.description = pokemonInfo.Description;
        this.base_HP = pokemonInfo.Base_HP;
        this.base_ATK = pokemonInfo.Base_ATK;
        this.base_DEF = pokemonInfo.Base_DEF;
        this.base_SPD = pokemonInfo.Base_SPD;

        this.types = pokemonInfo.Types;

        this.level = level;
        this.defeat = false;

        calculateStat();

        cur_HP = max_HP;
        atk_Rank = 0;
        def_Rank = 0;
        spd_Rank = 0;

        statusEffect = STATUSEFFECT.NONE;
        item = null;

        levelUpSkills = pokemonInfo.LevelSkills;

        SetSkill();
    }

    protected void calculateStat()
    {
        CalcMaxHP();
        CalcOtherStat(base_ATK, out attack);
        CalcOtherStat(base_DEF, out defense);
        CalcOtherStat(base_SPD, out speed);
    }

    private void CalcMaxHP()
    {
        max_HP = (int)(((base_HP * 2) + 150) * (double)level / 100 + 10);
    }
    private void CalcOtherStat(int baseStat, out int defStat)
    {
        defStat = (int)(((baseStat * 2) + 50) * (double)level / 100 + 5);
    }

    private void SetSkill()
    {
        List<int> availableSkills = new List<int>();

        foreach (var skill in levelUpSkills)
        {
            if (skill.level > this.level) continue;

            availableSkills.Add(skill.skillNum);
        }

        skills = SkillManager.Instance.GetSkill(availableSkills);
    }
    public bool SetSkill(Skill skill)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null)
            {
                skills[i] = skill;
                return true;
            }
        }
        return false;
    }
    public void SetSkill(Skill skill, int deletingSkill)
    {
        skills[deletingSkill] = skill;
    }

    public void DebugSkill()
    {
        foreach(var skill in skills)
        { 
            Debug.Log(skill.SkillName);
        }
    }

    public int CurrentTotalExp()
    {
        return (int)Math.Pow(level, 3) + exp;
    }

    public int RequiredExpUntilNextLevel()
    {
        return (level * 3) * (level + 1) + 1;
    }

    public List<int> LevelUp()
    {
        level++;

        List<int> statDiff = new List<int>();

        int prev = max_HP;
        CalcMaxHP();
        statDiff.Add(max_HP - prev);
        cur_HP += max_HP - prev;

        prev = attack;
        CalcOtherStat(base_ATK, out attack);
        statDiff.Add(attack - prev);


        prev = defense;
        CalcOtherStat(base_DEF, out defense);
        statDiff.Add(defense - prev);

        prev = speed;
        CalcOtherStat(base_SPD, out speed);
        statDiff.Add(speed - prev);

        return statDiff;
    }

    public int LearningSkillFromLevel()
    {
        foreach(var skill in levelUpSkills)
        {
            if(skill.level == level)
                return skill.skillNum;
        }

        return 0;
    }

    public void OnEnable()
    {
        if (isBattle) GetComponent<Animator>().SetBool("isBattle", true);
        else GetComponent<Animator>().SetBool("isBattle", false);
    }

    public void Rank_Refresh()
    {
        atk_Rank = 0;
        def_Rank = 0;
        spd_Rank = 0;
    }
}
