using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;

public enum TURN
{
    ME, // false
    ENEMY //true
}

public enum WEATHER
{
    NONE,
    RAINY,
    SUNNY,
    SANDSTORM,
    SNOWY
}

public class BattleProcessor
{
    private static BattleProcessor instance = new BattleProcessor();

    private BattleProcessor() 
    {
        turnOrder = new Queue<TURN>();
        queue = new Queue<BattleAction>();
        typeProcessor = Type.Instance;

        pokemons = new Pokemon[2];
        trainers = new Trainer[2];
        sleepTurns = new int[2];
        superPoison = new int[2];

        pastSkills = new Stack<Skill>[2];
        for (int i = 0; i < 2; i++)
        {
            pastSkills[i] = new Stack<Skill>();
        }
    }

    public static BattleProcessor Instance { get { return instance; } }

    private Queue<TURN> turnOrder;
    private Queue<BattleAction> queue;
    private Type typeProcessor;

    private Pokemon[] pokemons;
    private Trainer[] trainers;

    private Stack<Skill>[] pastSkills;
    private int[] sleepTurns;
    private int[] superPoison;
    private WEATHER weather;
    private int weatherTurns;

    bool isTrainerBattle;

    public void Init(Trainer player, Trainer enemy)
    {
        trainers[0] = player;
        trainers[1] = enemy;

        pokemons[0] = player.TrainerPokemons[0].GetComponent<Pokemon>();
        pokemons[1] = enemy.TrainerPokemons[0].GetComponent<Pokemon>();

        for(int i = 0; i < 2; i ++)
        {
            foreach (var pokemon in trainers[i].TrainerPokemons)
            {
                pokemon.GetComponent<Pokemon>().isBattle = true;
            }
        }
    }

    public Queue<BattleAction> BattleOpening()
    {
        BattleAction action;
        queue.Clear();

        action = new BattleAction();
        action.type = ACTION.SET_CAMERA;
        action.camera = CAMERA.ENEMY_CLOSEUP;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = trainers[1].Name + "��(��) " + pokemons[1].Name + "��(��) �����´�!";
        action.waitSec = 2.5f;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILLSET;
        action.target = TARGET.ENEMY;
        action.pokemon = pokemons[1];
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_UISET;
        action.target = TARGET.ENEMY;
        action.pokemon = pokemons[1];
        action.curHP = pokemons[1].Cur_HP;
        action.maxHP = pokemons[1].Max_HP;
        action.statusEffect = pokemons[1].StatusEffect;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_ENTRANCE;
        action.target = TARGET.ENEMY;
        action.entrancePokemon = trainers[1].TrainerPokemons[0];
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.SET_CAMERA;
        action.camera = CAMERA.PLAYER_CLOSEUP;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = "����! " + pokemons[0].Name + "!";
        action.waitSec = 1.5f;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILLSET;
        action.target = TARGET.ME;
        action.pokemon = pokemons[0];
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_UISET;
        action.target = TARGET.ME;
        action.pokemon = pokemons[0];
        action.curHP = pokemons[0].Cur_HP;
        action.maxHP = pokemons[0].Max_HP;
        action.statusEffect = pokemons[0].StatusEffect;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_ENTRANCE;
        action.target = TARGET.ME;
        action.entrancePokemon = trainers[0].TrainerPokemons[0];
        queue.Enqueue(action);

        pokemons[0].DebugSkill();
        isTrainerBattle = true;

        return queue;
    }

    public Queue<BattleAction> WildBattleOpening()
    {
        BattleAction action;
        queue.Clear();

        action = new BattleAction();
        action.type = ACTION.SET_CAMERA;
        action.camera = CAMERA.WILD_CLOSEUP;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = "�߻��� " + pokemons[1].Name + "(��)�� ��Ÿ����!";
        action.waitSec = 2.5f;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILLSET;
        action.target = TARGET.ENEMY;
        action.pokemon = pokemons[1];
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_UISET;
        action.target = TARGET.ENEMY;
        action.pokemon = pokemons[1];
        action.curHP = pokemons[1].Cur_HP;
        action.maxHP = pokemons[1].Max_HP;
        action.statusEffect = pokemons[1].StatusEffect;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.SET_CAMERA;
        action.camera = CAMERA.PLAYER_CLOSEUP;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = "����! " + pokemons[0].Name + "!";
        action.waitSec = 1.5f;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILLSET;
        action.target = TARGET.ME;
        action.pokemon = pokemons[0];
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_UISET;
        action.target = TARGET.ME;
        action.pokemon = pokemons[0];
        action.curHP = pokemons[0].Cur_HP;
        action.maxHP = pokemons[0].Max_HP;
        action.statusEffect = pokemons[0].StatusEffect;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_ENTRANCE;
        action.target = TARGET.ME;
        action.entrancePokemon = trainers[0].TrainerPokemons[0];
        queue.Enqueue(action);

        pokemons[0].DebugSkill();

        isTrainerBattle = false;

        return queue;
    }


