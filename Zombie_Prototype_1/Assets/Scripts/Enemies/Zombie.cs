using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Player player;
    public Rigidbody rb;
    public NavMeshAgent ai;
    public Animator animator;
    public ParticleSystem ps;
    public Canvas canvasParent;
    public Slider healthSlider;

    public enum States
    {
        Idle,
        Chasing
    }
    public States currentState;

    Quaternion originalRotation;

    public float health;
    public float moveSpeed;
    public float attackDamage;
    public float attackRate;
    public float coolDownToAttack;
    public float initialCoolDown;
    public float coolDownRate;
    public float aggroDistance;
    public float stoppingDistance;
    public float attackingDistance;
    public float hitScore;
    public float killScore;
    public float psTime;
    public float destroyTime;
    public float sliderYOffset;
    public float uiRemoveDistance;
    public float worldYLevel;

    public bool canAttack;
    public bool isAttacking;
    public bool isStopped;
    public bool isDead;
    public bool countDownShouldDecrease;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ai = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        player = FindObjectOfType<Player>();
        canvasParent = Instantiate(Resources.Load<Canvas>("WorldCanvas"));
        healthSlider = canvasParent.GetComponentInChildren<Slider>();
        ai.stoppingDistance = stoppingDistance;
        initialCoolDown = coolDownToAttack;
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            CheckDistance();
            UpdateUI();

            ai.isStopped = isStopped;
            ai.speed = moveSpeed;

            animator.SetBool("IsAttacking", isAttacking);

            if (health <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    private void Attack()
    {
        canAttack = false;
        isAttacking = true;

        FindObjectOfType<AudioManager>().Play("ZombieAttack1");
        player.TakeDamage(attackDamage);

        StartCoroutine(WaitToResetAttack());
    }

    private void CheckDistance()
    {
        // If zombie is close enough to player to start moving towards them
        if (Vector3.Distance(transform.position, player.transform.position) < aggroDistance)
        {
            isStopped = false;
            currentState = States.Chasing;
            ai.SetDestination(new Vector3(player.transform.position.x, worldYLevel, player.transform.position.z));
            animator.SetFloat("MoveSpeed", moveSpeed);
        }
        // Else if they are too far away
        else
        {
            isStopped = true;
            ai.SetDestination(new Vector3(transform.position.x, worldYLevel, transform.position.z));
            countDownShouldDecrease = false;
            initialCoolDown = coolDownToAttack;
            currentState = States.Idle;
            animator.SetFloat("MoveSpeed", 0);
        }

        // If zombie is close enough to attack player
        if (Vector3.Distance(transform.position, player.transform.position) <= attackingDistance)
        {
            animator.SetFloat("MoveSpeed", 0);
            countDownShouldDecrease = true;

            if (countDownShouldDecrease)
            {
                initialCoolDown -= coolDownRate;
            }

            if (initialCoolDown <= 0)
            {
                countDownShouldDecrease = false;
                initialCoolDown = 0;
            }

            if (canAttack && initialCoolDown <= 0)
            {
                Attack();
            }
        }
        else
        {
            isAttacking = false;
        }
    }

    public void TakeDamage(float amount)
    {
        if (!isDead)
        {
            FindObjectOfType<AudioManager>().Play("HitMarker");
            FindObjectOfType<AudioManager>().Play("ZombieHurt1");
            FindObjectOfType<PlayerCursor>().ShowHitMarker(transform.position);
           // FindObjectOfType<DamageText>().ShowDamageNumber(transform.position, transform.localRotation, amount);
            ps.Play();
            StartCoroutine(WaitToStopPS());
            player.AddToScore(hitScore);
            health -= amount;
        }
    }

    private void Die()
    {
        ai.isStopped = true;
        GetComponent<Collider>().enabled = false;
        player.AddToScore(killScore);
        FindObjectOfType<AudioManager>().Play("ZombieDeath1");
        animator.SetFloat("MoveSpeed", 0);
        animator.SetBool("IsAttacking", false);

        if (Random.Range(1, 3) == 1)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            animator.SetTrigger("Death2");
        }

        Destroy(canvasParent);
        //StartCoroutine(WaitToDestroy());
    }

    private void UpdateUI()
    {
        canvasParent.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, sliderYOffset, gameObject.transform.localPosition.z);
        canvasParent.transform.localRotation = Camera.main.transform.localRotation * originalRotation;
        healthSlider.value = health;

        if (Vector3.Distance(transform.position, player.transform.position) >= uiRemoveDistance)
        {
            canvasParent.gameObject.SetActive(false);
        }
        else
        {
            canvasParent.gameObject.SetActive(true);
        }
    }

    IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    IEnumerator WaitToStopPS()
    {
        yield return new WaitForSeconds(psTime);
        ps.Stop();
    }

    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
