using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneUpdater
{
    public static SceneUpdater instance = new SceneUpdater();
    public static SceneUpdater Instance { get { return instance; } }

    public List<EnemyDefeted> EnemyDefetedInfos { get; private set; }

    private SceneUpdater() 
    {
        EnemyDefetedInfos = new List<EnemyDefeted>();
    }

    public void AddEnemyDefededInfo(EnemyDefeted enemyDefeted)
    {
        EnemyDefetedInfos.Add(enemyDefeted);
    }    
}

public struct EnemyDefeted
{
    public int enemyNum;
    public bool Defeted;

    public EnemyDefeted(int enemyNum, bool defeted)
    {
        this.enemyNum = enemyNum;
        this.Defeted = defeted;
    }
}

