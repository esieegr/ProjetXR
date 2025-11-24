using UnityEngine;

public class ControllerFlashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [SerializeField] float intensity = 5f;
    [SerializeField] float range = 15f;
    [SerializeField] float spotAngle = 40f;
    [SerializeField] Color lightColor = new Color(1f, 0.95f, 0.85f);
    
    [Header("Shadow Settings")]
    [SerializeField] bool castShadows = true;
    [SerializeField] LightShadows shadowType = LightShadows.Soft;
    
    [Header("Position Offset")]
    [SerializeField] Vector3 positionOffset = new Vector3(0f, 0f, 0.05f);
    [SerializeField] Vector3 rotationOffset = Vector3.zero;

    [Header("Toggle Feature")]
    [SerializeField] bool canToggle = true;
    [SerializeField] OVRInput.Button toggleButton = OVRInput.Button.One;

    Light flashlight;
    bool isOn = true;
    OVRInput.Controller controller;

    void Start()
    {
        DetermineController();
        CreateFlashlight();
    }

    void DetermineController()
    {
        if (transform.name.Contains("Left"))
        {
            controller = OVRInput.Controller.LTouch;
        }
        else if (transform.name.Contains("Right"))
        {
            controller = OVRInput.Controller.RTouch;
        }
        else
        {
            controller = OVRInput.Controller.RTouch;
        }
    }

    void CreateFlashlight()
    {
        GameObject lightObj = new GameObject("ControllerFlashlight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = positionOffset;
        lightObj.transform.localRotation = Quaternion.Euler(rotationOffset);

        flashlight = lightObj.AddComponent<Light>();
        flashlight.type = LightType.Spot;
        flashlight.intensity = intensity;
        flashlight.range = range;
        flashlight.spotAngle = spotAngle;
        flashlight.color = lightColor;
        flashlight.shadows = castShadows ? shadowType : LightShadows.None;
        flashlight.renderMode = LightRenderMode.ForcePixel;
        flashlight.enabled = isOn;
    }

    void Update()
    {
        if (canToggle && OVRInput.GetDown(toggleButton, controller))
        {
            ToggleFlashlight();
        }
    }

    void ToggleFlashlight()
    {
        isOn = !isOn;
        if (flashlight != null)
        {
            flashlight.enabled = isOn;
        }
    }

    public void SetFlashlightState(bool state)
    {
        isOn = state;
        if (flashlight != null)
        {
            flashlight.enabled = isOn;
        }
    }

    public bool IsOn()
    {
        return isOn;
    }

    void OnValidate()
    {
        if (Application.isPlaying && flashlight != null)
        {
            flashlight.intensity = intensity;
            flashlight.range = range;
            flashlight.spotAngle = spotAngle;
            flashlight.color = lightColor;
            flashlight.shadows = castShadows ? shadowType : LightShadows.None;
            flashlight.renderMode = LightRenderMode.ForcePixel;
            
            Transform lightTransform = flashlight.transform;
            lightTransform.localPosition = positionOffset;
            lightTransform.localRotation = Quaternion.Euler(rotationOffset);
        }
    }
}

