using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public Transform target;
    void Start()
    {
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);

            if(agent.remainingDistance< 1.5f)
            {
                GetComponent<Animator>().SetTrigger("EnemyAttack");
            }
        }

       // if (agent.remainingDistance > agent.stoppingDistance)
         //   CharacterController.Move(agent.desiredVelocity, false, false);
    }
}
