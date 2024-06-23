using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WildPokemon : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform spawnPosition;
    private float wanderingRadius;
    private IEnumerator wandering;
    private IEnumerator chasing;
    private bool isWandering;
    private float offset;

    private int isWalking;
    private Animator animator;

    public float encounterRadius;
    private int layerMask;
    private int colliderCount;
    private Collider[] playerCollider;

    private void Awake()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.angularSpeed = 240f;
        agent.baseOffset = offset;
        agent.enabled = false;

        animator = GetComponent<Animator>();
        isWalking = Animator.StringToHash("isWalking");

        playerCollider = new Collider[1];
        layerMask = (int)LAYER.PLAYER;
    }

    private void Update()
    {
        colliderCount = Physics.OverlapSphereNonAlloc(transform.position, encounterRadius, playerCollider, layerMask);

        if (colliderCount != 0)
        {
            if (isWandering) ChasePlayer(playerCollider[0].transform);
        }
        else
        {
            if (!isWandering) StartWandering();
        }
    }

    private void OnEnable()
    {
        StartWandering();
    }

    public void Init(Transform spawnPosition, float wanderingRadius, float encounterRadius, float offset)
    {
        this.spawnPosition = spawnPosition;
        this.wanderingRadius = wanderingRadius;
        this.encounterRadius = encounterRadius;
        this.offset = offset;
    }

    public void StartWandering()
    {
        isWandering = true;

        agent.enabled = true;
        agent.speed = 5f;

        if(chasing !=  null) StopCoroutine(chasing);

        wandering = Wandering();
        StartCoroutine(wandering);
    }

    public void StopWandering()
    {
        if (wandering != null)
        {
            StopCoroutine(wandering);
            isWandering = false;
        }
    }
    public void StopChasing()
    {
        if (chasing != null)
        {
            StopCoroutine(chasing);
        }
    }

    public void ChasePlayer(Transform player)
    {
        isWandering = false;

        agent.speed = 15f;

        if (wandering != null) StopCoroutine(wandering);

        chasing = Chase(player);
        StartCoroutine(chasing);
    }

    private IEnumerator Wandering()
    {
        while (true)
        {
            float x = Random.Range(-wanderingRadius, wanderingRadius);

            int randomseed = Random.Range(0, 2) == 0 ? -1 : 1;
            float z = Mathf.Sqrt(wanderingRadius * wanderingRadius - x * x) * randomseed;

            float randomRadius = Random.Range(0, 1f);


            Vector3 randomPos = new Vector3(x, 0, z) * randomRadius;
            Vector3 finalDest = randomPos + spawnPosition.position;
            finalDest.y = transform.position.y;

            float distance = Vector3.Distance(finalDest, transform.position);

            animator.SetBool(isWalking, true);
            while (distance > 2)
            {
                agent.SetDestination(finalDest);
                distance = Vector3.Distance(finalDest, transform.position);

                yield return null;
            }
            animator.SetBool(isWalking, false);

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator Chase(Transform player)
    {
        float distance = Vector3.Distance(player.position, transform.position);
        animator.SetBool(isWalking, true);

        while (distance > 1) 
        {
            agent.SetDestination(player.position);
            distance = Vector3.Distance(player.position, transform.position);

            yield return null;
        }
        animator.SetBool(isWalking, false);
    }
}

