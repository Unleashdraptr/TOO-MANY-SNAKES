using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FaceCam : MonoBehaviour
{
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;

    public enum BillboardType { LookAtCamera, CameraForward };

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
    }
    void Update()
    {
        float dist =  Vector3.Distance(Camera.main.transform.position, transform.position);
        if (dist < 6)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = new Color(255,255, 255, 1-(dist-2)*0.25f);
        } 
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);
    }
}
