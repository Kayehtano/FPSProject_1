using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Properties and References")]
    public NavMeshAgent agent;
    public Transform player;
    public PlayerMovement playerComponent;
    public LayerMask whatPlayer, whatGround;
    public Animator anim;
    public Animator arms;
    public float health;
    public float damage = 2f;

    [Header("Attack")]
    public float TimeBetweenAttacks;
    bool alreadyAttacked;

    [Header("States")]
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool isAttacking;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        playerComponent = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = Random.Range(3, 8);
        health = Random.Range(8f, 15f);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        //Debug.Log("Chasing...");
    }

    private void AttackPlayer()
    {
        // make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            // Attack code
            //Debug.Log("Attacking...");
            arms.Play("attack1");
            isAttacking = true;
            playerComponent.TakeDamage(damage);

            alreadyAttacked = true;
            isAttacking = false;
            Invoke(nameof(ResetAttack), TimeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatPlayer);

        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        // animation
        if (agent.velocity.magnitude > 0f)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        // hp
        if (health < 1f)
        {
            Destroy(this.gameObject);
        }
    }

    public string TagToIgnore = "Barricade";

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagToIgnore)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}