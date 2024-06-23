using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    public Transform redBackground;

    public GameObject pokemonButton;
    public GameObject itemButton;
    public GameObject bagUIContent;
    public Text itemDescription;

    public GameObject commandSelect;
    public GameObject hpDisplay;
    public GameObject battleText;
    public Text battleTextContent;

    private List<GameObject> pokemonBtns;
    private List<GameObject> playerPokemons;
    private List<GameObject> itemBtns;
    private List<Item> bag;

    public List<Sprite> statusEffects;

    private int selectedItem = -1;
    private bool isItemSelected;
    private bool isItemUsing = false;

    private enum PokemonButtonChild
    {
        NAME = 1,
        LEVEL = 2,
        STATUSEFFECT = 3,
        HPBAR = 4,
        RENDERTEXTURE = 5
    }

    // Update is called once per frame
    void Start()
    {
        pokemonBtns = new List<GameObject>();
        itemBtns = new List<GameObject>();

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

        for (int i = 0; i < 50; i++)
        {
            itemBtns.Add(Instantiate(itemButton, bagUIContent.transform));
            itemBtns[i].SetActive(false);


            int temp = i;
            itemBtns[i].GetComponentInChildren<Button>().onClick.AddListener(() => OnItemButtonClick(temp));
        }
    }


    /// <summary>
    /// Should be called when Battle Started
    /// </summary>
    /// <param name="playerPokemons"></param>
    public void Init(List<GameObject> playerPokemons, List<Item> bag)
    {
        this.playerPokemons = playerPokemons;
        this.bag = bag;

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
    /// Should be called when bag button clicked
    /// </summary>
    public void BagUIUPdate()
    {
        for (int i = 0; i < playerPokemons.Count; i++)
        {
            playerPokemons[i].SetActive(true);
            PokemonButtonUpdate(pokemonBtns[i], playerPokemons[i].GetComponent<Pokemon>());
        }

        BagUpdate();

        selectedItem = -1;
        isItemSelected = false;
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

    private void BagUpdate()
    {
        int contentSize = bag.Count * 100;
        var bagUItransform = bagUIContent.GetComponent<RectTransform>();

        bagUItransform.sizeDelta = new Vector2(bagUItransform.sizeDelta.x, contentSize);

        for (int i = 0; i < bag.Count; i++)
        {
            var item = bag[i];
            var infos = itemBtns[i].GetComponent<ItemButton>();

            infos.icon.sprite = item.itemIcon;
            infos.itemName.text = item.itemName;
            infos.itemAmount.text = item.amount.ToString();

            itemBtns[i].SetActive(true);
        }

        for (int i = bag.Count; i < itemBtns.Count; i++)
        {
            if (itemBtns[i].activeSelf == false) return;
            else itemBtns[i].SetActive(false);
        }
    }

    private void OnItemButtonClick(int index)
    {
        if (isItemSelected || isItemUsing) return;

        if (index != selectedItem)
        {
            itemDescription.text = bag[index].description;
            selectedItem = index;
            return;
        }

        if (bag[index].type == ItemType.MONSTERBALL)
        {
            for (int i = 0; i < playerPokemons.Count; i++)
            {
                if (playerPokemons[i] == BattleProcessor.Instance.GetPlayerPokemon().gameObject) continue;
                playerPokemons[i].SetActive(false);
            }

            BattleManager.Instance.SetPlayerCommand(new Command() { type = Command.TYPE.ITEM, number = selectedItem });
            GetComponent<Canvas>().enabled = false;
            return;
        }

        battleTextContent.text = "어떤 포켓몬에게 사용할까?";
        battleText.SetActive(true);
        isItemSelected = true;
        StartCoroutine(BattleTextOff());
    }

    private void OnPokemonButtonClick(int index)
    {
        if (!isItemSelected || isItemUsing) return;

        var pokemon = playerPokemons[index].GetComponent<Pokemon>();
        var item = bag[selectedItem];

        if (!item.CheckItemUseCondition(playerPokemons[index].GetComponent<Pokemon>()))
        {
            isItemSelected = false;

            battleTextContent.text = "사용할 수 없다!";
            battleText.SetActive(true);
            StartCoroutine(BattleTextOff());
            return;
        }




        if (BattleProcessor.Instance.GetPlayerPokemon() == playerPokemons[index].GetComponent<Pokemon>())
        {
            for (int i = 0; i < playerPokemons.Count; i++)
            {
                if (playerPokemons[i] == BattleProcessor.Instance.GetPlayerPokemon().gameObject) continue;
                playerPokemons[i].SetActive(false);
            }

            BattleManager.Instance.SetPlayerCommand(new Command() { type = Command.TYPE.ITEM, number = selectedItem });
            GetComponent<Canvas>().enabled = false;
            return;
        }

        item.Use(pokemon);
        item.amount--;
        if(item.amount == 0) bag.Remove(item);

        isItemUsing = true;

        switch (item.type)
        {
            case ItemType.HEAL_HP:
            case ItemType.RESURRECT:
                StartCoroutine(HPUpdate(pokemon, pokemonBtns[index]));
                break;

            case ItemType.HEAL_STATUSEFFECT:
                StartCoroutine(SEUpdate(pokemon, pokemonBtns[index]));
                break;

            case ItemType.HEAL_ALL:
                StartCoroutine(HPandSEUpdate(pokemon, pokemonBtns[index]));
                break;


            case ItemType.SKILL:
                break;

            default:
                break;
        }
    }

    private IEnumerator BattleTextOff()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        battleText.SetActive(false);
    }

    private IEnumerator HPUpdate(Pokemon pokemon, GameObject pokemonBtn)
    {
        battleText.SetActive(false);

        var slider = pokemonBtn.GetComponentInChildren<Slider>();
        int maxHP = pokemon.Max_HP;
        int curHP = pokemon.Cur_HP;

        while (slider.value < (float)curHP / maxHP)
        {
            slider.value += 0.01f;

            if ((int)(slider.value * maxHP) <= curHP)

            yield return null;
        }

        slider.value = (float)curHP / maxHP;
        yield return new WaitForSeconds(1);

        battleTextContent.text = pokemon.Name + "(은)는 건강해졌다!";
        battleText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        battleText.SetActive(false);

        for (int i = 0; i < playerPokemons.Count; i++)
        {
            if (playerPokemons[i] == BattleProcessor.Instance.GetPlayerPokemon().gameObject) continue;
            playerPokemons[i].SetActive(false);
        }

        BattleManager.Instance.SetPlayerCommand(new Command() { type = Command.TYPE.NONE });
        isItemUsing = false;
        GetComponent<Canvas>().enabled = false;
    }

    private IEnumerator SEUpdate(Pokemon pokemon, GameObject pokemonBtn)
    {
        battleText.SetActive(false);

        pokemonBtn.transform.GetChild((int)PokemonButtonChild.STATUSEFFECT).GetComponent<Image>().sprite = statusEffects[(int)pokemon.StatusEffect];
        yield return new WaitForSeconds(1);

        battleTextContent.text = pokemon.Name + "(은)는 건강해졌다!";
        battleText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        battleText.SetActive(false);

        for (int i = 0; i < playerPokemons.Count; i++)
        {
            if (playerPokemons[i] == BattleProcessor.Instance.GetPlayerPokemon().gameObject) continue;
            playerPokemons[i].SetActive(false);
        }

        BattleManager.Instance.SetPlayerCommand(new Command() { type = Command.TYPE.NONE });
        isItemUsing = false;
        GetComponent<Canvas>().enabled = false;
    }

    private IEnumerator HPandSEUpdate(Pokemon pokemon, GameObject pokemonBtn)
    {
        battleText.SetActive(false);

        pokemonBtn.transform.GetChild((int)PokemonButtonChild.STATUSEFFECT).GetComponent<Image>().sprite = statusEffects[(int)pokemon.StatusEffect];

        var slider = pokemonBtn.GetComponentInChildren<Slider>();
        int maxHP = pokemon.Max_HP;
        int curHP = pokemon.Cur_HP;

        while (slider.value < (float)curHP / maxHP)
        {
            slider.value += 0.01f;

            if ((int)(slider.value * maxHP) <= curHP)

            yield return null;
        }

        slider.value = (float)curHP / maxHP;
        yield return new WaitForSeconds(1);

        battleTextContent.text = pokemon.Name + "(은)는 건강해졌다!";
        battleText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.anyKeyDown);
        battleText.SetActive(false);

        for (int i = 0; i < playerPokemons.Count; i++)
        {
            if (playerPokemons[i] == BattleProcessor.Instance.GetPlayerPokemon().gameObject) continue;
            playerPokemons[i].SetActive(false);
        }

        BattleManager.Instance.SetPlayerCommand(new Command() { type = Command.TYPE.NONE });
        isItemUsing = false;
        GetComponent<Canvas>().enabled = false;
    }

    public void ReturnButton()
    {
        if (isItemUsing) return;

        GetComponent<Canvas>().enabled = false;
        commandSelect.SetActive(true);
        hpDisplay.SetActive(true);
    }
}
