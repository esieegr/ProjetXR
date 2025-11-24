using UnityEngine;

public class PassthroughDiagnostic : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== PASSTHROUGH DIAGNOSTIC ===");
        
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Debug.Log($"Camera Found: {mainCamera.gameObject.name}");
            Debug.Log($"Clear Flags: {mainCamera.clearFlags}");
            Debug.Log($"Background Color: {mainCamera.backgroundColor}");
            Debug.Log($"Background Alpha: {mainCamera.backgroundColor.a}");
            
            if (mainCamera.backgroundColor.a > 0.01f)
            {
                Debug.LogWarning("WARNING: Camera Background Alpha is not 0! This will block passthrough.");
                Debug.LogWarning("Fix: Set Camera Background to Black with Alpha = 0");
            }
        }
        else
        {
            Debug.LogError("No main camera found!");
        }
        
        OVRPassthroughLayer passthroughLayer = FindFirstObjectByType<OVRPassthroughLayer>();
        if (passthroughLayer != null)
        {
            Debug.Log($"OVRPassthroughLayer Found on: {passthroughLayer.gameObject.name}");
            Debug.Log($"Passthrough Enabled: {passthroughLayer.enabled}");
            Debug.Log($"Passthrough Hidden: {passthroughLayer.hidden}");
            
            if (!passthroughLayer.enabled)
            {
                Debug.LogWarning("WARNING: OVRPassthroughLayer is disabled!");
            }
            if (passthroughLayer.hidden)
            {
                Debug.LogWarning("WARNING: OVRPassthroughLayer is hidden!");
            }
        }
        else
        {
            Debug.LogError("No OVRPassthroughLayer found in scene!");
        }
        
        OVRManager ovrManager = FindFirstObjectByType<OVRManager>();
        if (ovrManager != null)
        {
            Debug.Log($"OVRManager Found on: {ovrManager.gameObject.name}");
        }
        else
        {
            Debug.LogWarning("No OVRManager found in scene!");
        }
        
        Debug.Log("=== END DIAGNOSTIC ===");
    }
}

