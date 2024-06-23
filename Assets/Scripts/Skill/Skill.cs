using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum SKILL_CATEGORY
{
    ATTACK,
    STATUS
}

[Serializable]
public struct SkillEffect
{
    public SKILL_EFFECT skillEffect;
    public TARGET effectTarget;
    public INVOKING_TIMING invokingTiming;
    public int effect_Value;
    public int effect_Rate;
}

public enum SKILL_EFFECT
{
    RANKCHANGE_ATTACK,
    RANKCHANGE_DEFENSE,
    RANKCHANGE_SPEED, // 2


    STATUSEFFECT_POISON,
    STATUSEFFECT_SUPERPOISON,
    STATUSEFFECT_BURN,
    STATUSEFFECT_PARALYSIS,
    STATUSEFFECT_SLEEP, // !0 == Fixed
    STATUSEFFECT_FREEZE, // 8

    REBOUND_DAMAGE,
    REBOUND_DEVIATE,
    REBOUND_TURN, // 11

    ONEHIT,
    DEFENSE,
    EXPLOSION,
    REQUIRED_TWO_TURNS,
    FIXED_DAMAGE,

    HEAL_DAMAGE,
    HEAL_WEATHER,
    HEAL_PURE,
}

public enum SPECIALRANK
{
    RETURN_TO_ZERO = 0,
    COPY_ENEMY = 13,
    INCREASED_STATUS_TO_ZERO = 14,

}

public enum STATUSEFFECT
{
    NONE,
    POISON,
    SUPERPOISON,
    BURN,
    PARALYSIS,
    SLEEP,
    FREEZE
}

public enum TWOTURN
{
    VISIBLE = 0,
    INVISIBLE = 1,
}

public enum TARGET
{
    ME,
    ENEMY
}

public enum INVOKING_TIMING
{
    PRETURN,
    POSTTURN
}

[CreateAssetMenu(fileName = "", menuName = "Skill")]
public class Skill : ScriptableObject, ICloneable
{
    [SerializeField] private SKILL_CATEGORY category;
    [SerializeField] private TYPE type;

    [SerializeField] private string skillName;
    [SerializeField] private string skillText;

    [SerializeField] private List<SkillEffect> skillEffects = new List<SkillEffect>();

    [SerializeField] private int number;
    [SerializeField] private int power;
    [SerializeField] private int rate;
    [SerializeField] private int maxPP;
    [SerializeField] private int curPP;

    [SerializeField] private int priority;
    [SerializeField] private bool contact;

    [SerializeField] private GameObject playerAnim;
    [SerializeField] private GameObject enemyAnim;


    public SKILL_CATEGORY Category { get { return category; } }
    public TYPE Type { get { return type; } }
    public string SkillName { get { return skillName; } }
    public string SkillText { get { return skillText; } }
    public List<SkillEffect> Effects { get { return skillEffects; } }

    public int Number { get { return number; } }
    public int Power { get { return power; } }
    public int Rate { get { return rate; } }
    public int MaxPP { get { return maxPP; } }
    public int CurPP { get { return curPP; } set { curPP = value; } }

    public int Priority { get { return priority; } }
    public bool Contact { get { return contact; } }
    public GameObject PlayerAnim { get {  return playerAnim; } }
    public GameObject EnemyAnim { get { return enemyAnim; } }

    public object Clone()
    {
        Skill skill = CreateInstance<Skill>();
        skill.category = category;
        skill.type = type;
        skill.skillName = skillName;
        skill.skillText = skillText;
        skill.skillEffects = skillEffects.ToList();
        skill.number = number;
        skill.power = power;
        skill.rate = rate;
        skill.maxPP = maxPP;
        skill.curPP = curPP;
        skill.priority = priority;
        skill.contact = contact;
        skill.playerAnim = playerAnim;
        skill.enemyAnim = enemyAnim;
        return skill;
    }
}