    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int speed;

    Rigidbody2D rb;
    Animator animator;
    void Awake()
    {
        speed = 5;

        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        rb.velocity = new Vector2(speed * transform.right.x, rb.velocity.y);
        animator.SetFloat("VelocityY", rb.velocity.y);

        Debug.DrawRay(transform.position, transform.right);

        if (Physics2D.Raycast(transform.position, transform.right, 2,1 << LayerMask.NameToLayer("EnemyAttack")))
        {
            StartCoroutine(Attack());
        }
        if (Physics2D.Raycast(transform.position - new Vector3(0, 0.5f ,0), transform.right, 2, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 300));
    }

    IEnumerator Attack()
    {
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.7f);
        animator.SetBool("IsAttacking", false);
    }

    IEnumerator Slide()
    {
        animator.SetBool("IsSliding", true);
        yield return new WaitForSeconds(0.55f);
        animator.SetBool("IsSliding", false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("NinjaWeapon"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), collider);
            rb.freezeRotation = false;
        }
    }
}
