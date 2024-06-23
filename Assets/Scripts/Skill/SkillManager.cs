using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SkillManager : MonoBehaviour
{

    private static SkillManager instance;
    private GameObject animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static SkillManager Instance { get { return instance; } }

    public List<Skill> skills;
    public int SkillAmount { get { return skills.Count - 1; } }


    private void Init()
    {
        UnityEngine.Object[] tempInfos = Resources.LoadAll("Skills");

        foreach (Skill skill in tempInfos)
        {
            skills.Add(skill);
        }
    }

    /// <summary>
    /// Get Skill with Skill Number
    /// </summary>
    /// <param name="skillNum">Number of the Skill</param>
    /// <returns></returns>
    public Skill GetSkill(int skillNum)
    {
        if (skillNum < skills.Count)
            return skills[skillNum].Clone() as Skill;
        else return null;
    }
    public Skill[] GetSkill(List<int> indexes)
    {
        Skill[] returnSkills = new Skill[4];
        int[] skillNums = new int[4];
        int i = 0;
        int randIndex;

        while(i < 4 && indexes.Count > 0)
        {
            randIndex = UnityEngine.Random.Range(0, indexes.Count);
            skillNums[i++] = indexes[randIndex];
            indexes.RemoveAt(randIndex);
        }

        i = 0;
        foreach (int index in skillNums)
        {
            if(index == 0) returnSkills[i++] = null;
            else returnSkills[i++] = skills[index].Clone() as Skill;
        }

        return returnSkills;
    }
    public PlayableDirector GetSkillAnimDirector(GameObject skillAnim, GameObject attacker, GameObject defender, CinemachineBrain camera)
    {
        animator = Instantiate(skillAnim, Vector3.zero, Quaternion.identity);

        var animScript = animator.GetComponent<SkillAnimation>();
        animScript.attacker = attacker;
        animScript.defender = defender;

        var director = animator.GetComponent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;

        foreach (var track in timeline.GetOutputTracks())
        {
            switch (track.name)
            {
                case "Attacker":
                    director.SetGenericBinding(track, attacker);
                    break;

                case "Defender":
                    director.SetGenericBinding(track, defender);
                    break;

                case "Camera":
                    director.SetGenericBinding(track, camera);
                    break;

                default:
                    break;
            }
        }

        director.Play();
        return director;
    }
}
