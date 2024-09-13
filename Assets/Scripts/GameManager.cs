using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour
{
    InputAction m_PressAction;
    [SerializeField]
    GameObject[] menus;
    enum GameMenus {TutorialMajor, TutorialOne, TutorialTwo, Minimap};
    GameMenus gameMenus;
    int index = 0;
    bool isPressed = false;
    SpawnLevel spawnLevel;
    void Awake()
    {
        var resolution = Screen.resolutions;
        //This just helps aleviate overheating the gpu
        Screen.SetResolution((int)(resolution[0].width * 0.75), (int)(resolution[0].height * 0.75), true);
        QualitySettings.vSyncCount = 0; 
        //The AR camera might interfere with targetframerate, but i'm not sure
        //Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        Screen.orientation = ScreenOrientation.Portrait;

        m_PressAction = new InputAction("touch", binding: "<Pointer>/press");

        m_PressAction.started += ctx =>
        {
            if (ctx.control.device is Pointer device)
            {
                OnPress();
            }
        };

        spawnLevel = GameObject.FindAnyObjectByType<SpawnLevel>();
    }

    void Update()
    {
        if (Pointer.current == null)
            return;


    }

    void OnPress()
    {
        if (menus[(int)GameMenus.TutorialMajor].activeInHierarchy && menus[(int)GameMenus.TutorialOne].activeInHierarchy)
        {
            menus[(int)GameMenus.TutorialOne].SetActive(false);
            menus[(int)GameMenus.TutorialTwo].SetActive(true);
        }
        else if (menus[(int)GameMenus.TutorialTwo].activeInHierarchy)
        {
            menus[(int)GameMenus.TutorialMajor].SetActive(false);
            menus[(int)GameMenus.Minimap].SetActive(true);
            spawnLevel.ChangeTracking();

        }
    }

    void OnEnable()
    {
        m_PressAction.Enable();
    }
    void OnDisable()
    {
        m_PressAction.Disable();
    }
    void OnDestroy()
    {
        m_PressAction.Dispose();
    }

}
