using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACTION
{
    SET_CAMERA,
    
    POKEMON_ENTRANCE,
    POKEMON_UISET,
    POKEMON_EXIT,
    POKEMON_LEVELUP,
    POKEMON_CHANGE,
    POKEMON_SKILLSET,
    POKEMON_HP_UPDATE,
    POKEMON_SE_UPDATE,
    POKEMON_EXP_UPDATE,
    POKEMON_STAT_NOTICE,
    POKEMON_LEVELUP_NOTICE,
    POKEMON_SKILL_LEARN,
    POKEMON_SKILL_TRYTOLEARN,

    SKILL_PP_UPDATE,
    SKILL_ANIM,
    ITEM_ANIM,

    RANKUP_ANIM,
    RANKDOWN_ANIM,
    HEAL_ANIM,
    STATUS_EFFECT_ANIM,

    CANT_MOVE_NEXT_TURN,

    BATTLE_TEXT,

    RUN,

    CATCH,
    CATCH_ANIMATION,

    BATTLE_END,
    PLAYER_WIN,
    ENEMY_WIN,
    TIE
}

public class BattleAction
{
    public ACTION type;

    public string battleText;
    public bool textSelect;

    public TARGET target;

    public ACTION winner;

    public Pokemon pokemon;
    public Pokemon defender;
    public Skill skill;

    public STATUSEFFECT statusEffect;

    public float waitSec;

    public CAMERA camera;

    public GameObject exitPokemon;
    public GameObject entrancePokemon;

    public int curHP;
    public int maxHP;
    public int atk;
    public int def;
    public int spd;
    public int exp;
    public int maxExp;
    public int level;

    public List<int> statDifference;

    public Skill learningSkill;

    public bool catched;
    public int tick;
}

public enum EFFECTIVE
{
     NONE,
     ZERO_EFFECTIVE,
     NOT_EFFECTIVE,
     SUPER_EFFECTIVE
}

public enum CAMERA
{
    PLAYER_CLOSEUP,
    ENEMY_CLOSEUP,
    WILD_CLOSEUP
}