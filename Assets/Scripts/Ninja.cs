using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    int speed;
    float score;
    float stamina;

    Rigidbody2D rb;
    Animator animator;
    PlayerController playerController;

    bool isJumping;
    float timerForJump;
    bool onGround;
    float multiplier;

    void Awake()
    {
        GameManager.Instance.startGame += () => this.enabled = true;

        speed = 2;
        score = 0;
        stamina = 100;

        playerController = new PlayerController();
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        isJumping = false;
        onGround = true;
        timerForJump = 0.3f;

        playerController.Gameplay.Jump.started += ctx => { if (onGround) isJumping = true; };
        playerController.Gameplay.Jump.canceled += ctx => { isJumping = false; timerForJump = 0.3f; };

        playerController.Gameplay.Slide.performed += ctx => SlideCaller();

        playerController.Gameplay.Attack.performed += ctx => AttackCaller();
    }

    void Update()
    {
        IncreaseScore();
        InformUI();

        rb.velocity = new Vector3(speed, rb.velocity.y);

        Jump();
        StaminaRegenarater();
    }

    /// <summary>
    /// Jump depending on how long press Jump Key (If do not stop pressing, stop automaticly)
    /// </summary>
    void Jump()
    {
        if (isJumping && timerForJump > 0 && stamina > 6 && !animator.GetBool("IsSliding") && !animator.GetBool("IsAttacking"))
        {
            timerForJump -= Time.deltaTime;
            stamina -= 0.125f;
            rb.velocity = new Vector2(rb.velocity.x, 7);
        }
    }

    /// <summary>
    /// Make the "Slide Animation"
    /// </summary>
    IEnumerator Slide()
    {
        if (onGround && !animator.GetBool("IsSliding") && stamina > 10 && !animator.GetBool("IsAttacking"))
        {
            stamina -= 10;
            animator.SetBool("IsSliding", true);
            yield return new WaitForSeconds(0.75f);
            animator.SetBool("IsSliding", false);
        }
    }

    /// <summary>
    /// Just call Slide()
    /// </summary>
    void SlideCaller()
    {
        StartCoroutine(Slide());
    }

    /// <summary>
    /// Make the "Attack Animation" and decrease stamina everytime
    /// </summary>
    IEnumerator Attack()
    {
        if (!animator.GetBool("IsAttacking") && stamina > 20 && !animator.GetBool("IsSliding"))
        {
            stamina -= 18;
            animator.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(0.75f);
            animator.SetBool("IsAttacking", false);
        }
    }

    /// <summary>
    /// Just call Attack()
    /// </summary>
    void AttackCaller()
    {
        StartCoroutine(Attack());
    }

    /// <summary>
    /// Regenerate stamina when doing nothing require stamina
    /// </summary>
    void StaminaRegenarater()
    {
        if (stamina <= 99.8f && !animator.GetBool("IsAttacking") && onGround && !animator.GetBool("IsSliding"))
        {
            if (multiplier < 1)
            {
                multiplier += Time.deltaTime;
            }
            stamina += 0.2f * multiplier;
        }
        else
        {
            multiplier = 0;
        }
    }

    /// <summary>
    /// Send information to UIManager
    /// </summary>
    void InformUI()
    {
        UIManager.Instance.stamina = this.stamina;
        UIManager.Instance.score = this.score;
    }
    
    /// <summary>
    /// Increase score overtime
    /// </summary>
    void IncreaseScore()
    {
        score += Time.deltaTime * 168;
    }

    #region OnCollisionStay2D / OnCollisionExit2D
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }        
    }
    #endregion

    #region OnTriggerEnter2D
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            score += 5000;
        }
    }
    #endregion

    #region OnEnable / OnDisable
    private void OnEnable()
    {
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }
    #endregion
}
