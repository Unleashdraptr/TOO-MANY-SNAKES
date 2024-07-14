using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class BossAI : MonoBehaviour
{
    public Transform goal;
    public float moveSpeed;
    public float turnSpeed;
    public float wanderRadius;
    public float timeToSummon = 25f;

    public GameObject Snake;
    public GameObject Particle;


    public Animator animator;

    private float Timer;

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
        Timer += Time.deltaTime;
        if (Timer > timeToSummon)
        {
            Timer = 0;
            for (int i = 0; i < 10; i++) luanchSnake();
            Particle.GetComponent<ParticleSystem>().Play();
        }
        if (CheckDistance(10,snake.destination)) SetRandomGoal();
        MoveToTarget();
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

    bool CheckDistance(float distance, Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) < distance
            && snake.path.corners.Length < 3)
        {
            return true;
        }
        return false;
    }

    void luanchSnake()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 40;
            randomDirection += transform.position;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1) && Vector3.Distance(transform.position,navHit.position) > 10)
            {
                GameObject snake = Instantiate(Snake);
                snake.transform.position = transform.position + new Vector3(0,-5,0);
                Snakelaunched script = snake.GetComponent<Snakelaunched>();
                script.player = goal;
                script.goal = navHit.position;
                return;
            }
        }
    }
}

