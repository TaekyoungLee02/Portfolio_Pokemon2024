using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    public static ItemManager instance = new();

    public static ItemManager Instance {  get { return instance; } }
    public List<ItemInfo> ItemInfos { get; private set; }

    public ItemManager() 
    {
        ItemInfos = new List<ItemInfo>();
        var infos = Resources.LoadAll("Items");

        foreach (var info in infos)
        {
            if(info is ItemInfo) ItemInfos.Add((ItemInfo)info);
        }
    }

    public List<Item> CreateBagFromTrainerBagInfo(List<TrainerBagInfo> trainerBagInfos)
    {
        var bag = new List<Item>();

        foreach(var info in trainerBagInfos)
        {
            bag.Add(new Item(ItemInfos[info.itemNum], info.amount));
        }

        return bag;
    }
}
