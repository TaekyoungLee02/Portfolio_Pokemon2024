using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildPokemonSpawner : MonoBehaviour
{
    public int pokemonNum;
    public float spawningInterval;
    public float wanderingRange;
    public int spawnMax;
    public int minLevel;
    public int maxLevel;
    public float offset;

    public float encounterRadius;

    private PokemonManager manager;
    private List<GameObject> spawnList;

    // Start is called before the first frame update
    void Start()
    {
        manager = PokemonManager.Instance;
        spawnList = new List<GameObject>();

        for (int i = 0; i < spawnMax; i++)
        {
            spawnList.Add(manager.InstantiatePokemon(pokemonNum, transform.position, transform.rotation, Random.Range(minLevel, maxLevel + 1)));
            InitWildPoke(i);
        }

        StartCoroutine(SpawnPokemon());
    }


    private void InitWildPoke(int index)
    {
        spawnList[index].SetActive(false);
        spawnList[index].AddComponent<WildPokemon>().Init(transform, wanderingRange, encounterRadius, offset);
        spawnList[index].GetComponent<WildPokemon>().StopWandering();
    }

    private IEnumerator SpawnPokemon()
    {
        for (int i = 0; i < spawnMax; i++)
        {
            spawnList[i].SetActive(true);
            yield return new WaitForSeconds(spawningInterval);
        }
    }
}
