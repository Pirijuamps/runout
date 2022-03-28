using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drivingMapper : MonoBehaviour {

    LogitechGSDK.LogiControllerPropertiesData properties;

    public float xAxis, GasInput, BreakInput, ClutchInput;

    public bool Hshift = true;
    bool isInGear;
    public int CurrentGear;

    private void Start()
    {
        print(LogitechGSDK.LogiSteeringInitialize(false));
    }

    private void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(0);
            HShifter(rec);
            xAxis = rec.lX / 32768f; //-1 0 1
            if(rec.lY > 0)
            {
                GasInput = 0;
            }
            else if(rec.lY < 0)
            {
                GasInput = rec.lY / -32768f;
            }

            if(rec.lRz > 0)
            {
                BreakInput = 0;
            }
            else if(rec.lRz < 0)
            {
                BreakInput = rec.lRz / -32768f;
            }

            if(rec.rglSlider[0] > 0)
            {
                ClutchInput = 0;
            }
            else if (rec.rglSlider[0] < 0)
            {
                ClutchInput = rec.rglSlider[0] / -32768f;
            }
        }
        else
        {
            print("No steering wheel lul");
        }

        //somos unos dummy brains
        //transform.Translate(1f*xAxis*Time.deltaTime, 0f, 0f);

    }

    public void HShifter(LogitechGSDK.DIJOYSTATE2ENGINES shifter)
    {
        for (int i = 12; i < 19; i++)
        {
            if(ClutchInput > 0.5f)
            {
                if(shifter.rgbButtons[i] == 128)
                {
                    CurrentGear = i - 11;
                    if(CurrentGear == 7) CurrentGear = -1;
                    isInGear = true;
                    break;
                }
                else
                {
                    CurrentGear = 0;
                    isInGear = false;
                }
            }
        }
    }
}
