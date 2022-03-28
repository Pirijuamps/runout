using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    private double horizontalInput;
    private float verticalInput;
    private float breakInput;
    private float steeringAngle;
    private int currentGear;
    private float clutchInput;
    private int trkCounter = 0;

    public WheelCollider frontLeftW, frontRightW;
    public WheelCollider backLeftW, backRightW;
    public Transform frontLeftT, frontRightT;
    public Transform backLeftT, backRightT;
    public float maxSteerAngle = 20;
    public float motorForce = 100;
    public Transform steerT;
    public float CurrentSpeed { get { return gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.23696329f; } }
    public UnityEngine.UI.Text SpeedText, GearText, trkText;

    [SerializeField]
    private Vector3 _centerOfMass;

    public float accelForce;
    public float breakForce;


    //limites de velocidad
    public float maxSpeedGeneral;
    public float maxSpeed1;
    public float minSpeed2;
    public float maxSpeed2;
    public float minSpeed3;
    public float maxSpeed3;
    public float minSpeed4;
    public float maxSpeed4;
    public float minSpeed5;
    public float maxSpeed5;
    public float maxSpeed6;
    public float maxSpeedR;

    public AudioSource audio;
    public AudioClip[] tracks = new AudioClip[12];
    public AudioClip moving;
    public AudioClip idle;
    AudioSource engine;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().centerOfMass = _centerOfMass;
        engine = gameObject.GetComponent<AudioSource>();
    }

    public void GetInput()
    {
        horizontalInput = gameObject.GetComponent<drivingMapper>().xAxis;

        //ajustando deadzone
        if(horizontalInput < .05 && horizontalInput > -.05)
        {
            horizontalInput = 0;
        }
        else if(horizontalInput > .05)
        {
            horizontalInput -= .05;
        }
        else
        {
            horizontalInput += .05;
        }

        verticalInput = gameObject.GetComponent<drivingMapper>().GasInput;
        breakInput = gameObject.GetComponent<drivingMapper>().BreakInput;


    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * (float)horizontalInput;
        frontLeftW.steerAngle = steeringAngle;
        frontRightW.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        currentGear = gameObject.GetComponent<drivingMapper>().CurrentGear;
        clutchInput = gameObject.GetComponent<drivingMapper>().ClutchInput;

        if (currentGear == 0 || clutchInput > 0.5f)
        {
            accelForce = 0;
            engineSound(0);
        }
        else
        {
            //Cuando este en primera, antes de limite 1
            if(currentGear == 1 && CurrentSpeed < maxSpeed1)
            {
                accelForce = 12;
                engineSound(maxSpeed1);
            }
            //Cuando este en primera, que no pase del limite 1
            else if(currentGear == 1 && CurrentSpeed >= maxSpeed1)
            {
                accelForce = 0;

            }
            //Cuando este en segunda, entre el limite 1 y el limite 2
            if (currentGear == 2 && minSpeed2 < CurrentSpeed && CurrentSpeed < maxSpeed2)
            {
                accelForce = 10;
                engineSound(maxSpeed2);
            }
            //Cuando este en segunda, que no pase del limite 2
            else if (currentGear == 2 && (CurrentSpeed >= maxSpeed2 || CurrentSpeed < minSpeed2))
            {
                accelForce = 0;

            }

            // ... y asi con los otros 3 cambios

            if (currentGear == 3 && minSpeed3 < CurrentSpeed && CurrentSpeed < maxSpeed3)
            {
                accelForce = 8;
                engineSound(maxSpeed3);
            }
            else if (currentGear == 3 && (CurrentSpeed >= maxSpeed3 || CurrentSpeed < minSpeed3))
            {
                accelForce = 0;

            }

            if (currentGear == 4 && minSpeed4 < CurrentSpeed && CurrentSpeed < maxSpeed4)
            {
                accelForce = 6;
                engineSound(maxSpeed4);
            }
            else if (currentGear == 4 && (CurrentSpeed >= maxSpeed4 || CurrentSpeed < minSpeed4))
            {
                accelForce = 0;

            }

            if (currentGear == 5 && minSpeed5 < CurrentSpeed && CurrentSpeed < maxSpeed5)
            {
                accelForce = 4;
                engineSound(maxSpeed5);
            }
            else if (currentGear == 5 && (CurrentSpeed >= maxSpeed5 || CurrentSpeed < minSpeed5))
            {
                accelForce = 0;

            }
        }

        frontLeftW.motorTorque = verticalInput * motorForce * accelForce;
        frontRightW.motorTorque = verticalInput * motorForce * accelForce;

        if (breakInput != 0 && CurrentSpeed >= 0)
        {
            frontLeftW.motorTorque = -breakInput * motorForce * breakForce;
            frontRightW.motorTorque = -breakInput * motorForce * breakForce;
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);
    }

    //Ajustando la posicion de las ruedas conforme a la rotacion de sus colliders
    private void UpdateWheelPose(WheelCollider elCollider, Transform elTransform)
    {
        Vector3 laPos = elTransform.position;
        Quaternion elQuat = elTransform.rotation;

        elCollider.GetWorldPose(out laPos, out elQuat);

        elTransform.position = laPos;
        elTransform.rotation = elQuat;
    }

    //pa el speedometer
    public void getText()
    {
        SpeedText = GetComponent<UnityEngine.UI.Text>();
        GearText = GetComponent<UnityEngine.UI.Text>();
        trkText = GetComponent<UnityEngine.UI.Text>();
    }

    private void musicController()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button10))
        {
            trkCounter += 1;
            audio.clip = tracks[trkCounter];
            audio.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button11))
        {
            trkCounter -= 1;
            audio.clip = tracks[trkCounter];
            audio.Play();
        }

        if (trkCounter == -1) trkCounter = 11;

        trkCounter = trkCounter % 12;  
    }

    private void engineSound(float maxSpeedCustom)
    {
        engine.clip = moving;
        engine.Play();
        engine.pitch = CurrentSpeed / maxSpeedCustom;


        if (maxSpeedCustom == 0)
        {
            engine.clip = idle;
            engine.Play();
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        musicController();
        steerT.Rotate(0, gameObject.GetComponent<drivingMapper>().xAxis * 6, 0, Space.Self);
        SpeedText.text = CurrentSpeed.ToString("f0");
        GearText.text = currentGear.ToString("f0");
        trkText.text = trkCounter.ToString("f0");

        print(trkCounter);
    }

}
