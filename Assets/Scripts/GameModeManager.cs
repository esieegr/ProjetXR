using UnityEngine;
using Meta.XR.MRUtilityKit;

public class GameModeManager : MonoBehaviour
{
    public enum GameMode
    {
        VR,
        MR
    }

    [Header("Current Mode")]
    [SerializeField] GameMode currentMode = GameMode.VR;

    [Header("EffectMesh References")]
    [SerializeField] EffectMesh[] environmentMeshes;
    [SerializeField] EffectMesh[] furnitureMeshes;

    [Header("Passthrough")]
    [SerializeField] OVRPassthroughLayer passthroughLayer;

    [Header("Flashlight Management")]
    [SerializeField] bool disableFlashlightsInMR = true;

    ControllerFlashlight[] controllerFlashlights;
    bool[] flashlightsInitialState;

    void Start()
    {
        FindFlashlights();
        EnsureCameraTransparency();
        ApplyMode(currentMode);
    }

    void EnsureCameraTransparency()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Color bg = mainCamera.backgroundColor;
            mainCamera.backgroundColor = new Color(bg.r, bg.g, bg.b, 0f);
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    void FindFlashlights()
    {
        controllerFlashlights = FindObjectsByType<ControllerFlashlight>(FindObjectsSortMode.None);
        if (controllerFlashlights != null && controllerFlashlights.Length > 0)
        {
            flashlightsInitialState = new bool[controllerFlashlights.Length];
            for (int i = 0; i < controllerFlashlights.Length; i++)
            {
                flashlightsInitialState[i] = controllerFlashlights[i].IsOn();
            }
        }
    }

    public void SetVRMode()
    {
        currentMode = GameMode.VR;
        ApplyMode(GameMode.VR);
    }

    public void SetMRMode()
    {
        currentMode = GameMode.MR;
        ApplyMode(GameMode.MR);
    }

    public void ToggleMode()
    {
        if (currentMode == GameMode.VR)
        {
            SetMRMode();
        }
        else
        {
            SetVRMode();
        }
    }

    void ApplyMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.VR:
                ApplyVRMode();
                break;
            case GameMode.MR:
                ApplyMRMode();
                break;
        }

        Debug.Log($"Game mode changed to: {mode}");
    }

    void ApplyVRMode()
    {
        if (passthroughLayer != null)
        {
            passthroughLayer.enabled = false;
        }

        if (disableFlashlightsInMR && controllerFlashlights != null)
        {
            for (int i = 0; i < controllerFlashlights.Length; i++)
            {
                if (controllerFlashlights[i] != null && i < flashlightsInitialState.Length)
                {
                    controllerFlashlights[i].SetFlashlightState(flashlightsInitialState[i]);
                }
            }
        }

        if (environmentMeshes != null)
        {
            foreach (var environmentMesh in environmentMeshes)
            {
                if (environmentMesh != null)
                {
                    environmentMesh.HideMesh = false;
                }
            }
        }

        if (furnitureMeshes != null)
        {
            foreach (var furnitureMesh in furnitureMeshes)
            {
                if (furnitureMesh != null)
                {
                    furnitureMesh.HideMesh = false;
                }
            }
        }
    }

    void ApplyMRMode()
    {
        if (passthroughLayer != null)
        {
            passthroughLayer.enabled = true;
        }

        if (disableFlashlightsInMR && controllerFlashlights != null)
        {
            foreach (var flashlight in controllerFlashlights)
            {
                if (flashlight != null)
                {
                    flashlight.SetFlashlightState(false);
                }
            }
        }

        if (environmentMeshes != null)
        {
            foreach (var environmentMesh in environmentMeshes)
            {
                if (environmentMesh != null)
                {
                    environmentMesh.HideMesh = true;
                }
            }
        }

        if (furnitureMeshes != null)
        {
            foreach (var furnitureMesh in furnitureMeshes)
            {
                if (furnitureMesh != null)
                {
                    furnitureMesh.HideMesh = true;
                }
            }
        }
    }
}

