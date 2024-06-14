using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] cameraPosition;

    private CameraState state;
    private Quaternion targetRotation;
    private Transform tf;

    public Transform Tf
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    private void Awake()
    {
        ChangeCameraState(CameraState.MainMenu);
    }

    private void LateUpdate()
    {
        Vector3.Lerp(Tf.position, offset, moveSpeed * Time.deltaTime);
        Tf.rotation = Quaternion.Lerp(Tf.rotation, targetRotation, moveSpeed * Time.deltaTime);
        Tf.position = Vector3.Lerp(Tf.position, target.position + offset, moveSpeed * Time.deltaTime);

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void ChangeCameraState(CameraState state)
    {
        this.state = state;
        //lay vi tri offset tuong ung voi state
        offset = cameraPosition[(int)state].localPosition;
        //lay goc xoay tuong ung voi state
        targetRotation = cameraPosition[(int)state].localRotation;
    }
}
