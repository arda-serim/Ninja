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

    int jumpsLeft;
    bool onGround;
    float multiplier;

    void Awake()
    {
        GameManager.Instance.startGame += () => this.enabled = true;

        speed = 7;
        score = 0;
        stamina = 100;

        playerController = new PlayerController();
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();

        jumpsLeft = 2;
        onGround = true;

        playerController.Gameplay.Jump.performed += ctx => { if (jumpsLeft > 0 && stamina > 6 && !animator.GetBool("IsSliding") && !animator.GetBool("IsAttacking")) JumpCalller(); };

        playerController.Gameplay.Slide.performed += ctx => SlideCaller();

        playerController.Gameplay.Attack.performed += ctx => AttackCaller();
    }

    void Update()
    {
        IncreaseScore();
        InformUI();

        rb.velocity = new Vector3(speed * transform.right.x, rb.velocity.y);
        animator.SetFloat("VelocityY", rb.velocity.y);

        if (onGround && !animator.GetBool("IsSliding") && !animator.GetBool("IsSliding"))
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }
        StaminaRegenarater();
    }

    /// <summary>
    /// Jump
    /// </summary>
    IEnumerator Jump()
    {
        if (jumpsLeft == 1)
        {
            animator.SetBool("DoubleJump", true);
        }
        stamina -= 7.7f;
        jumpsLeft -= 1;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 300));
        if (jumpsLeft == 0)
        {
            yield return new WaitForSeconds(0.45f);
            animator.SetBool("DoubleJump", false);
        }
    }

    /// <summary>
    /// Just call Jump()
    /// </summary>
    void JumpCalller()
    {
        StartCoroutine(Jump());
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
            yield return new WaitForSeconds(0.55f);
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
        if (!animator.GetBool("IsAttacking") && stamina > 20 && !animator.GetBool("IsSliding") && !animator.GetBool("DoubleJump"))
        {
            stamina -= 18;
            animator.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(0.7f);
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            jumpsLeft = 2;
        }

        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
        {
            rb.freezeRotation = false;
            animator.enabled = false;
            this.enabled = false;
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

        if (collider.gameObject.CompareTag("EnemyWeapon"))
        {
            Physics2D.IgnoreCollision(gameObject.transform.GetChild(0).gameObject.GetComponent<Collider2D>(), collider.transform.root.GetComponent<Collider2D>());
            rb.freezeRotation = false;
            animator.enabled = false;
            this.enabled = false;
        }

        if (collider.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collider.gameObject);
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
