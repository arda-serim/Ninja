using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : MonoBehaviour
{
    int speed;
    int score;
    [SerializeField] float stamina;

    Rigidbody2D rb;
    Animator animator;
    PlayerController playerController;
    EdgeCollider2D edgeCollider2D;

    [SerializeField]bool isJumping;
    [SerializeField]bool onGround;
    void Awake()
    {
        speed = 2;
        score = 0;
        stamina = 100;

        playerController = new PlayerController();
        animator = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        edgeCollider2D = gameObject.GetComponent<EdgeCollider2D>();

        isJumping = false;

        playerController.Gameplay.Jump.started += ctx => { isJumping = true; };
        playerController.Gameplay.Jump.performed += ctx => { isJumping = false; onGround = false; };
        playerController.Gameplay.Jump.canceled += ctx => { isJumping = false; onGround = false; };

        playerController.Gameplay.Slide.performed += ctx => SlideCaller();

        playerController.Gameplay.Attack.performed += ctx => AttackCaller();
    }

    void Update()
    {
        rb.velocity = new Vector3(speed, rb.velocity.y);

        Jump();
        StaminaRegenarater();
    }

    /// <summary>
    /// Jump depending on how long press Jump Key (If do not stop pressing, stop automaticly)
    /// </summary>
    void Jump()
    {
        if (isJumping && onGround && stamina > 0.5)
        {
            stamina -= 0.125f;
            rb.velocity = new Vector2(rb.velocity.x, 7);
        }
    }

    /// <summary>
    /// Make the "Slide Animation"
    /// </summary>
    IEnumerator Slide()
    {
        if (!animator.GetBool("Slide") && stamina > 10)
        {
            stamina -= 10;
            animator.SetBool("Slide", true);
            yield return new WaitForSeconds(0.75f);
            animator.SetBool("Slide", false);
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
        if (!animator.GetBool("Attack") && stamina > 20)
        {
            stamina -= 20;
            animator.SetBool("Attack", true);
            yield return new WaitForSeconds(0.75f);
            animator.SetBool("Attack", false);
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
        if (stamina <= 99.585f && !animator.GetBool("Attack") && onGround && !animator.GetBool("Slide"))
        {
            stamina += 0.415f;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground");
            onGround = true;
        }
    }

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
