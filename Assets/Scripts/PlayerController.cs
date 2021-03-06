using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    [SerializeField] float _speed = 6f;
    [SerializeField] float _turnSmoothTime = 0.1f;
    float _turnSmoothVelocity;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] Collider _dodgeCollider;
    [SerializeField] GameObject _dodgeColliderReturn;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    Vector3 velocity;

    //Dodging variables
    public float DelayBeforeInvinsible = 0.2f;
    public float InvinsibleDuration = 0.5f;

    public float DodgeCoolDown = 1;
    private float ActCoolDown;

    public float PushAmt = 3;

    Animator anim;

    AudioSource audioSource;
    public AudioClip hit;

    [SerializeField] Collider enemyCollider;
    [SerializeField] Collider playerFist;
    public static bool enemyHit = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _dodgeCollider.transform.position = _dodgeColliderReturn.transform.position;
        _dodgeCollider.enabled = false;
        audioSource = GetComponent<AudioSource>();
        playerFist.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool Roll = Input.GetKeyDown(KeyCode.LeftShift);
        if(ActCoolDown <= 0)
        {
            if (Roll)
            {
                Dodge();
            }
        }
        else
        {
            ActCoolDown -= Time.deltaTime;
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * _speed * Time.deltaTime);
            anim.SetTrigger("WalkingForward");
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dodge();
        }

        Attacks();

        if (Enemy.playerHit)
        {
            anim.SetTrigger("Hit");
            Enemy.playerHit = false;
        }
    }

    void Dodge()
    {
        _dodgeCollider.enabled = true;
        _dodgeCollider.transform.position = controller.transform.position;
        Debug.Log("DODGED");
        Vector3 WTPosition = controller.transform.position;
        ActCoolDown = DodgeCoolDown;
        controller.Move(velocity * Time.deltaTime*PushAmt);
        anim.SetTrigger("Dodging");

        StartCoroutine(DodgeReset());
    }

    void Attacks()
    {
        playerFist.enabled = true;
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Punch");
            anim.SetTrigger("Punching");
            audioSource.PlayOneShot(hit);
            if (Vector3.Distance(enemyCollider.transform.position, playerFist.transform.position) < 1.5f)
            {
                enemyHit = true;
            }
            StartCoroutine(Hit());

        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Kick");
            anim.SetTrigger("Kicking");
            audioSource.PlayOneShot(hit);
            if (Vector3.Distance(enemyCollider.transform.position, playerFist.transform.position) < 1.5f)
            {
                enemyHit = true;
            }
            StartCoroutine(Hit());
        }
    }

    private IEnumerator DodgeReset()
    {
        yield return new WaitForSeconds(1);
        _dodgeCollider.transform.position = _dodgeColliderReturn.transform.position;
        _dodgeCollider.enabled = false;
    }
    private IEnumerator Hit()
    {
        yield return new WaitForSeconds(2);
        playerFist.enabled = false;
    }
}
