using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public Transform target;
    public Collider _playerDodgeCollider;

    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInChildren < UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     if(target != null)
        {
            agent.SetDestination(target.position);
            if (agent.remainingDistance<1.5f)
            {
                anim.SetTrigger("EnemyAttack");
                if (Vector3.Distance(_playerDodgeCollider.transform.position, this.transform.position) < 1.5f)
                {
                    DidIDodge();
                }
            }
        }   
    }

    bool DidIDodge()
    {
        if (Vector3.Distance(_playerDodgeCollider.transform.position, this.transform.position) < 1.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
}
