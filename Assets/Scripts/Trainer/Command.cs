using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Command
{
    public enum TYPE
    {
        NONE,
        SKILL,
        ITEM,
        POKEMON,
        RUN
    }

    public TYPE type;

    /// <summary>
    /// if type is item, number is bag number
    /// </summary>
    public int number;
}