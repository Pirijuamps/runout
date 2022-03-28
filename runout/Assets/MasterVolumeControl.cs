using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasterVolumeControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public static double masterVolume = 1.0f;

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                if (masterVolume > 0.1)
                    masterVolume -= 0.1;
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button6))
            {
                if (masterVolume < 1.0)
                    masterVolume += 0.1;
            }
        }

        AudioListener.volume = (float)masterVolume;
    }
}
