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

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    Vector3 velocity;

    [SerializeField] private Animator _punch = null;
    [SerializeField] private Animator _kick = null;


    //Dodging variables
    public float DelayBeforeInvinsible = 0.2f;
    public float InvinsibleDuration = 0.5f;

    public float DodgeCoolDown = 1;
    private float ActCoolDown;

    public float PushAmt = 3;

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
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Dodge();
        }

        Attacks();
    }

    void Dodge()
    {
        _dodgeCollider.transform.position = controller.transform.position;
        Debug.Log("DODGED");
        Vector3 WTPosition = controller.transform.position;
        ActCoolDown = DodgeCoolDown;
        controller.Move(velocity * Time.deltaTime*PushAmt);

    }

    void Attacks()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Punch");
            if (isActiveAndEnabled)
            {
                _punch.SetBool("Punching", Input.GetKey(KeyCode.W));
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Kick");
            _kick.Play("Kicking", 0, 0.0f);
        }
    }
}
