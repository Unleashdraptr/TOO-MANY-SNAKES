using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snakelaunched : MonoBehaviour
{
    public Vector3 goal;
    public Transform player;
    public GameObject snake;
    private float Duration;
    private float Height;
    // Start is called before the first frame update
    void Start()
    {
        float dis = Vector3.Distance(transform.position, goal);
        Duration = dis/8;
        Height = dis;
        StartCoroutine(MoveInArc(goal, Duration, Height));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveInArc(Vector3 destination, float duration, float height)
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
            currentPosition.y += Mathf.Sin(fraction * Mathf.PI) * height;

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
        GameObject landedSnake = Instantiate(snake);
        landedSnake.transform.position = transform.position;
        landedSnake.GetComponent<SnakeAI>().goal = player;
        Destroy(gameObject);
    }
}
