using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMove : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform playerBody;
    NavMeshAgent navMeshAgent;
    Vector3 targetVector;
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerDestination();
    }

    private void GetPlayerDestination()
    {
        targetVector = playerBody.transform.position;
        navMeshAgent.SetDestination(targetVector);
    }
}
