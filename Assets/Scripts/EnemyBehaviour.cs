using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private NavMeshAgent enemyNavMesh;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float followDistance;
    private Vector3 originalPosition;
    private bool isFollowingPlayer = false;

    private void Awake()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= followDistance)
        {
            enemyNavMesh.SetDestination(playerTransform.position);
            isFollowingPlayer = true;
        }
        else
        {
            if (isFollowingPlayer)
            {
                enemyNavMesh.ResetPath();
                isFollowingPlayer = false;
            }
        }
    }
}
