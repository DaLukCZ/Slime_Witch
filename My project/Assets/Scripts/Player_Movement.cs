using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private EdgeCollider2D edgeColl;
    private SpriteRenderer spriteR;

    private float m_dirX = 0f;
    const float k_GroundedRadius = 0.51f;
    public bool m_FacingRight = true;
                               //   0                  1                2          3        4        5       6       7     8     9 
    private enum MovementState {abilitySpike, abilityPoweredBall, abilityShield, attack, attackUP, death, ghostRun, hurt, idle, run}
    private bool isGhost;
    private bool m_Grounded;
    private bool m_moving;
    private bool m_alive;

    public int maxHP = 100;
    public int currentHP;
    public int armor = 0;
    [Range(10, 15)] public float m_jumpForce = 14f;
    [Range(5, 15)] public float m_movementSpeed = 7f;
    public LayerMask m_WhatIsGround;                 
    public Transform m_GroundCheck;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        edgeColl = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementState state;
        m_dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(m_dirX * m_movementSpeed, rb.velocity.y);

        if (Input.GetButton("Jump") && m_Grounded)
        {
            m_moving = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x, m_jumpForce);
            m_Grounded = false;
        }

            UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }
    private void Awake()
    {
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;
        if (!ghost()) { 
            if (m_dirX > 0f && !m_FacingRight)
            {
                Flip();
                state = MovementState.run;
                m_moving = true;
            }
            else if (m_dirX < 0f && m_FacingRight)
            {
                Flip();
                state = MovementState.run;
                m_moving = true;
            }
            else if (m_dirX == 0f)
            {
                state = MovementState.idle;
                m_moving = false;
            }
            animator.SetInteger("state", (int)state);
        }
        else
        {
            animator.SetInteger("state", 6);
        }
         
    }

    private bool ghost()
    {
        MovementState state;
        if (isGhost)
        {
            state = MovementState.ghostRun;
            edgeColl.isTrigger = true;
            return true;
        }
        else
        {
            state = MovementState.idle;
            edgeColl.isTrigger = false;
            return false;
        }
    }

    private void fire()
    {
        Debug.Log("SHOTTED");
    }

    private void fireUP()
    {

    }

    private void abilityCTRL()
    {

    }

    private void abilitySHIFT()
    {

    }

    private void getHit(int dmg)
    {
        MovementState state;
        currentHP -= dmg;
        death();
        if (m_alive)
        {
            state = MovementState.hurt;
        }
    }

    private void heal(int heal)
    {
        if (currentHP >= maxHP && currentHP <= 0)
        {
            //Cant heal
        }
        else if((currentHP + heal) > maxHP)
        {
            currentHP = maxHP;
        }
        else
        {
            currentHP += heal;
        }
    }

    private void death()
    {
        MovementState state;
        if (currentHP <= 0)
        {
            m_alive = false;
            state = MovementState.death;
        }
        else
        {
            m_alive = true;
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
