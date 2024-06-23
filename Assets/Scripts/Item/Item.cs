using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
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

    public int amount;

    public Item(ItemInfo info, int amount)
    {
        this.number = info.number;
        this.itemName = info.itemName;
        this.description = info.description;

        this.type = info.type;
        this.healAmount = info.healAmount;
        this.healingSE = info.healingSE;
        this.catchRate = info.catchRate;
        this.skillNum = info.skillNum;

        this.isBattleUse = info.isBattleUse;

        this.itemIcon = info.itemIcon;

        this.amount = amount;
    }


    /// <summary>
    /// Use Item to Pokemon
    /// </summary>
    /// <param name="itemNum"></param>
    /// <param name="pokemon"></param>
    /// <param name="deletingSkill">Please give value when item is Skill</param>
    public void Use(Pokemon pokemon, int deletingSkill = -1)
    {
        switch (type)
        {
            case ItemType.HEAL_HP:

                if (healAmount == -1)
                    pokemon.Cur_HP = pokemon.Max_HP;
                else
                    pokemon.Cur_HP += healAmount;

                if (pokemon.Cur_HP > pokemon.Max_HP) pokemon.Cur_HP = pokemon.Max_HP;

                break;

            case ItemType.HEAL_STATUSEFFECT:

                pokemon.StatusEffect = STATUSEFFECT.NONE;

                break;

            case ItemType.HEAL_ALL:

                if (healAmount == -1)
                    pokemon.Cur_HP = pokemon.Max_HP;
                else
                    pokemon.Cur_HP += healAmount;

                if (pokemon.Cur_HP > pokemon.Max_HP) pokemon.Cur_HP = pokemon.Max_HP;

                pokemon.StatusEffect = STATUSEFFECT.NONE;

                break;

            case ItemType.RESURRECT:

                pokemon.Defeat = false;
                pokemon.Cur_HP = pokemon.Max_HP;

                break;

            case ItemType.SKILL:

                pokemon.Skills[deletingSkill] = SkillManager.Instance.GetSkill(skillNum);

                break;
        }
    }

    public bool CheckItemUseCondition(Pokemon pokemon)
    {
        switch (type)
        {
            case ItemType.HEAL_HP:
                if (pokemon.Cur_HP == pokemon.Max_HP || pokemon.Defeat) return false;
                else return true;

            case ItemType.HEAL_STATUSEFFECT:
                if (pokemon.StatusEffect == STATUSEFFECT.NONE || pokemon.Defeat) return false;
                else return true;

            case ItemType.HEAL_ALL:
                if ((pokemon.Cur_HP == pokemon.Max_HP && pokemon.StatusEffect == STATUSEFFECT.NONE) || pokemon.Defeat) return false;
                else return true;

            case ItemType.RESURRECT:
                if (pokemon.Defeat) return true;
                else return false;


            case ItemType.SKILL:
                return true;

            default:
                return false;
        }
    }
}
