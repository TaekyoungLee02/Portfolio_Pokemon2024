using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerManager : MonoBehaviour
{
    public GameObject trainerPositions;
    public Transform playerPosition;
    public List<TrainerInfo> TrainerInfos { get; private set; }
    public List<GameObject> Trainers { get; private set; }
    private List<GameObject> enemies;
    private SceneUpdater sceneUpdater;

    private ItemManager itemManager;

    private void Start()
    {
        Init();
        CreateTrainers();
    }


    private void Init()
    {
        Trainers = new List<GameObject>();
        TrainerInfos = new List<TrainerInfo>();
        enemies = new List<GameObject>();
        sceneUpdater = SceneUpdater.Instance;
        itemManager = ItemManager.Instance;

        Object[] tempInfos = Resources.LoadAll("Trainers/Enemies");

        foreach (TrainerInfo info in tempInfos)
        {
            TrainerInfos.Add(info);
        }
    }

    private void CreateTrainers()
    {
        if (trainerPositions.transform.childCount == 0) return;

        for(int i = 0; i < trainerPositions.transform.childCount; i++)
        {
            var pos = trainerPositions.transform.GetChild(i).position;
            var rot = trainerPositions.transform.GetChild(i).rotation;

            var trainer = Instantiate(TrainerInfos[i].TrainerGameObject, pos, rot);
            enemies.Add(trainer);

            // Set Enemy Tag and Layer
            SetEnemyLayer(trainer, LayerMask.NameToLayer("Enemy"));
            trainer.tag = "Enemy";

            // Set Enemy Component
            trainer.AddComponent<Enemy>().Init(TrainerInfos[i]);
            var enemyInfos = sceneUpdater.EnemyDefetedInfos;
            foreach (var info in enemyInfos)
                if(info.enemyNum == TrainerInfos[i].enemyNum && info.Defeted == true) { trainer.GetComponent<Enemy>().isDefeted = true; }

            // Set Enemy Bag
            trainer.GetComponent<Enemy>().Bag = itemManager.CreateBagFromTrainerBagInfo(TrainerInfos[i].trainerBagInfos);

            // Set Enemy Rigidbody
            trainer.AddComponent<Rigidbody>();
            trainer.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        for (int i = 0; i < enemies.Count; i ++)
        {
            if (!enemies[i].GetComponent<Enemy>().isDefeted) return;
        }

        Application.Quit();
    }

    private void SetEnemyLayer(GameObject enemy, int layer)
    {
        enemy.layer = layer;
        
        foreach(Transform child in enemy.transform)
        {
            SetEnemyLayer(child.gameObject, layer);
        }
    }
}
