using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimation : MonoBehaviour
{
    public GameObject attacker;
    public GameObject defender;

    public void OnAttackerAttack()
    {
        attacker.GetComponent<Animator>().SetTrigger("Attack");
    }

    public void OnDefenderHit()
    {
        defender.GetComponent<Animator>().SetTrigger("Hit");
    }
}
