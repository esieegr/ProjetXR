using UnityEngine;
using TMPro;

public class ControllerUI : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] Vector3 uiOffset = new Vector3(-0.05f, 0f, 0.05f);
    [SerializeField] Vector3 uiRotation = new Vector3(0f, 90f, 0f);
    [SerializeField] float uiScale = 0.001f;

    [Header("Controller Reference")]
    [SerializeField] Transform leftControllerAnchor;

    Canvas canvas;
    TMP_Text targetText;

    void Start()
    {
        FindLeftController();
        CreateUI();
        RegisterWithScoreManager();
    }

    void FindLeftController()
    {
        if (leftControllerAnchor == null)
        {
            OVRCameraRig cameraRig = FindAnyObjectByType<OVRCameraRig>();
            if (cameraRig != null)
            {
                leftControllerAnchor = cameraRig.leftHandAnchor;
            }
            else
            {
                Debug.LogError("OVRCameraRig not found");
            }
        }
    }

    void CreateUI()
    {
        if (leftControllerAnchor == null) return;

        GameObject canvasObj = new GameObject("ControllerCanvas");
        canvasObj.transform.SetParent(leftControllerAnchor);
        canvasObj.transform.localPosition = uiOffset;
        canvasObj.transform.localRotation = Quaternion.Euler(uiRotation);
        canvasObj.transform.localScale = Vector3.one * uiScale;

        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(300, 100);

        GameObject textObj = new GameObject("TargetsText");
        textObj.transform.SetParent(canvasObj.transform);
        textObj.transform.localPosition = Vector3.zero;
        textObj.transform.localRotation = Quaternion.identity;
        textObj.transform.localScale = Vector3.one;

        targetText = textObj.AddComponent<TextMeshProUGUI>();
        targetText.text = "0 Restants";
        targetText.fontSize = 36;
        targetText.alignment = TextAlignmentOptions.Center;
        targetText.color = Color.white;

        RectTransform textRect = targetText.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(300, 100);
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);
    }

    void RegisterWithScoreManager()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.targetsText = targetText;
        }
    }
}

