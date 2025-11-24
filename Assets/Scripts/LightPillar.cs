using UnityEngine;

public class LightPillar : MonoBehaviour
{
    [Header("Pillar Settings")]
    [SerializeField] float pillarHeight = 5f;
    [SerializeField] float pillarRadius = 0.025f;
    [SerializeField] Color laserColor = new Color(0f, 1f, 1f, 0.8f);
    [SerializeField] float emissionStrength = 4f;
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] float coreIntensity = 0.7f;
    [SerializeField] float coreSize = 0.4f;

    GameObject pillarObject;
    Material pillarMaterial;
    bool isInitialized = false;

    void Start()
    {
        if (!isInitialized)
        {
            CreateLaserPillar();
        }
    }

    public void Initialize(float height, float radius, Color color)
    {
        pillarHeight = height;
        pillarRadius = radius;
        laserColor = color;
        CreateLaserPillar();
        isInitialized = true;
    }

    void CreateLaserPillar()
    {
        pillarObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pillarObject.name = "LaserPillar";
        pillarObject.transform.SetParent(transform);
        pillarObject.transform.localPosition = new Vector3(0, pillarHeight * 0.5f, 0);
        pillarObject.transform.localRotation = Quaternion.identity;
        pillarObject.transform.localScale = new Vector3(pillarRadius * 2f, pillarHeight * 0.5f, pillarRadius * 2f);

        Destroy(pillarObject.GetComponent<Collider>());

        Renderer renderer = pillarObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Shader laserShader = Shader.Find("Custom/LaserPillar");
            if (laserShader != null)
            {
                pillarMaterial = new Material(laserShader);
                pillarMaterial.SetColor("_Color", laserColor);
                pillarMaterial.SetFloat("_EmissionStrength", emissionStrength);
                pillarMaterial.SetFloat("_ScrollSpeed", scrollSpeed);
                pillarMaterial.SetFloat("_CoreIntensity", coreIntensity);
                pillarMaterial.SetFloat("_CoreSize", coreSize);
                renderer.material = pillarMaterial;
            }
            else
            {
                Debug.LogWarning("LaserPillar shader not found. Using default material.");
            }
        }
    }

    void OnDestroy()
    {
        if (pillarMaterial != null)
        {
            Destroy(pillarMaterial);
        }
    }

    public void SetColor(Color color)
    {
        laserColor = color;
        if (pillarMaterial != null)
        {
            pillarMaterial.SetColor("_Color", color);
        }
    }

    public void SetHeight(float height)
    {
        pillarHeight = height;
        if (pillarObject != null)
        {
            pillarObject.transform.localPosition = new Vector3(0, height * 0.5f, 0);
            pillarObject.transform.localScale = new Vector3(pillarRadius * 2f, height * 0.5f, pillarRadius * 2f);
        }
    }
}

