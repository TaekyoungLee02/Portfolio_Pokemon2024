using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private Type type;
    private BattleManager battleManager;

    public GameObject commandSelect;
    public GameObject skillSelect;
    public GameObject skillButton;
    public GameObject returnButton;
    public GameObject skillInfo;

    private List<GameObject> skillBtns;
    private List<Text> ppTexts;
    private Skill[] skills;

    private void Start()
    {
        skillBtns = new List<GameObject>();
        battleManager = BattleManager.Instance;
        ppTexts = new List<Text>();
        type = Type.Instance;

        for(int i = 0; i < 4; i++)
        {
            skillBtns.Add(Instantiate(skillButton, skillSelect.transform));
            skillBtns[i].SetActive(false);

            int temp = i;
            skillBtns[i].GetComponentInChildren<Button>().onClick.AddListener(() => SendSkillCommand(Command.TYPE.SKILL, temp));
        }
    }

    public void CreateSkillBtn(Skill[] skills)
    {
        //skillBtns.Clear();
        ppTexts.Clear();
        this.skills = skills;

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null) break;

            var btn = skillBtns[i];
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, (skills.Length - i) * 100);

            Skill skill = skills[i];

            btn.GetComponentInChildren<Text>().text = skill.SkillName;
            btn.GetComponentInChildren<SVGImage>().sprite = type.GetSprite(skill.Type);
            btn.transform.GetChild(1).GetComponent<Image>().color = type.ToColor(skill.Type);

            var pp = btn.transform.GetChild(1).GetChild(2).GetComponentInChildren<Text>();
            pp.text = skill.CurPP + " / " + skill.MaxPP;

            btn.SetActive(true);
            ppTexts.Add(pp);
        }

        returnButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, (skills.Length + 1) * 100);
    }

    public void SkillBtnInit()
    {
        for (int i = 0; i < 4; i++)
        {
            skillBtns[i].SetActive(false);
        }
    }

    public void SkillPPUpdate(Skill[] skills)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            ppTexts[i].text = skills[i].CurPP + " / " + skills[i].MaxPP;
        }
    }

    public void Fight()
    {
        skillSelect.SetActive(true);
        commandSelect.SetActive(false);
    }

    public void ReturnFromSkillSelect()
    {
        skillSelect.SetActive(false);
        commandSelect.SetActive(true);
        skillInfo.SetActive(false);
    }

    public void SendSkillCommand(Command.TYPE type, int number)
    {
        if(skillInfo.activeSelf == false || skillInfo.transform.GetChild(0).GetComponent<Text>().text != skills[number].SkillName)
        {
            skillInfo.SetActive(true);

            skillInfo.transform.GetChild(0).GetComponent<Text>().text = skills[number].SkillName;
            skillInfo.transform.GetChild(2).GetComponent<Text>().text = "위력 : " + skills[number].Power;
            skillInfo.transform.GetChild(3).GetComponent<Text>().text = "명중률 : " + skills[number].Rate;
            skillInfo.transform.GetChild(4).GetComponent<Text>().text = skills[number].SkillText;

            if (skills[number].Category == SKILL_CATEGORY.ATTACK) skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 공격";
            else skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 특수";

            return;
        }

        Command cmd = new()
        {
            type = type,
            number = number
        };

        Debug.Log(type + ", " + number);

        battleManager.SetPlayerCommand(cmd);

        skillInfo.SetActive(false);
        skillSelect.SetActive(false);
    }
}
