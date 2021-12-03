using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }
    public Transform target;
    public Collider _playerDodgeCollider;
    public Collider _enemyAttackCollider;
    public Collider _playerCollider;

    Animator anim;
    public static bool dodged = false;
    public static bool playerHit = false;

    AudioSource audioSource;
    public AudioClip hit;
    bool alreadyPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInChildren < UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = true;

        target = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponent<Animator>();

        _enemyAttackCollider.enabled = false;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dodged == true)
        {
            StartCoroutine(WitchTime());
        }

        else
        {
            transform.LookAt(target);
            if (target != null)
            {
                agent.SetDestination(target.position);
                if (agent.remainingDistance < 3f)
                {
                    _enemyAttackCollider.enabled = true;
                    anim.SetTrigger("EnemyAttack");
                    StartCoroutine(Attack());

                    if (Vector3.Distance(_playerDodgeCollider.transform.position, this.transform.position) < 3f)
                    {
                        DidIDodge();
                        if (Vector3.Distance(_playerCollider.transform.position, _enemyAttackCollider.transform.position) < 1.5f)
                        {
                            playerHit = true; 
                        }
                    }
                }
                else
                {
                    anim.SetTrigger("Walking");
                }
            }
            if (PlayerController.enemyHit)
            {
                anim.SetTrigger("Hit");
            }

        }
    }

    bool DidIDodge()
    {
        if (Vector3.Distance(_playerDodgeCollider.transform.position, _enemyAttackCollider.transform.position) < 1.5f)
        {
            Debug.Log("Slow Time");
            dodged = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator WitchTime()
    {
        anim.enabled = false;
        yield return new WaitForSeconds(5);
        dodged = false;
        anim.enabled = true;
    }

    private IEnumerator Attack()
    {
        if (!alreadyPlayed)
        {
            audioSource.PlayOneShot(hit);
            alreadyPlayed = true;
        }
        yield return new WaitForSeconds(2);
        _enemyAttackCollider.enabled = false;
        alreadyPlayed = false;
    }
}
