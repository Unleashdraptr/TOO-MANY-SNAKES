using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public float bodySpeed = 5;
    public float StartingSize = 5;
    public int Gap;

    public List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PosHistory = new List<Vector3>();


    public GameObject BodyPrefab;
    // Start is called before the first frame update
    void Start()
    {
       for (int i = 0; i < StartingSize; i++) 
        {
            GrowSnake();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PosHistory.Insert(0, transform.position);

       int index = 0;
        foreach (GameObject body in BodyParts) 
        { 
            Vector3 point = PosHistory[Mathf.Min(index * Gap, PosHistory.Count-1)];
            Vector3 moveDir = point - body.transform.position;
            body.transform.position += moveDir * bodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }
    }

    private void GrowSnake()
    { 
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }
}
