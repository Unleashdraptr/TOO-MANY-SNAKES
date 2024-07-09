using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


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
                snake.destination = goal.position;
                MoveToTarget();
                attackTime += Time.deltaTime;
                if (CheckDistance(attackDistance, goal.position) && attackTime > 1)
                {
                    state = State.ReadyAttack;
                    attackTime = 0;
                    attackPos = goal.position;
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

    void attack()
    {
        switch (snakeType)
        {
            case SnakeType.DashSnake:
                if (attackTime == 0)
                {
                    Destroy(Instantiate(SnakeItem, transform), 1);
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
                    StartCoroutine(MoveInArc(goal.position, 1.5f));
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
                if (CheckDistance(attackDistance + 3, goal.position) && attackTime == 0)
                {
                    goal.GetComponent<Rigidbody>().velocity += transform.forward * 15 + transform.up * 5;
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
        attackTime += Time.deltaTime;
        if (attackTime > attackWindUp)
        {
            transform.LookAt(attackPos);
            state = State.Attack;
            attackTime = 0;
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
            // Increment the timer by the time passed since last frame
            timer += Time.deltaTime;
            // Calculate the fraction of the journey completed
            float fraction = timer / duration;

            // Calculate the current horizontal position
            Vector3 currentPosition = Vector3.Lerp(startPosition, destination, fraction);

            // Add the arc height based on the sine of the fraction of the journey completed
            currentPosition.y += Mathf.Sin(fraction * Mathf.PI) * 10;

            // Apply the calculated position
            transform.position = currentPosition;

            // Wait until next frame
            yield return null;
        }
        transform.position = destination;
    }
}
