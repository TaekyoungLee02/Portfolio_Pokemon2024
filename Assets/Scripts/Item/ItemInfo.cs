using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Info")]
public class ItemInfo : ScriptableObject
{
    public int number;
    public string itemName;
    public string description;

    public ItemType type;
    public int healAmount;
    public STATUSEFFECT healingSE;
    public int catchRate;
    public int skillNum;

    public bool isBattleUse;

    public Sprite itemIcon;
}

public enum ItemType
{
    HEAL_HP,
    HEAL_STATUSEFFECT,
    HEAL_ALL,
    RESURRECT,
    MONSTERBALL,
    SKILL
}
