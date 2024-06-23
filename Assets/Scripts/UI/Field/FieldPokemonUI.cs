using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class FieldPokemonUI : MonoBehaviour
{
    private Type type;

    public GameObject skillButton;
    public GameObject skillInfo;
    public GameObject pokemonButton;
    public GameObject arrow;

    public GameObject menuOptions;

    public Transform whiteBackground;
    public Transform redBackground;

    public List<GameObject> pokemonBtns;
    private List<GameObject> skillBtns;
    private List<GameObject> playerPokemons;
    private Skill[] skills;

    public Text titlePokemonName;
    public Text titlePokemonLevel;

    public SVGImage[] types;

    public Text pokemonNumber;
    public Text pokemonName;
    public Text hp;
    public Text attack;
    public Text defense;
    public Text speed;

    public List<Sprite> statusEffects;

    private PlayerMovement playerMovement;

    private enum PokemonButtonChild
    {
        NAME = 1,
        LEVEL = 2,
        STATUSEFFECT = 3,
        HPBAR = 4,
        RENDERTEXTURE = 5
    }


    // Start is called before the first frame update
    void Start()
    {
        pokemonBtns = new List<GameObject>();
        skillBtns = new List<GameObject>();
        type = Type.Instance;

        for (int i = 0; i < 6; i++)
        {
            pokemonBtns.Add(Instantiate(pokemonButton, redBackground));
            pokemonBtns[i].SetActive(false);

            var btnTransform = pokemonBtns[i].GetComponent<RectTransform>();

            btnTransform.offsetMin = new Vector2(btnTransform.offsetMin.x, 880 - (i * 133.33f));
            btnTransform.offsetMax = new Vector2(btnTransform.offsetMax.x, -(80 + (i * 133.33f)));

            int temp = i;
            pokemonBtns[i].GetComponent<Button>().onClick.AddListener(() => OnPokemonButtonClick(temp));
        }

        for (int i = 0; i < 4; i++)
        {
            skillBtns.Add(Instantiate(skillButton, whiteBackground));
            skillBtns[i].SetActive(false);

            int x = i % 2 == 0 ? -800 : -300;
            int y = i < 2 ? 300 : 200;

            skillBtns[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            int temp = i;
            skillBtns[i].GetComponentInChildren<Button>().onClick.AddListener(() => DisplaySkillInfo(temp));
        }
    }

    /// <summary>
    /// Should be called when Field Started
    /// </summary>
    /// <param name="playerPokemons"></param>
    public void Init(List<GameObject> playerPokemons, PlayerMovement playerMovement)
    {
        this.playerPokemons = playerPokemons;
        this.playerMovement = playerMovement;

        // Pokemon Buttons Init
        for (int i = 0; i < playerPokemons.Count; i++)
        {
            var btn = pokemonBtns[i];
            var pokemon = playerPokemons[i].GetComponent<Pokemon>();

            PokemonButtonUpdate(btn, pokemon);

            btn.transform.GetChild((int)PokemonButtonChild.NAME).GetComponent<Text>().text = pokemon.Name;
            btn.transform.GetChild((int)PokemonButtonChild.RENDERTEXTURE).GetComponent<RawImage>().texture = pokemon.PokemonTexture;
            btn.SetActive(true);
        }
    }

    /// <summary>
    /// Should be called when pokemon button clicked
    /// </summary>
    public void PokemonButtonUpdate()
    {
        for (int i = 0; i < playerPokemons.Count; i++)
        {
            playerPokemons[i].SetActive(true);
            PokemonButtonUpdate(pokemonBtns[i], playerPokemons[i].GetComponent<Pokemon>());
        }

        arrow.GetComponent<RectTransform>().offsetMin = new Vector2(arrow.GetComponent<RectTransform>().offsetMin.x, pokemonBtns[0].GetComponent<RectTransform>().offsetMin.y);
        arrow.GetComponent<RectTransform>().offsetMax = new Vector2(arrow.GetComponent<RectTransform>().offsetMax.x, pokemonBtns[0].GetComponent<RectTransform>().offsetMax.y);

        PokemonInfoUpdate(playerPokemons[0].GetComponent<Pokemon>());
        SkillButtonUpdate(playerPokemons[0].GetComponent<Pokemon>());
        DisplaySkillInfo(0);
    }

    private void PokemonButtonUpdate(GameObject btn, Pokemon pokemon)
    {
        btn.transform.GetChild((int)PokemonButtonChild.LEVEL).GetComponent<Text>().text = "Lv." + pokemon.Level;
        btn.transform.GetChild((int)PokemonButtonChild.STATUSEFFECT).GetComponent<Image>().sprite = statusEffects[(int)pokemon.StatusEffect];

        float pokemonHP = (float)pokemon.Cur_HP / pokemon.Max_HP;
        btn.transform.GetChild((int)PokemonButtonChild.HPBAR).GetComponent<Slider>().value = pokemonHP;

        var hpBarColor = btn.transform.GetChild((int)PokemonButtonChild.HPBAR).GetChild(1).GetComponent<Image>();
        if (pokemonHP <= 0.2f) hpBarColor.color = new Color(1, 0.294f, 0.294f);
        else if (pokemonHP <= 0.5f) hpBarColor.color = new Color(1, 0.745f, 0);
        else hpBarColor.color = new Color(0, 0.863f, 0);
    }

    private void DisplaySkillInfo(int index)
    {
        skillInfo.transform.GetChild(0).GetComponent<Text>().text = skills[index].SkillName;
        skillInfo.transform.GetChild(2).GetComponent<Text>().text = "위력 : " + skills[index].Power;
        skillInfo.transform.GetChild(3).GetComponent<Text>().text = "명중률 : " + skills[index].Rate;
        skillInfo.transform.GetChild(4).GetComponent<Text>().text = skills[index].SkillText;

        if (skills[index].Category == SKILL_CATEGORY.ATTACK) skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 공격";
        else skillInfo.transform.GetChild(1).GetComponent<Text>().text = "분류 : 특수";
    }

    private void PokemonInfoUpdate(Pokemon pokemon)
    {
        titlePokemonName.text = pokemon.Name;
        titlePokemonLevel.text = "Lv." + pokemon.Level;
        pokemonNumber.text = "No." + pokemon.Number;
        pokemonName.text = pokemon.Name;
        hp.text = pokemon.Cur_HP + "/" + pokemon.Max_HP;
        attack.text = pokemon.Attack.ToString();
        defense.text = pokemon.Defense.ToString();
        speed.text = pokemon.Speed.ToString();

        for (int i = 0; i < 2; i++)
        {
            if (pokemon.Types[i] == TYPE.NONE) break;
            types[i].sprite = type.GetSprite(pokemon.Types[i]);
        }
    }

    private void SkillButtonUpdate(Pokemon pokemon)
    {
        for (int i = 0; i < 4; i++) skillBtns[i].SetActive(false);

        skills = pokemon.Skills;

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i] == null) break;
            skillBtns[i].GetComponentInChildren<Text>().text = skills[i].SkillName;
            skillBtns[i].GetComponentInChildren<SVGImage>().sprite = type.GetSprite(skills[i].Type);
            skillBtns[i].transform.GetChild(1).GetComponent<Image>().color = type.ToColor(skills[i].Type);

            var pp = skillBtns[i].transform.GetChild(1).GetChild(2).GetComponentInChildren<Text>();
            pp.text = skills[i].CurPP + " / " + skills[i].MaxPP;

            skillBtns[i].SetActive(true);
        }
    }

    private void OnPokemonButtonClick(int selectedPokemon)
    {
        arrow.GetComponent<RectTransform>().offsetMin = new Vector2(arrow.GetComponent<RectTransform>().offsetMin.x, pokemonBtns[selectedPokemon].GetComponent<RectTransform>().offsetMin.y);
        arrow.GetComponent<RectTransform>().offsetMax = new Vector2(arrow.GetComponent<RectTransform>().offsetMax.x, pokemonBtns[selectedPokemon].GetComponent<RectTransform>().offsetMax.y);

        PokemonInfoUpdate(playerPokemons[selectedPokemon].GetComponent<Pokemon>());
        SkillButtonUpdate(playerPokemons[selectedPokemon].GetComponent<Pokemon>());
        DisplaySkillInfo(0);
    }

    public void ReturnButton()
    {
        foreach (var pokemon in playerPokemons) pokemon.SetActive(false);

        GetComponent<Canvas>().enabled = false;
        menuOptions.SetActive(true);
        playerMovement.enabled = true;
    }
}
