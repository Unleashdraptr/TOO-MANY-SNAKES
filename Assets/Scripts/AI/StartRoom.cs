using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
    public float SpawnArea;
    public int NormalSnakeAmount;
    public GameObject DashSnake;
    public int SpecialSnakeAmount;
    public List<GameObject> snakeTypes;
    private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        SpawnSnakes();
        Destroy(gameObject);
    }

    void SpawnSnakes()
    {
        for(int i = 0; i < NormalSnakeAmount; i++)
        {
            Instantiate(DashSnake, transform.position + new Vector3(Random.Range(-SpawnArea,SpawnArea),0, Random.Range(-SpawnArea, SpawnArea)), Quaternion.identity).GetComponent<SnakeAI>().goal = player.transform;
        }
        for (int i = 0; i < SpecialSnakeAmount; i++)
        {
            Instantiate(snakeTypes[Random.Range(0, snakeTypes.Count)], transform.position + new Vector3(Random.Range(-SpawnArea, SpawnArea), 0, Random.Range(-SpawnArea, SpawnArea)), Quaternion.identity).GetComponent<SnakeAI>().goal = player.transform; ;
        }
    }
}
