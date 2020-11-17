using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.UI;

public class DayNightSystem : MonoBehaviour {
    
    public struct userAttributes { }  
    public struct appAttributes { }

    public bool isNight = true;

    public Camera MainCam;

    public Color Light;
    public Color Dark;

    public Image RoundCircleJoystick;
    public Image HandleJoystick;

    public SpriteRenderer[] WallSprite;

    private void Awake() {
        ConfigManager.FetchCompleted += SetDayAndNight;
        ConfigManager.FetchConfigs<userAttributes, appAttributes>( new userAttributes(), new appAttributes());
    }

    private void Update() {
        
    }

    void SetDayAndNight(ConfigResponse configResponse)
    {
        isNight = ConfigManager.appConfig.GetBool("IsNight");

        if(isNight)
        {
            MainCam.backgroundColor = Dark;
            foreach (SpriteRenderer SR in WallSprite)
            {
                SR.color = Light;
            }
            RoundCircleJoystick.color = Light;
            HandleJoystick.color = Light;
        }
        else
        {
            MainCam.backgroundColor = Light;
            foreach (SpriteRenderer SR in WallSprite)
            {
                SR.color = Dark;
            }
            RoundCircleJoystick.color = Dark;
            HandleJoystick.color = Dark;
        }
    }
}
