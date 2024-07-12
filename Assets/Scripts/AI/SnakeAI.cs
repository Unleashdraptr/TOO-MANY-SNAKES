using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


enum SnakeType
{
    DashSnake,
    DivebombSnake,
    SpitterSnake,
    Constrictor,
    SlamSnake
}
enum State
{
    Wander,
    Following,
    ReadyAttack,
    Attack
}

public class SnakeAI : MonoBehaviour
{
    public Transform goal;
    public float moveSpeed;
    public float turnSpeed;
    public float turnAmount;
    public float wanderRadius;
    public float wanderFindDistance;

    public float attackDistance;
    public float attackWindUp;

    public GameObject SnakeItem;

    [SerializeField] SnakeType snakeType;

    public Animator animator;

    private State state = State.Wander;


    private float attackTime;
    private Vector3 attackPos;


    NavMeshAgent snake;

    // Start is called before the first frame update
    void Start()
    {
        snake = GetComponent<NavMeshAgent>();
        snake.updateRotation = false;
        SetRandomGoal();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Pause)
        {
            snake.speed = 0;
        }
        if (!GameManager.Pause)
        {
            snake.speed = moveSpeed;
            switch (state)
            {
                case State.Wander:
                    if (CheckDistance(wanderFindDistance, goal.position))
                    {
                        state = State.Following;
                    }
                    if (CheckDistance(5, snake.destination))
                    {
                        SetRandomGoal();
                    }
                    MoveToTarget();
                    break;

                case State.Following:
                    animator.SetTrigger("Return");
                    snake.destination = goal.position;
                    MoveToTarget();
                    attackTime += Time.deltaTime;
                    if (CheckDistance(attackDistance, goal.position) && attackTime > 1)
                    {
                        state = State.ReadyAttack;
                        attackTime = 0;
                        attackPos = goal.position;
                        animator.SetTrigger("LungePrep");
                    }
                    break;

                case State.ReadyAttack:
                    readyAttack();
                    break;

                case State.Attack:
                    attack();
                    break;

            }
        }

    }

    void attack()
    {
        switch (snakeType)
        {
            case SnakeType.DashSnake:
                if (attackTime == 0)
                {
                    Destroy(Instantiate(SnakeItem, transform), 0.5f);
                }
                transform.position += transform.forward * moveSpeed * Time.deltaTime * 2;
                attackTime += Time.deltaTime;
                if (attackTime > 1)
                {
                    state = State.Following;
                    attackTime = 0;
                }
                break;

            case SnakeType.DivebombSnake:
                if (attackTime == 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.up * -1, out hit) && hit.collider.gameObject.layer  == LayerMask.NameToLayer("Ground"))
                    {
                        StartCoroutine(MoveInArc(new Vector3(goal.position.x,hit.collider.transform.position.y,goal.position.z), 1.5f));
                    } 
                    else
                    {
                        StartCoroutine(MoveInArc(goal.position, 1.5f));
                    }
                    
                }
                attackTime += Time.deltaTime;
                break;

            case SnakeType.SpitterSnake:
                if (attackTime == 0)
                {
                    GameObject Spit = Instantiate(SnakeItem);
                    Rigidbody rb = Spit.GetComponent<Rigidbody>();
                    Spit.transform.SetPositionAndRotation(transform.position, transform.rotation);
                    rb.velocity += Spit.transform.forward * 10 + Spit.transform.up * 10;
                }
                transform.position += transform.forward * moveSpeed * Time.deltaTime * -1.3f;
                attackTime += Time.deltaTime;
                if (attackTime > 1)
                {
                    state = State.Following;
                    attackTime = 0;
                }
                break;

            case SnakeType.Constrictor:
                if (attackTime == 0)
                {
                    Destroy(Instantiate(SnakeItem, transform), 1);
                }
                transform.position += transform.forward * moveSpeed * Time.deltaTime * 3;
                attackTime += Time.deltaTime;
                if (attackTime > 1)
                {
                    state = State.Following;
                    attackTime = 0;
                }
                break;

            case SnakeType.SlamSnake:
                if (CheckDistance(attackDistance + 1.2f, goal.position) && attackTime == 0)
                {
                    goal.GetComponent<Rigidbody>().velocity += transform.forward * 10 + transform.up * 5;
                }
                attackTime += Time.deltaTime;
                if (attackTime > 1)
                {
                    state = State.Following;
                    attackTime = 0;
                }
                break;
        }
    }


    void readyAttack()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime * -0.1f;
        attackTime += Time.deltaTime;
        Vector3 direction = (goal.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed * 3);
        if (attackTime > attackWindUp)
        {
            state = State.Attack;
            attackTime = 0;
            animator.SetTrigger("Lunge");
        }
    }

    void MoveToTarget()
    {
        if (snake.path.corners.Length > 1)
        {
            Vector3 direction = (snake.path.corners[1] - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

    }

    bool CheckDistance(float distance, Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) < distance
            && snake.path.corners.Length < 3)
        {
            return true;
        }
        return false;
    }

    private void SetRandomGoal()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1))
        {
            snake.destination = navHit.position;
        }
    }

    IEnumerator MoveInArc(Vector3 destination, float duration)
    {
        Vector3 startPosition = transform.position;
        float timer = 0f;

        while (timer < duration)
        {
            // Increment the timer by the time passed since the last frame
            timer += Time.deltaTime;
            // Calculate the fraction of the journey completed
            float fraction = timer / duration;

            // Calculate the current horizontal position
            Vector3 currentPosition = Vector3.Lerp(startPosition, destination, fraction);

            // Add the arc height based on the sine of the fraction of the journey completed
            currentPosition.y += Mathf.Sin(fraction * Mathf.PI) * 5;

            // Calculate the direction of movement
            Vector3 direction = (currentPosition - transform.position).normalized;

            // Apply the calculated position
            transform.position = currentPosition;

            // Update the rotation to face the direction of movement with an interpolation between up and down
            if (direction != Vector3.zero)
            {
                Quaternion startRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(45, 0, 0); // Initial upward tilt
                Quaternion endRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(155, 0, 0); // Final downward tilt
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, fraction);
            }

            // Wait until the next frame
            yield return null;
        }
        transform.position = destination;

        if (CheckDistance(5, goal.position))
        {
            Vector3 dir = (transform.position - goal.position).normalized;
            goal.GetComponent<Rigidbody>().velocity += dir * -15 + transform.up * 5;
        }
        GameObject exp = Instantiate(SnakeItem);
        exp.transform.position = transform.position;
        Destroy(exp, 1);
        Destroy(gameObject);
    }

}

