using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour {

    public Transform objectToFollow;
    public Vector3 offset;
    public float followspeed = 10;
    public float lookspeed = 10;

    public void LookAtTarget()
    {
        Vector3 lookDirection = objectToFollow.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookspeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 targetPos = objectToFollow.position + objectToFollow.forward * offset.z + objectToFollow.right * offset.x + objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, followspeed * Time.deltaTime);
    }

    public void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }
}
