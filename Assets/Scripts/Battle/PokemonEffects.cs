using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonEffects : MonoBehaviour
{
    public enum TYPE
    {
        POISON,
        BURN,
        PARALYSIS,
        SLEEP,
        FREEZE,
        BUFF,
        DEBUFF,
        HEAL
    }

    public List<ParticleSystem> playerEffects;
    public List<ParticleSystem> enemyEffects;

    public ParticleSystem PlayEffects(TYPE type, TARGET target)
    {
        if (target == TARGET.ME)
        {
            playerEffects[(int)type].Play();
            return playerEffects[(int)type];
        }
        else
        {
            enemyEffects[(int)type].Play();
            return enemyEffects[(int)type];
        }
    }
}
