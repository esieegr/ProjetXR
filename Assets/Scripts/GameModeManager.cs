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

    void Start()
    {
        ApplyMode(currentMode);
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

