using System;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class SkillLearnUI : MonoBehaviour
{
    private Type type;
    private BattleManager battleManager;

    public GameObject selectButton;
    public GameObject skillButton;
    public GameObject returnButton;
    public GameObject skillInfo;
    public Transform redBackground;

    private List<GameObject> skillBtns;
    private List<Text> ppTexts;
    private Skill[] skills;
    private Skill learningSkill;

    public Text titlePokemonName;
    public Text titlePokemonLevel;

    public SVGImage[] types;

    public Text pokemonNumber;
    public Text pokemonName;
    public Text hp;
    public Text attack;
    public Text defense;
    public Text speed;

    void Start()
    {
        skillBtns = new List<GameObject>();
        battleManager = BattleManager.Instance;
        ppTexts = new List<Text>();
        type = Type.Instance;

        for (int i = 0; i < 5; i++)
        {
            skillBtns.Add(Instantiate(skillButton, redBackground));
            skillBtns[i].SetActive(false);
            skillBtns[i].GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            skillBtns[i].GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);

            int temp = i;
            if(i < 4)
                skillBtns[i].GetComponentInChildren<Button>().onClick.AddListener(() => DisplaySkillInfo(temp));
        }

        skillBtns[4].GetComponentInChildren<Button>().onClick.AddListener(() => LearningSkillInfo());
    }

    public void PokemonInfoUpdate(Pokemon pokemon)
    {
        titlePokemonName.text = pokemon.Name;
        titlePokemonLevel.text = "Lv." + pokemon.Level;
        pokemonNumber.text = "No." + pokemon.Number;
        pokemonName.text = pokemon.Name;
        hp.text = pokemon.Cur_HP + "/" + pokemon.Max_HP;
        attack.text = pokemon.Attack.ToString();
        defense.text = pokemon.Defense.ToString();
        speed.text = pokemon.Speed.ToString();

        for(int i = 0; i < 2; i ++)
        {
            if(pokemon.Types[i] == TYPE.NONE) break;
            types[i].sprite = type.GetSprite(pokemon.Types[i]);
        }
    }
    

    public void CreateSkillBtn(Skill[] skills, Skill learningSkill)
    {
        ppTexts.Clear();
        this.skills = skills;
        this.learningSkill = learningSkill;

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null) break;

            var btn = skillBtns[i];
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(i + 1) * 100);

            Skill skill = skills[i];

            btn.GetComponentInChildren<Text>().text = skill.SkillName;
            btn.GetComponentInChildren<SVGImage>().sprite = type.GetSprite(skill.Type);
            btn.transform.GetChild(1).GetComponent<Image>().color = type.ToColor(skill.Type);

            var pp = btn.transform.GetChild(1).GetChild(2).GetComponentInChildren<Text>();
            pp.text = skill.CurPP + " / " + skill.MaxPP;

            btn.SetActive(true);
            ppTexts.Add(pp);
        }

        skillInfo.transform.GetChild(0).GetComponent<Text>().text = skills[0].SkillName;
        skillInfo.transform.GetChild(2).GetComponent<Text>().text = "위력 : " + skills[0].Power;
        skillInfo.transform.GetChild(3).GetComponent<Text>().text = "명중률 : " + skills[0].Rate;
        skillInfo.transform.GetChild(4).GetComponent<Text>().text = skills[0].SkillText;
        if (skills[0].Category == SKILL_CATEGORY.ATTACK) skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 공격";
        else skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 특수";


        selectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(450, -100);


        skillBtns[4].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -500);
        skillBtns[4].GetComponentInChildren<Text>().text = learningSkill.SkillName;
        skillBtns[4].GetComponentInChildren<SVGImage>().sprite = type.GetSprite(learningSkill.Type);
        skillBtns[4].transform.GetChild(1).GetComponent<Image>().color = type.ToColor(learningSkill.Type);

        var learnpp = skillBtns[4].transform.GetChild(1).GetChild(2).GetComponentInChildren<Text>();
        learnpp.text = learningSkill.CurPP + " / " + learningSkill.MaxPP;
        skillBtns[4].SetActive(true);
    }

    public void DisplaySkillInfo(int index)
    {
        skillInfo.transform.GetChild(0).GetComponent<Text>().text = skills[index].SkillName;
        skillInfo.transform.GetChild(2).GetComponent<Text>().text = "위력 : " + skills[index].Power;
        skillInfo.transform.GetChild(3).GetComponent<Text>().text = "명중률 : " + skills[index].Rate;
        skillInfo.transform.GetChild(4).GetComponent<Text>().text = skills[index].SkillText;

        if (skills[index].Category == SKILL_CATEGORY.ATTACK) skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 공격";
        else skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 특수";

        selectButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(450, -(index + 1) * 100);
    }

    public void LearningSkillInfo()
    {
        skillInfo.transform.GetChild(0).GetComponent<Text>().text = learningSkill.SkillName;
        skillInfo.transform.GetChild(2).GetComponent<Text>().text = "위력 : " + learningSkill.Power;
        skillInfo.transform.GetChild(3).GetComponent<Text>().text = "명중률 : " + learningSkill.Rate;
        skillInfo.transform.GetChild(4).GetComponent<Text>().text = learningSkill.SkillText;

        if (learningSkill.Category == SKILL_CATEGORY.ATTACK) skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 공격";
        else skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 특수";
    }

    public void SelectButton()
    {
        for(int i = 0; i < skills.Length; i++)
        {
            if (selectButton.GetComponent<RectTransform>().anchoredPosition.y == skillBtns[i].GetComponent<RectTransform>().anchoredPosition.y)
            {
                battleManager.SelectLearningSkill(i);
                break;
            }
        }
    }

    public void ReturnButton()
    {
        battleManager.SelectLearningSkill(4);
    }
}