    public Queue<BattleAction> GetBattleActionQueue(Command myCommand, Command enemyCommand)
    {
        BattleAction action;
        CheckTurnOrder(myCommand, enemyCommand);
        int attacker;
        int defender;

        queue.Clear();

        while (turnOrder.Count > 0)
        {
            Command command;
            TURN turn = turnOrder.Dequeue();
            attacker = (int)turn;

            Debug.Log("Turn : " + turn);

            if (turn == TURN.ME)
            {
                command = myCommand;
                defender = (int)TURN.ENEMY;
            }
            else
            {
                command = enemyCommand;
                defender = (int)TURN.ME;
            }

            /*
             ********************** ����ģ�� **********************
             */
            if (command.type == Command.TYPE.RUN)
            {
                if(isTrainerBattle)
                {
                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = "������ �ºο��� ���� ���� �� ����!";
                    queue.Enqueue(action);

                    return queue;
                }

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                if (turn == TURN.ME) action.battleText = "������ �����ƴ�!";
                else action.battleText = pokemons[attacker].Name + "��(��) ������ ���ȴ�...";
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.RUN;
                queue.Enqueue(action);

                return queue;
            }

            /*
             ********************** ������ ��� **********************
             */
            else if (command.type == Command.TYPE.ITEM)
            {
                // Get Item from player bag
                Item item = trainers[attacker].Bag[command.number];


                // Reduce amount from bag and removes when amount is zero
                item.amount--;
                if (item.amount == 0) trainers[attacker].Bag.Remove(item);


                // Use item when not monsterball
                if (item.type != ItemType.MONSTERBALL)
                {
                    item.Use(pokemons[attacker]);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = trainers[attacker].Name + "(��)�� " + item.itemName + "(��)�� ���!";
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.HEAL_ANIM;
                    action.target = (TARGET)attacker;
                    queue.Enqueue(action);

                    switch (item.type)
                    {
                        case ItemType.HEAL_HP:

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_HP_UPDATE;
                            action.target = (TARGET)attacker;
                            action.curHP = pokemons[attacker].Cur_HP;
                            action.maxHP = pokemons[attacker].Max_HP;
                            queue.Enqueue(action);

                            break;

                        case ItemType.HEAL_STATUSEFFECT:

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_SE_UPDATE;
                            action.target = (TARGET)attacker;
                            action.statusEffect = pokemons[attacker].StatusEffect;
                            queue.Enqueue(action);

                            break;

                        case ItemType.HEAL_ALL:

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_HP_UPDATE;
                            action.target = (TARGET)attacker;
                            action.curHP = pokemons[attacker].Cur_HP;
                            action.maxHP = pokemons[attacker].Max_HP;
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_SE_UPDATE;
                            action.target = (TARGET)attacker;
                            action.statusEffect = pokemons[attacker].StatusEffect;
                            queue.Enqueue(action);

                            break;

                        default:
                            break;
                    }

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = pokemons[attacker].Name + "�� ü���� ȸ���Ǿ���!";
                    queue.Enqueue(action);
                }

                // use Item when monsterball
                else
                {
                    // if it is Trainer battle, can't catch pokemon
                    if(isTrainerBattle)
                    {
                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = "������ ���ϸ��� ���� ���� ����!";
                        queue.Enqueue(action);

                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = "���� �Ϳ� �մ�� ����!";
                        queue.Enqueue(action);

                        return queue;
                    }

                    // Catching Pokemon

                    int rate = item.catchRate;
                    int rand = UnityEngine.Random.Range(1, 101);
                    int tick;
                    bool catched = false;

                    if (rand > 100 - rate) catched = true; // ����

                    // tick count
                    if (rand > 90) tick = 3;
                    else if (rand > 60) tick = 2;
                    else if (rand > 30) tick = 1;
                    else tick = 0;

                    action = new BattleAction();
                    action.type = ACTION.CATCH_ANIMATION;
                    action.catched = catched;
                    action.tick = tick;
                    queue.Enqueue(action);

                    if (catched)
                    {
                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = "�ų���! " + pokemons[1].Name + "(��)�� ��Ҵ�!";
                        queue.Enqueue(action);

                        int trainerPokeCount = trainers[0].TrainerPokemons.Count;

                        if (trainerPokeCount < 6)
                        {
                            var catchPokemon = pokemons[1].gameObject;

                            trainers[0].TrainerPokemons.Add(catchPokemon);
                            UnityEngine.Object.DontDestroyOnLoad(catchPokemon);

                            action = new BattleAction();
                            action.type = ACTION.CATCH;
                            queue.Enqueue(action);

                            return queue;
                        }

                        else
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = trainers[0].Name + "(��)�� ���ϸ��� �� ������ �ٴ� �� ����...";
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[1].Name + "(��)�� ����� ���ϸ��� �����ְڽ��ϱ�?";
                            action.textSelect = true;
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_CHANGE;
                            action.catched = true;
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.CATCH;
                            queue.Enqueue(action);

                            return queue;
                        }
                    }

                    else
                    {
                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = "�� ��! ���ϸ��� ������ Ƣ��Դ�!";
                        queue.Enqueue(action);
                    }
                }
            }

            /*
             ********************** ���ϸ� �ٲٱ� **********************
             */
            else if (command.type == Command.TYPE.POKEMON)
            {
                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                if (turn == TURN.ME) action.battleText = "���ƿ�! " + pokemons[attacker].Name + "!";
                else action.battleText = trainers[attacker].Name + "��(��) " + pokemons[attacker].Name + "��(��) ����־���!";
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.POKEMON_EXIT;
                action.target = (TARGET)turn;
                action.exitPokemon = pokemons[attacker].gameObject;
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                if (turn == TURN.ME) action.battleText = "����! " + trainers[attacker].TrainerPokemons[command.number].GetComponent<Pokemon>().Name + "!";
                else action.battleText = trainers[attacker].Name + "��(��) " + trainers[attacker].TrainerPokemons[command.number].GetComponent<Pokemon>().Name + "��(��) �����´�!";
                queue.Enqueue(action);

                pokemons[attacker].Rank_Refresh();
                pokemons[attacker] = trainers[attacker].TrainerPokemons[command.number].GetComponent<Pokemon>();
                pastSkills[attacker].Clear();
                superPoison[attacker] = 0;

                action = new BattleAction();
                action.type = ACTION.POKEMON_ENTRANCE;
                action.target = (TARGET)turn;
                action.entrancePokemon = pokemons[attacker].gameObject;
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.POKEMON_SKILLSET;
                action.target = (TARGET)turn;
                action.pokemon = pokemons[attacker];
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.POKEMON_UISET;
                action.target = (TARGET)turn;
                action.pokemon = pokemons[attacker];
                action.curHP = pokemons[attacker].Cur_HP;
                action.maxHP = pokemons[attacker].Max_HP;
                action.statusEffect = pokemons[attacker].StatusEffect;
                queue.Enqueue(action);
            }

            /*
             ********************** ��� ��� **********************
             */
            else
            {
                // ��� ��� �� //

                PokemonDebug(pokemons[attacker], "attacker pokemon");

                PokemonDebug(pokemons[defender], "defender pokemon");


                bool skipTurn = false;
                int rand;

                // �����̻� Ȯ�� //
                switch(pokemons[attacker].StatusEffect)
                {
                    case STATUSEFFECT.PARALYSIS:

                        rand = UnityEngine.Random.Range(0, 4);
                        if (rand == 0)
                        {
                            skipTurn = true;

                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[attacker].Name + "��(��) ���� ������ ������ �� ����!";
                            queue.Enqueue(action);
                        }

                        break;

                    case STATUSEFFECT.SLEEP:

                        rand = UnityEngine.Random.Range(1, 4);
                        string sleepText;

                        // sleepTurns[(int)turn] �� ����� ��� == ���� ���, ������ ��� == ���� �� ���
                        if (sleepTurns[(int)turn] >= 0)
                        {
                            if (rand > sleepTurns[(int)turn])
                            {
                                pokemons[attacker].StatusEffect = STATUSEFFECT.NONE;

                                action = new BattleAction();
                                action.type = ACTION.POKEMON_SE_UPDATE;
                                action.target = (TARGET)attacker;
                                action.statusEffect = STATUSEFFECT.NONE;
                                queue.Enqueue(action);

                                sleepText = pokemons[attacker].Name + "��(��) ���� ����!";
                            }
                            else
                            {
                                sleepTurns[(int)turn]--;
                                skipTurn = true;
  
                                sleepText = pokemons[attacker].Name + "��(��) ���� ���� �ִ�...";
                            }
                        }
                        else
                        {
                            sleepTurns[(int)turn]++;
                            skipTurn = true;

                            sleepText = pokemons[attacker].Name + "��(��) ���� ���� �ִ�...";
                        }

                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = sleepText;
                        queue.Enqueue(action);

                        break;

                    case STATUSEFFECT.FREEZE:

                        rand = UnityEngine.Random.Range(0, 5);

                        if (rand == 0)
                        {
                            pokemons[attacker].StatusEffect = STATUSEFFECT.NONE;

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_SE_UPDATE;
                            action.target = (TARGET)attacker;
                            action.statusEffect = STATUSEFFECT.NONE;
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[attacker].Name + "�� ���� ��Ҵ�!";
                            queue.Enqueue(action);
                        }
                        else
                        {
                            skipTurn = true;

                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[attacker].Name + "��(��) �������� ������ �� ����!";
                            queue.Enqueue(action);
                        }

                        break;
                }

                if (skipTurn) continue;

                var skill = pokemons[attacker].Skills[command.number];

                skill.CurPP--;

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = pokemons[attacker].Name + "�� " + skill.SkillName + "!";
                queue.Enqueue(action);

                // ���߷� Ȯ��

                int rate = UnityEngine.Random.Range(0, 100);
                if(rate >= skill.Rate)
                {
                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = "������ �������� ���Ҵ�!";
                    queue.Enqueue(action);

                    skipTurn = true;
                }

                if (skipTurn) continue;

                // ��� ���� ����

                action = new BattleAction();
                action.type = ACTION.SKILL_ANIM;
                action.target = (TARGET)attacker;
                action.pokemon = pokemons[attacker];
                action.defender = pokemons[defender];
                action.skill = skill;
                queue.Enqueue(action);

                // ���� ���� ������ �ߵ��ϴ� ȿ��

                if (skipTurn == false)
                    skipTurn = InvokingCondition(skill);

                SkillEffect(skill, turn, out bool skipAttack, 0, INVOKING_TIMING.PRETURN);

                if (skipTurn) continue;


                // UI�� PP �Ҹ� ������Ʈ

                action = new BattleAction();
                action.type = ACTION.SKILL_PP_UPDATE;
                action.target = (TARGET)turn;
                action.pokemon = pokemons[attacker];
                queue.Enqueue(action);


                if (skill.Category == SKILL_CATEGORY.ATTACK && !skipAttack)
                {
                    // ��ų ������
                    int levelCoeff = (pokemons[attacker].Level * 2 / 5) + 2; // ���� ���
                    Debug.Log(levelCoeff);
                    int skillPower = skill.Power; // ��ų �Ŀ�
                    int attack = GetAttack(pokemons[attacker]); // ������ ���ݷ�
                    Debug.Log(attack);
                    int defense = GetDefense(pokemons[attacker]); // ����� ����

                    int defaultDamage = (levelCoeff * skillPower * attack / defense / 50) + 2; // �⺻ ������
                    Debug.Log(defaultDamage);

                    double sameType = typeProcessor.SameType(pokemons[attacker], skill) ? 1.5 : 1; // �ڼ� ����
                    double defendType = typeProcessor.TypeCalc(skill.Type, pokemons[defender].Types); // ��� Ÿ�� ���

                    int damage = (int)(defaultDamage * sameType * defendType); // ���� ������

                    if (damage < 1) { damage = 1; } // �������� 1���� ������ 1�� ����

                    Debug.Log("now Pokemon : " + turn + ", inflicted damage : " + damage);

                    pokemons[defender].Cur_HP -= damage; // ���� ������ ����
                    if (pokemons[defender].Cur_HP < 0) pokemons[defender].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)defender;
                    action.curHP = pokemons[defender].Cur_HP;
                    action.maxHP = pokemons[defender].Max_HP;
                    queue.Enqueue(action);

                    double type = Type.Instance.TypeCalc(skill.Type, pokemons[defender].Types);

                    if(type != 1)
                    {
                        string effective = "";

                        switch (type)
                        {
                            case 0:
                                effective = "ȿ���� ���� �� ����...";
                                break;
                                   
                            case 0.5:
                            case 0.25:
                                effective = "ȿ���� ������ �� ����...";
                                break;

                            case 2:
                            case 4:
                                effective = "ȿ���� �����ߴ�!";
                                break;
                        }

                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = effective;
                        queue.Enqueue(action);
                    }

                    if (PokemonDefeat(defender)) return queue;


                    // ���ݽ�ų ���� �� ȿ��

                    SkillEffect(skill, turn, out _, damage);
                }
                else if (skill.Category == SKILL_CATEGORY.STATUS && !skipAttack)
                {
                    // ��ȭ�� ȿ��

                    SkillEffect(skill, turn, out _, 0);
                }


                // ��ų ȿ���� �������� ��

                if (PokemonDefeat(defender)) return queue;
                if (PokemonDefeat(attacker)) return queue;


                // ���� ��ų ���ÿ� �ֱ�

                pastSkills[attacker].Push(skill);
            }
        }


        // ���� �� ���� �� �����̻�

        CheckTurnOrder();

        while(turnOrder.Count > 0)
        {
            int i = (int)turnOrder.Dequeue();

            switch (pokemons[i].StatusEffect)
            {
                case STATUSEFFECT.BURN:

                    int burnDotDamage = pokemons[i].Max_HP / 16;

                    if (burnDotDamage < 1) burnDotDamage = 1;

                    pokemons[i].Cur_HP -= burnDotDamage;
                    if (pokemons[i].Cur_HP < 0) pokemons[i].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.STATUS_EFFECT_ANIM;
                    action.target = (TARGET)i;
                    action.statusEffect = STATUSEFFECT.BURN;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)i;
                    action.curHP = pokemons[i].Cur_HP;
                    action.maxHP = pokemons[i].Max_HP;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = pokemons[i].Name + "��(��) ȭ�� �������� �Ծ���!";
                    queue.Enqueue(action);

                    break;

                case STATUSEFFECT.POISON:

                    int poisonDotDamage = pokemons[i].Max_HP / 8;

                    if (poisonDotDamage < 1) poisonDotDamage = 1;

                    pokemons[i].Cur_HP -= poisonDotDamage;
                    if (pokemons[i].Cur_HP < 0) pokemons[i].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.STATUS_EFFECT_ANIM;
                    action.target = (TARGET)i;
                    action.statusEffect = STATUSEFFECT.POISON;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)i;
                    action.curHP = pokemons[i].Cur_HP;
                    action.maxHP = pokemons[i].Max_HP;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = pokemons[i].Name + "��(��) ���� ���� �������� �Ծ���!";
                    queue.Enqueue(action);

                    break;

                case STATUSEFFECT.SUPERPOISON:

                    int superPoisonDamage = pokemons[i].Max_HP * ++superPoison[i] / 16;

                    if (superPoisonDamage < 1) superPoisonDamage = 1;

                    pokemons[i].Cur_HP -= superPoisonDamage;
                    if (pokemons[i].Cur_HP < 0) pokemons[i].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.STATUS_EFFECT_ANIM;
                    action.target = (TARGET)i;
                    action.statusEffect = STATUSEFFECT.POISON;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)i;
                    action.curHP = pokemons[i].Cur_HP;
                    action.maxHP = pokemons[i].Max_HP;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = pokemons[i].Name + "��(��) ���� ���� �������� �Ծ���!";
                    queue.Enqueue(action);

                    break;
            }

            if (PokemonDefeat(i)) return queue;
        }

        return queue;
    }

    /// <summary>
    /// Checkes turn order with command nad speed
    /// </summary>
    private void CheckTurnOrder(Command myCommand, Command enemyCommand)
    {
        turnOrder.Clear();

        int myCmdSpd = 0, enemyCmdSpd = 0;

        switch(myCommand.type)
        {
            case Command.TYPE.RUN: myCmdSpd = 4; break;
            case Command.TYPE.ITEM: myCmdSpd = 4; break; 
            case Command.TYPE.POKEMON: myCmdSpd = 2; break;
            case Command.TYPE.SKILL: myCmdSpd = 1; break;
            default: turnOrder.Enqueue(TURN.ENEMY); return;
        }
        switch (enemyCommand.type)
        {
            case Command.TYPE.RUN: enemyCmdSpd = 3; break;
            case Command.TYPE.ITEM: enemyCmdSpd = 3; break;
            case Command.TYPE.POKEMON: enemyCmdSpd = 2; break;
            case Command.TYPE.SKILL: enemyCmdSpd = 1; break;
            default: turnOrder.Enqueue(TURN.ME); return;
        }

        // �ൿ ���� ��
        if (myCmdSpd > enemyCmdSpd) { turnOrder.Enqueue(TURN.ME); turnOrder.Enqueue(TURN.ENEMY); return; } // me first
        else if (myCmdSpd < enemyCmdSpd) { turnOrder.Enqueue(TURN.ENEMY); turnOrder.Enqueue(TURN.ME); return; } // enemy first
        else
        // �� �� ����� ��� �켱�� ��
        if (myCommand.type == Command.TYPE.SKILL && enemyCommand.type == Command.TYPE.SKILL)
        {
            if (pokemons[0].Skills[myCommand.number].Priority > pokemons[1].Skills[enemyCommand.number].Priority) { turnOrder.Enqueue(TURN.ME); turnOrder.Enqueue(TURN.ENEMY); return; }
            else if (pokemons[0].Skills[myCommand.number].Priority < pokemons[1].Skills[enemyCommand.number].Priority) { turnOrder.Enqueue(TURN.ENEMY); turnOrder.Enqueue(TURN.ME); return; }
        }
        CheckTurnOrder();
    }

    /// <summary>
    /// Checkes turn order with only speed
    /// </summary>
    private void CheckTurnOrder()
    {
        int mySpeed = GetSpeed(pokemons[0]);
        int enemySpeed = GetSpeed(pokemons[1]);

        if (mySpeed > enemySpeed) { turnOrder.Enqueue(TURN.ME); turnOrder.Enqueue(TURN.ENEMY); return; }
        else if (mySpeed < enemySpeed) { turnOrder.Enqueue(TURN.ENEMY); turnOrder.Enqueue(TURN.ME); return; }

        int rand = UnityEngine.Random.Range(0, 2);
        if (rand == 0) { turnOrder.Enqueue(TURN.ME); turnOrder.Enqueue(TURN.ENEMY); return; }
        else { turnOrder.Enqueue(TURN.ENEMY); turnOrder.Enqueue(TURN.ME); return; }
    }

    private bool PokemonDefeat(int fainted)
    {
        if (pokemons[fainted].Cur_HP == 0)
        {
            pokemons[fainted].Defeat = true;

            pokemons[fainted].Rank_Refresh();

            BattleAction action;
            action = new BattleAction();
            action.type = ACTION.POKEMON_EXIT;
            action.target = (TARGET)fainted;
            action.exitPokemon = pokemons[fainted].gameObject;
            queue.Enqueue(action);

            action = new BattleAction();
            action.type = ACTION.BATTLE_TEXT;
            action.battleText = pokemons[fainted].Name + "��(��) ��������!";
            queue.Enqueue(action);

            pokemons[fainted].StatusEffect = STATUSEFFECT.NONE;

            if ((TARGET)fainted == TARGET.ENEMY)
            {
                // ����ġ ���� ���� �߰�

                int gainingExp = 125 * pokemons[fainted].Level;

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = pokemons[0].Name + "��(��) " + gainingExp + "�� ����ġ�� �����!";
                queue.Enqueue(action);

                pokemons[0].Exp += gainingExp;

                while(true)
                {
                    action = new BattleAction();
                    action.type = ACTION.POKEMON_EXP_UPDATE;
                    action.exp = pokemons[0].Exp;
                    action.maxExp = pokemons[0].RequiredExpUntilNextLevel();
                    queue.Enqueue(action);

                    if (pokemons[0].Exp >= pokemons[0].RequiredExpUntilNextLevel())
                    {
                        pokemons[0].Exp -= pokemons[0].RequiredExpUntilNextLevel();

                        var diff = pokemons[0].LevelUp();

                        action = new BattleAction();
                        action.type = ACTION.POKEMON_LEVELUP;
                        action.level = pokemons[0].Level;
                        action.maxExp = pokemons[0].RequiredExpUntilNextLevel();
                        action.curHP = pokemons[0].Cur_HP;
                        action.maxHP = pokemons[0].Max_HP;
                        queue.Enqueue(action);

                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = pokemons[0].Name + "��(��) " + pokemons[0].Level + "������ �Ǿ���!";
                        queue.Enqueue(action);

                        action = new BattleAction();
                        action.type = ACTION.POKEMON_LEVELUP_NOTICE;
                        action.statDifference = diff;
                        action.maxHP = pokemons[0].Max_HP;
                        action.atk = pokemons[0].Attack;
                        action.def = pokemons[0].Defense;
                        action.spd = pokemons[0].Speed;
                        queue.Enqueue(action);


                        int skillNum = pokemons[0].LearningSkillFromLevel();
                        if (skillNum != 0)
                        {
                            var skill = SkillManager.Instance.GetSkill(skillNum);

                            action = new BattleAction();
                            action.type = ACTION.POKEMON_SKILL_TRYTOLEARN;
                            action.learningSkill = skill;
                            action.pokemon = pokemons[0];
                            queue.Enqueue(action);
                        }
                    }
                    else break;
                }
            }


            bool playerDefeat = TrainerDefeat(trainers[(int)TURN.ME]);
            bool enemyDefeat = TrainerDefeat(trainers[(int)TURN.ENEMY]);

            if (enemyDefeat && isTrainerBattle)
            {
                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = trainers[1].Name + "(��)���� �ºο��� �̰��! ";
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = (trainers[1] as Enemy).defeatComment;
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.BATTLE_END;
                action.winner = ACTION.PLAYER_WIN;
                queue.Enqueue(action);

                return true;
            }
            else if (playerDefeat && isTrainerBattle)
            {
                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = trainers[1].Name + "(��)���� �ºο��� ����... ";
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = "������ įį������! ";
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.BATTLE_END;
                action.winner = ACTION.ENEMY_WIN;
                queue.Enqueue(action);

                return true;
            }

            // Wild Battle
            else if (enemyDefeat && !isTrainerBattle)
            {
                action = new BattleAction();
                action.type = ACTION.BATTLE_END;
                action.winner = ACTION.PLAYER_WIN;
                queue.Enqueue(action);

                return true;
            }
            else if (playerDefeat && !isTrainerBattle)
            {
                action = new BattleAction();
                action.type = ACTION.BATTLE_END;
                action.winner = ACTION.ENEMY_WIN;
                queue.Enqueue(action);

                return true;
            }

            action = new BattleAction();
            action.type = ACTION.POKEMON_CHANGE;
            action.target = (TARGET)fainted;
            queue.Enqueue(action);

            return true;
        }
        return false;
    }

    private bool TrainerDefeat(Trainer trainer)
    {
        foreach(var pok in trainer.TrainerPokemons)
        {
            if (pok.GetComponent<Pokemon>().Defeat == false) return false;
        }

        return true;
    }

    public void SkillEffect(Skill skill, TURN turn, out bool skipAttack, int damage, INVOKING_TIMING timing = INVOKING_TIMING.POSTTURN)
    {
        BattleAction action;
        skipAttack = false;

        int attacker = (int)turn;
        int other; // Ÿ���� �ƴ� ��
        int target;

        foreach (var effect in skill.Effects)
        {
            if (effect.invokingTiming != timing) continue;

            if(effect.effect_Rate > 0)
            {
                int rand = UnityEngine.Random.Range(1, 101);
                if (rand > effect.effect_Rate) continue;
            }

            if (effect.effectTarget == TARGET.ME) target = attacker;
            else { if (attacker == 0) target = 1; else target = 0; }

            if (target == 0) other = 1;
            else other = 0;

            switch (effect.skillEffect)
            {
                case SKILL_EFFECT.RANKCHANGE_ATTACK:

                    pokemons[target].Atk_Rank += RankEffect(pokemons[target].Atk_Rank, pokemons[other].Atk_Rank, "������");

                    if (pokemons[target].Atk_Rank > 6) pokemons[target].Atk_Rank = 6;
                    else if (pokemons[target].Atk_Rank < -6) pokemons[target].Atk_Rank = -6;

                    break;

                case SKILL_EFFECT.RANKCHANGE_DEFENSE:

                    pokemons[target].Def_Rank += RankEffect(pokemons[target].Def_Rank, pokemons[other].Def_Rank, "��");

                    if (pokemons[target].Def_Rank > 6) pokemons[target].Def_Rank = 6;
                    else if (pokemons[target].Def_Rank < -6) pokemons[target].Def_Rank = -6;

                    break;

                case SKILL_EFFECT.RANKCHANGE_SPEED:

                    pokemons[target].Spd_Rank += RankEffect(pokemons[target].Spd_Rank, pokemons[other].Spd_Rank, "���ǵ尡");

                    if (pokemons[target].Spd_Rank > 6) pokemons[target].Spd_Rank = 6;
                    else if (pokemons[target].Spd_Rank < -6) pokemons[target].Spd_Rank = -6;

                    break;

                case SKILL_EFFECT.STATUSEFFECT_POISON:

                    if (pokemons[target].StatusEffect != STATUSEFFECT.NONE)
                    {
                        if (skill.Category == SKILL_CATEGORY.STATUS)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = "�׷��� �����ϰ� ���Ҵ�!";
                            queue.Enqueue(action);
                        }
                        break;
                    }

                    pokemons[target].StatusEffect = STATUSEFFECT.POISON;

                    StatusEffect(STATUSEFFECT.POISON, "�� ���� ���� ������!");

                    break;

                case SKILL_EFFECT.STATUSEFFECT_SUPERPOISON:

                    if (pokemons[target].StatusEffect != STATUSEFFECT.NONE)
                    {
                        if (skill.Category == SKILL_CATEGORY.STATUS)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = "�׷��� �����ϰ� ���Ҵ�!";
                            queue.Enqueue(action);
                        }
                        break;
                    }

                    pokemons[target].StatusEffect = STATUSEFFECT.SUPERPOISON;

                    StatusEffect(STATUSEFFECT.SUPERPOISON, "�� ���� �͵��� ������!");

                    break;

                case SKILL_EFFECT.STATUSEFFECT_BURN:

                    if (pokemons[target].StatusEffect != STATUSEFFECT.NONE)
                    {
                        if (skill.Category == SKILL_CATEGORY.STATUS)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = "�׷��� �����ϰ� ���Ҵ�!";
                            queue.Enqueue(action);
                        }
                        break;
                    }

                    pokemons[target].StatusEffect = STATUSEFFECT.BURN;

                    StatusEffect(STATUSEFFECT.BURN, "��(��) ȭ���� �Ծ���!");

                    break;

                case SKILL_EFFECT.STATUSEFFECT_PARALYSIS:

                    if (pokemons[target].StatusEffect != STATUSEFFECT.NONE)
                    {
                        if (skill.Category == SKILL_CATEGORY.STATUS)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = "�׷��� �����ϰ� ���Ҵ�!";
                            queue.Enqueue(action);
                        }
                        break;
                    }

                    pokemons[target].StatusEffect = STATUSEFFECT.PARALYSIS;

                    StatusEffect(STATUSEFFECT.PARALYSIS, "��(��) ����Ǿ� ����� ������ ���������!");

                    break;

                case SKILL_EFFECT.STATUSEFFECT_SLEEP:

                    if (pokemons[target].StatusEffect != STATUSEFFECT.NONE)
                    {
                        if (skill.Category == SKILL_CATEGORY.STATUS)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = "�׷��� �����ϰ� ���Ҵ�!";
                            queue.Enqueue(action);
                        }
                        break;
                    }

                    pokemons[target].StatusEffect = STATUSEFFECT.SLEEP;
                    if (effect.effect_Value != 0) sleepTurns[target] = -effect.effect_Value;
                    else sleepTurns[target] = 3;

                    StatusEffect(STATUSEFFECT.SLEEP, "��(��) ���� ���ȴ�!");

                    break;

                case SKILL_EFFECT.STATUSEFFECT_FREEZE:

                    if (pokemons[target].StatusEffect != STATUSEFFECT.NONE)
                    {
                        if (skill.Category == SKILL_CATEGORY.STATUS)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = "�׷��� �����ϰ� ���Ҵ�!";
                            queue.Enqueue(action);
                        }
                        break;
                    }

                    pokemons[target].StatusEffect = STATUSEFFECT.FREEZE;

                    StatusEffect(STATUSEFFECT.FREEZE, "��(��) ���پ���!");

                    break;

                case SKILL_EFFECT.REBOUND_DAMAGE:

                    int reboundDamage = damage / effect.effect_Value;
                    if (reboundDamage < 1) reboundDamage = 1;

                    pokemons[attacker].Cur_HP -= reboundDamage;
                    if (pokemons[attacker].Cur_HP < 0) pokemons[attacker].Cur_HP = 0;

                    Debug.Log(pokemons[attacker].Cur_HP);

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)attacker;
                    action.curHP = pokemons[attacker].Cur_HP;
                    action.maxHP = pokemons[attacker].Max_HP;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = pokemons[target].Name + "��(��) �ݵ����� �������� �Ծ���!";
                    queue.Enqueue(action);

                    break;

                case SKILL_EFFECT.REBOUND_DEVIATE:

                    int deviateDamage = pokemons[attacker].Max_HP / effect.effect_Value;
                    if (deviateDamage < 1) deviateDamage = 1;

                    pokemons[attacker].Cur_HP -= deviateDamage;
                    if (pokemons[attacker].Cur_HP < 0) pokemons[attacker].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)attacker;
                    action.curHP = pokemons[attacker].Cur_HP;
                    action.maxHP = pokemons[attacker].Max_HP;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = pokemons[target].Name + "��(��) �ݵ����� �������� �Ծ���!";
                    queue.Enqueue(action);

                    break;

                case SKILL_EFFECT.REBOUND_TURN:
                    // not implemented
                    break;

                case SKILL_EFFECT.ONEHIT:

                    if (typeProcessor.TypeCalc(skill.Type, pokemons[target].Types) == 0)
                    {
                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = "ȿ���� ���� �� ����...";
                        queue.Enqueue(action);

                        break;
                    }

                    pokemons[target].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)target;
                    action.curHP = pokemons[target].Cur_HP;
                    action.maxHP = pokemons[target].Max_HP;
                    queue.Enqueue(action);

                    action = new BattleAction();
                    action.type = ACTION.BATTLE_TEXT;
                    action.battleText = "�ϰ��ʻ�!";
                    queue.Enqueue(action);

                    skipAttack = true;

                    break;

                case SKILL_EFFECT.DEFENSE:

                    //pokemons[attacker].IsDefending = true;

                    break;

                case SKILL_EFFECT.EXPLOSION:

                    pokemons[attacker].Cur_HP = 0;

                    action = new BattleAction();
                    action.type = ACTION.POKEMON_HP_UPDATE;
                    action.target = (TARGET)attacker;
                    action.curHP = pokemons[attacker].Cur_HP;
                    action.maxHP = pokemons[attacker].Max_HP;
                    queue.Enqueue(action);

                    break;

                case SKILL_EFFECT.REQUIRED_TWO_TURNS:

                    if (pastSkills[(int)turn].Peek().SkillName != skill.SkillName) break;

                    // not implemented
                    break;

                case SKILL_EFFECT.FIXED_DAMAGE:

                    pokemons[target].Cur_HP -= effect.effect_Value;
                    if (pokemons[target].Cur_HP < 0) pokemons[target].Cur_HP = 0;

                    skipAttack = true;
                    break;

                case SKILL_EFFECT.HEAL_DAMAGE:

                    if (pokemons[attacker].Cur_HP == pokemons[attacker].Max_HP)
                    {
                        break;
                    }

                    int damageHeal = damage / effect.effect_Value;
                    if (damageHeal < 1) damageHeal = 1;
                    pokemons[attacker].Cur_HP += damageHeal;

                    if (pokemons[attacker].Cur_HP > pokemons[attacker].Max_HP)
                    {
                        pokemons[attacker].Cur_HP = pokemons[attacker].Max_HP;
                    }

                    break;

                case SKILL_EFFECT.HEAL_WEATHER:
                    // not implemented
                    break;

                case SKILL_EFFECT.HEAL_PURE:

                    if (pokemons[attacker].Cur_HP == pokemons[attacker].Max_HP)
                    {
                        break;
                    }

                    int pureHeal = pokemons[target].Max_HP / effect.effect_Value;
                    pokemons[attacker].Cur_HP += pureHeal;

                    if (pokemons[target].Cur_HP > pokemons[target].Max_HP)
                    {
                        pokemons[target].Cur_HP = pokemons[target].Max_HP;
                    }

                    break;

                default:
                    break;
            }

            int RankEffect(int targetRank, int enemyRank, string rankWord)
            {
                switch ((SPECIALRANK)effect.effect_Value)
                {
                    case SPECIALRANK.RETURN_TO_ZERO:
                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = pokemons[target].Name + "�� " + rankWord + " ������� ���ư���!";
                        queue.Enqueue(action);
                        return -targetRank;

                    case SPECIALRANK.COPY_ENEMY:
                        action = new BattleAction();
                        action.type = ACTION.BATTLE_TEXT;
                        action.battleText = pokemons[target].Name + "�� " + rankWord + " " + pokemons[other].Name + "��(��) �Ȱ��� ���ߴ�!";
                        queue.Enqueue(action);
                        return enemyRank - targetRank;

                    case SPECIALRANK.INCREASED_STATUS_TO_ZERO:
                        if (targetRank > 0)
                        {
                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[target].Name + "�� " + rankWord + " ������� ���ư���!";
                            queue.Enqueue(action);
                            return -targetRank;
                        }
                        else return 0;

                    default:
                        if (effect.effect_Value > 0)
                        {
                            action = new BattleAction();
                            action.type = ACTION.RANKUP_ANIM;
                            action.target = (TARGET)target;
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[target].Name + "�� " + rankWord + " �ö󰬴�!";
                            queue.Enqueue(action);
                        }
                        else if (effect.effect_Value < 0)
                        {
                            action = new BattleAction();
                            action.type = ACTION.RANKDOWN_ANIM;
                            action.target = (TARGET)target;
                            queue.Enqueue(action);

                            action = new BattleAction();
                            action.type = ACTION.BATTLE_TEXT;
                            action.battleText = pokemons[target].Name + "�� " + rankWord + " ��������!";
                            queue.Enqueue(action);
                        }
                        return effect.effect_Value;
                }
            }

            void StatusEffect(STATUSEFFECT statusEffect, string seWord)
            {
                action = new BattleAction();
                action.type = ACTION.STATUS_EFFECT_ANIM;
                action.target = (TARGET)target;
                action.statusEffect = statusEffect;
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.POKEMON_SE_UPDATE;
                action.target = (TARGET)target;
                action.statusEffect = statusEffect;
                queue.Enqueue(action);

                action = new BattleAction();
                action.type = ACTION.BATTLE_TEXT;
                action.battleText = pokemons[target].Name + seWord;
                queue.Enqueue(action);
            }
        }
    }

    public Queue<BattleAction> SkillLearn(string skillName, string pokeName)
    {
        Queue<BattleAction> queue = new Queue<BattleAction>();

        BattleAction action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = pokeName + "(��)�� " + skillName + "(��)�� �����!";
        queue.Enqueue(action);

        return queue;
    }
    public Queue<BattleAction> SkillDelLearn(Skill learnSkill, Pokemon pokemon)
    {
        Queue<BattleAction> queue = new Queue<BattleAction>();
        BattleAction action;

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = pokemon.Name + "(��)�� ���� " + learnSkill.SkillName + "(��)�� ���� �ʹ�...";
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = learnSkill.SkillName + " ��� �ٸ� ����� �ذ� �ϰڽ��ϱ�?";
        action.textSelect = true;
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILL_LEARN;
        action.learningSkill = learnSkill;
        action.pokemon = pokemons[0];
        queue.Enqueue(action);

        return queue;
    }
    public Queue<BattleAction> SkillDelLearn(string delSkillName, string learnSkillName, Pokemon pokemon)
    {
        Queue<BattleAction> queue = new Queue<BattleAction>();
        BattleAction action;

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = "1, 2...... §!";
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = pokemon.Name + "(��)�� " + delSkillName + "(��)�� �ؾ���! �׸���..";
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = pokemon.Name + "(��)�� ���� " + learnSkillName + "(��)�� �����!";
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILLSET;
        action.pokemon = pokemon;
        queue.Enqueue(action);

        return queue;
    }
    public Queue<BattleAction> SkillDelLearn(string learnSkillName, string pokeName)
    {
        Queue<BattleAction> queue = new Queue<BattleAction>();

        BattleAction action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = pokeName + "(��)�� " + learnSkillName + "(��)�� ����� �ʾҴ�!";
        queue.Enqueue(action);

        return queue;
    }

    public Queue<BattleAction> SetNextPoke(TARGET target, int index)
    {
        Queue<BattleAction> queue = new Queue<BattleAction>();

        BattleAction action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        if (target == TARGET.ME) action.battleText = "����! " + trainers[(int)target].TrainerPokemons[index].GetComponent<Pokemon>().Name + "!";
        else action.battleText = trainers[(int)target].Name + "��(��) " + trainers[(int)target].TrainerPokemons[index].GetComponent<Pokemon>().Name + "��(��) �����´�!";
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_ENTRANCE;
        action.target = target;
        action.entrancePokemon = trainers[(int)target].TrainerPokemons[index];
        queue.Enqueue(action);

        pokemons[(int)target] = trainers[(int)target].TrainerPokemons[index].GetComponent<Pokemon>();
        pastSkills[(int)target].Clear();
        superPoison[(int)target] = 0;

        action = new BattleAction();
        action.type = ACTION.POKEMON_SKILLSET;
        action.target = target;
        action.pokemon = pokemons[(int)target];
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.POKEMON_UISET;
        action.target = target;
        action.pokemon = pokemons[(int)target];
        action.curHP = pokemons[(int)target].Cur_HP;
        action.maxHP = pokemons[(int)target].Max_HP;
        action.statusEffect = pokemons[(int)target].StatusEffect;
        queue.Enqueue(action);

        return queue;
    }

    public Queue<BattleAction> LeavingPokemon(Pokemon leavingPokemon, Pokemon newPokemon)
    {
        Queue<BattleAction> queue = new Queue<BattleAction>();

        BattleAction action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = "���̹���, " + leavingPokemon.Name + "!";
        queue.Enqueue(action);

        action = new BattleAction();
        action.type = ACTION.BATTLE_TEXT;
        action.battleText = newPokemon.Name + "(��)�� ���� ���ᰡ �Ǿ���!";
        queue.Enqueue(action);

        return queue;
    }

    // true == �� ��ŵ��
    public bool InvokingCondition(Skill skill)
    {
        // not yet implemented
        return false;
    }

    private void SpecialEffect(string skillName, Pokemon attacker, Pokemon defender, Queue<BattleAction> queue)
    {
        // not yet implemented
    }

    private int GetAttack(Pokemon pokemon)
    {
        double burned = 1;

        if (pokemon.StatusEffect == STATUSEFFECT.BURN) { burned = 0.5; }

        int attack = (int)(burned * RankCalculator(pokemon.Attack, pokemon.Atk_Rank));

        return attack < 1 ? 1 : attack;
    }

    private int GetDefense(Pokemon pokemon)
    {
        int defense = (int)(RankCalculator(pokemon.Defense, pokemon.Def_Rank));

        return defense < 1 ? 1 : defense;
    }

    private int GetSpeed(Pokemon pokemon)
    {
        double paralysis = 1;

        if (pokemon.StatusEffect == STATUSEFFECT.PARALYSIS) { paralysis = 0.5; }

        int speed = (int)(paralysis * RankCalculator(pokemon.Speed, pokemon.Spd_Rank));

        return speed < 1 ? 1 : speed;
    }

    private double RankCalculator(int stat, int rank)
    {
        double rankMultiplier;

        if (rank > 6 || rank < -6) return 0; // error

        if (rank == 0) { rankMultiplier = 1; }
        else if (rank > 0) { rankMultiplier = (double)(rank + 2) / 2; } // ��ũ�� 0���� ���� �� ���ڿ� ����
        else { rankMultiplier = (double)2 /(2 - rank); } // ��ũ�� 0���� ���� �� �и� ����

        return rankMultiplier * stat;
    }

    public void PokemonDebug(Pokemon pokemon, string title)
    {
        Debug.Log(
            title + 
            "\npokemonName : " + pokemon.Name +
            "\nmax_HP : " + pokemon.Max_HP +
            "\ncur_HP : " + pokemon.Cur_HP +
            "\nattack : " + GetAttack(pokemon) +
            "\ndefense : " + GetDefense(pokemon) +
            "\nspeed : " + GetSpeed(pokemon) +
            "\natk_Rank : " + pokemon.Atk_Rank +
            "\ndef_Rank : " + pokemon.Def_Rank +
            "\nspd_Rank : " + pokemon.Spd_Rank +
            "\nStatusEffect : " + pokemon.StatusEffect +
            ""



            );
        //Debug.Log("level" + level);
        //Debug.Log("description" + description);
        //Debug.Log("attack" + attack);
        //Debug.Log("defense" + defense);
        //Debug.Log("speed" + speed);
        //Debug.Log("base_HP" + base_HP);
        //Debug.Log("base_ATK" + base_ATK);
        //Debug.Log("base_DEF" + base_DEF);
        //Debug.Log("base_SPD" + base_SPD);

        //Debug.Log("Skillname" + skills[0].SkillName);
    }

    public void PokemonDebug()
    {
        for (int i = 0; i < 2; i++) PokemonDebug(pokemons[i], ((TURN)i).ToString());
    }

    public Pokemon GetPlayerPokemon() { return pokemons[0]; }
    public Pokemon GetEnemyPokemon() { return pokemons[1]; }
}