using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Axel
{
    Front,
    Rear
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider collider;
    public Axel axel;
}

public class CarControler2 : MonoBehaviour
{

    [SerializeField]
    private float maxAccel = 20.0f;
    [SerializeField]
    private float turnSensitivity = 1.0f;
    [SerializeField]
    private float maxSteerAngle = 45.0f;
    [SerializeField]
    private Vector3 _centerOfMass;
    [SerializeField]
    private List<Wheel> wheels;

    private float inputX, inputY;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _centerOfMass;
    }

    // Update is called once per frame
    private void Update()
    {
        AnimateWheels();
        GetInputs();
    }

    private void FixedUpdate()
    {
        Moverino();
        Turn();
    }

    private void GetInputs()
    {
        inputX = gameObject.GetComponent<drivingMapper>().xAxis;
        inputY = gameObject.GetComponent<drivingMapper>().GasInput;
    }

    private void Moverino()
    {
        foreach(var wheel in wheels)
        {
            wheel.collider.motorTorque = inputY * maxAccel * 500 * Time.deltaTime;
        }
    }

    private void Turn()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = inputX * turnSensitivity * maxSteerAngle;
                wheel.collider.steerAngle = Mathf.Lerp(wheel.collider.steerAngle, _steerAngle, 0.5f);
            }
        }
    }

    private void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion _rot;
            Vector3 _pos;
            wheel.collider.GetWorldPose(out _pos, out _rot);
            wheel.model.transform.position = _pos;
            wheel.model.transform.rotation = _rot;
        }
    }

}
