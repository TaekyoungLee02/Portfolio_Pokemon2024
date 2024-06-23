
using UnityEngine;

public enum TYPE
{
    NONE,
    NORMAL,
    FIRE,
    WATER,
    ELECTRIC,
    GRASS,
    ICE,
    FIGHTING,
    POISON,
    GROUND,
    FLYING,
    PSYCHIC,
    BUG,
    ROCK,
    GHOST,
    DRAGON,
    DARK,
    STEEL,
    FAIRY,
}

public class Type
{
    private static Type instance = new Type();
    private Sprite[] typeSprites;

    private Type() 
    {
        typeSprites = Resources.LoadAll<Sprite>("Sprites/Types");
    }

    public static Type Instance
    {
        get { return instance; }
    }

    public bool SameType(Pokemon pokemon, Skill skill)
    {
        foreach (var type in pokemon.Types)
        {
            if (type == skill.Type) return true;
        }
        return false;
    }

    public double TypeCalc(TYPE skill, TYPE[] defender)
    {
        double result = 1;

        foreach (var type in defender)
        {
            switch (skill) 
            {
                case TYPE.NORMAL:
                    result *= Normal(type); break;

                case TYPE.FIRE:
                    result *= Fire(type); break;

                case TYPE.WATER:
                    result *= Water(type); break;

                case TYPE.ELECTRIC:
                    result *= Electric(type); break;

                case TYPE.GRASS:
                    result *= Grass(type); break;

                case TYPE.ICE:
                    result *= Ice(type); break;

                case TYPE.FIGHTING:
                    result *= Fighting(type); break;

                case TYPE.POISON:
                    result *= Poison(type); break;

                case TYPE.GROUND:
                    result *= Ground(type); break;

                case TYPE.FLYING:
                    result *= Flying(type); break;

                case TYPE.PSYCHIC:
                    result *= Psychic(type); break;

                case TYPE.BUG:
                    result *= Bug(type); break;

                case TYPE.ROCK:
                    result *= Rock(type); break;

                case TYPE.GHOST:
                    result *= Ghost(type); break;

                case TYPE.DRAGON:
                    result *= Dragon(type); break;

                case TYPE.DARK:
                    result *= Dark(type); break;

                case TYPE.STEEL:
                    result *= Steel(type); break;

                case TYPE.FAIRY:
                    result *= Fairy(type); break;
            }
        }

        return result;
    }

