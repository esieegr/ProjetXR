using UnityEngine;

public class HeadAmbientLight : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] float intensity = 1f;
    [SerializeField] float range = 8f;
    [SerializeField] Color lightColor = Color.white;
    [SerializeField] LightType lightType = LightType.Point;
    
    [Header("Spot Light Settings")]
    [SerializeField] float spotAngle = 90f;
    
    [Header("Performance")]
    [SerializeField] bool castShadows = false;

    Light ambientLight;

    void Start()
    {
        CreateAmbientLight();
    }

    void CreateAmbientLight()
    {
        GameObject lightObj = new GameObject("HeadAmbientLight");
        lightObj.transform.SetParent(transform);
        lightObj.transform.localPosition = Vector3.zero;
        lightObj.transform.localRotation = Quaternion.identity;

        ambientLight = lightObj.AddComponent<Light>();
        ambientLight.type = lightType;
        ambientLight.intensity = intensity;
        ambientLight.range = range;
        ambientLight.color = lightColor;
        ambientLight.shadows = castShadows ? LightShadows.Soft : LightShadows.None;
        ambientLight.renderMode = LightRenderMode.ForcePixel;

        if (lightType == LightType.Spot)
        {
            ambientLight.spotAngle = spotAngle;
        }
    }

    void OnValidate()
    {
        if (Application.isPlaying && ambientLight != null)
        {
            ambientLight.intensity = intensity;
            ambientLight.range = range;
            ambientLight.color = lightColor;
            ambientLight.type = lightType;
            ambientLight.shadows = castShadows ? LightShadows.Soft : LightShadows.None;
            ambientLight.renderMode = LightRenderMode.ForcePixel;

            if (lightType == LightType.Spot)
            {
                ambientLight.spotAngle = spotAngle;
            }
        }
    }
}

