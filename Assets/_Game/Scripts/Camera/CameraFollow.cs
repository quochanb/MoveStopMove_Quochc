using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;

    private void LateUpdate()
    {
        Vector3 camPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, camPos, smoothTime);
        transform.LookAt(target);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

}