    private double Normal(TYPE defender)
    {
        switch (defender) 
        {
            case TYPE.ROCK:
                return 0.5;

            case TYPE.GHOST:
                return 0;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Fire(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 0.5;

            case TYPE.WATER:
                return 0.5;

            case TYPE.GRASS:
                return 2;

            case TYPE.ICE:
                return 2;

            case TYPE.BUG:
                return 2;

            case TYPE.ROCK:
                return 0.5;

            case TYPE.DRAGON:
                return 0.5;

            case TYPE.STEEL:
                return 2;

            default:
                return 1;
        }
    }

    private double Water(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 2;

            case TYPE.WATER:
                return 0.5;

            case TYPE.GRASS:
                return 0.5;

            case TYPE.GROUND:
                return 2;

            case TYPE.ROCK:
                return 2;

            case TYPE.DRAGON:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Grass(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 0.5;

            case TYPE.WATER:
                return 2;

            case TYPE.GRASS:
                return 0.5;

            case TYPE.POISON:
                return 0.5;

            case TYPE.GROUND:
                return 2;

            case TYPE.FLYING:
                return 0.5;

            case TYPE.BUG:
                return 0.5;

            case TYPE.ROCK:
                return 2;

            case TYPE.DRAGON:
                return 0.5;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Electric(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.WATER:
                return 2;

            case TYPE.GRASS:
                return 0.5;

            case TYPE.ELECTRIC:
                return 0.5;

            case TYPE.GROUND:
                return 0;

            case TYPE.FLYING:
                return 2;

            case TYPE.DRAGON:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Ice(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 0.5;

            case TYPE.WATER:
                return 0.5;

            case TYPE.GRASS:
                return 2;

            case TYPE.ICE:
                return 0.5;

            case TYPE.GROUND:
                return 2;

            case TYPE.FLYING:
                return 2;

            case TYPE.DRAGON:
                return 2;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Fighting(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.NORMAL:
                return 2;

            case TYPE.ICE:
                return 2;

            case TYPE.POISON:
                return 0.5;

            case TYPE.FLYING:
                return 0.5;

            case TYPE.PSYCHIC:
                return 0.5;

            case TYPE.BUG:
                return 0.5;

            case TYPE.ROCK:
                return 2;

            case TYPE.GHOST:
                return 0;

            case TYPE.DARK:
                return 2;

            case TYPE.STEEL:
                return 2;

            case TYPE.FAIRY:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Poison(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.GRASS:
                return 2;

            case TYPE.POISON:
                return 0.5;

            case TYPE.GROUND:
                return 0.5;

            case TYPE.ROCK:
                return 0.5;
                
            case TYPE.GHOST:
                return 0.5;

            case TYPE.STEEL:
                return 0;

            case TYPE.FAIRY:
                return 2;

            default:
                return 1;
        }
    }

    private double Ground(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 2;

            case TYPE.GRASS:
                return 0.5;

            case TYPE.ELECTRIC:
                return 2;

            case TYPE.POISON:
                return 2;

            case TYPE.FLYING:
                return 0;

            case TYPE.BUG:
                return 0.5;

            case TYPE.ROCK:
                return 2;

            case TYPE.STEEL:
                return 2;

            default:
                return 1;
        }
    }

    private double Flying(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.GRASS:
                return 2;

            case TYPE.ELECTRIC:
                return 0.5;

            case TYPE.FIGHTING:
                return 2;

            case TYPE.BUG:
                return 2;

            case TYPE.ROCK:
                return 0.5;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Psychic(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIGHTING:
                return 2;

            case TYPE.POISON:
                return 2;

            case TYPE.PSYCHIC:
                return 0.5;

            case TYPE.DARK:
                return 0;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Bug(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 0.5;

            case TYPE.GRASS:
                return 2;

            case TYPE.FIGHTING:
                return 0.5;

            case TYPE.POISON:
                return 0.5;

            case TYPE.FLYING:
                return 0.5;

            case TYPE.PSYCHIC:
                return 2;

            case TYPE.GHOST:
                return 0.5;

            case TYPE.DARK:
                return 2;

            case TYPE.STEEL:
                return 0.5;

            case TYPE.FAIRY:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Rock(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 2;

            case TYPE.ICE:
                return 2;

            case TYPE.FIGHTING:
                return 0.5;

            case TYPE.GROUND:
                return 0.5;

            case TYPE.FLYING:
                return 2;

            case TYPE.BUG:
                return 2;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Ghost(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.NORMAL:
                return 0;

            case TYPE.PSYCHIC:
                return 2;

            case TYPE.GHOST:
                return 2;

            case TYPE.DARK:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Dragon(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.DRAGON:
                return 2;

            case TYPE.STEEL:
                return 0.5;

            case TYPE.FAIRY:
                return 0;

            default:
                return 1;
        }
    }

    private double Dark(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIGHTING:
                return 0.5;

            case TYPE.PSYCHIC:
                return 2;

            case TYPE.GHOST:
                return 2;

            case TYPE.DARK:
                return 0.5;

            case TYPE.FAIRY:
                return 0.5;

            default:
                return 1;
        }
    }

    private double Steel(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 0.5;

            case TYPE.WATER:
                return 0.5;

            case TYPE.ELECTRIC:
                return 0.5;

            case TYPE.ICE:
                return 2;

            case TYPE.ROCK:
                return 2;

            case TYPE.STEEL:
                return 0.5;

            case TYPE.FAIRY:
                return 2;

            default:
                return 1;
        }
    }

    private double Fairy(TYPE defender)
    {
        switch (defender)
        {
            case TYPE.FIRE:
                return 0.5;

            case TYPE.FIGHTING:
                return 2;

            case TYPE.POISON:
                return 0.5;

            case TYPE.DRAGON:
                return 2;

            case TYPE.DARK:
                return 2;

            case TYPE.STEEL:
                return 0.5;

            default:
                return 1;
        }
    }

    public Color ToColor(TYPE type, float alpha = 0.6f)
    {
        switch (type)
        {
            case TYPE.NORMAL:
                return new Color(0.631f, 0.631f, 0.631f, alpha);

            case TYPE.FIRE:
                return new Color(0.831f, 0.227f, 0.188f, alpha);

            case TYPE.WATER:
                return new Color(0.298f, 0.475f, 0.737f, alpha);

            case TYPE.ELECTRIC:
                return new Color(0.949f, 0.765f, 0.255f, alpha);

            case TYPE.GRASS:
                return new Color(0.365f, 0.616f, 0.235f, alpha);

            case TYPE.ICE:
                return new Color(0.471f, 0.8f, 0.941f, alpha);

            case TYPE.FIGHTING:
                return new Color(0.941f, 0.533f, 0.2f, alpha);

            case TYPE.POISON:
                return new Color(0.427f, 0.294f, 0.592f, alpha);

            case TYPE.GROUND:
                return new Color(0.537f, 0.322f, 0.161f, alpha);

            case TYPE.FLYING:
                return new Color(0.561f, 0.722f, 0.894f, alpha);

            case TYPE.PSYCHIC:
                return new Color(0.863f, 0.302f, 0.475f, alpha);

            case TYPE.BUG:
                return new Color(0.584f, 0.631f, 0.207f, alpha);

            case TYPE.ROCK:
                return new Color(0.678f, 0.663f, 0.518f, alpha);

            case TYPE.GHOST:
                return new Color(0.42f, 0.259f, 0.431f, alpha);

            case TYPE.DRAGON:
                return new Color(0.298f, 0.376f, 0.663f, alpha);

            case TYPE.DARK:
                return new Color(0.306f, 0.251f, 0.247f, alpha);

            case TYPE.STEEL:
                return new Color(0.455f, 0.635f, 0.725f, alpha);

            case TYPE.FAIRY:
                return new Color(0.729f, 0.498f, 0.71f, alpha);

            default:
                return new Color(0, 0, 0);
        }
    }

    public Sprite GetSprite(TYPE type)
    {
        return typeSprites[(int)type];
    }
}