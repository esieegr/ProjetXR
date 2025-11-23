using UnityEngine;

public class ModeToggleInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameModeManager gameModeManager;

    [Header("Input Settings")]
    [SerializeField] private OVRInput.Button toggleButton = OVRInput.Button.Four;
    
    private bool wasButtonPressed = false;

    void Start()
    {
        if (gameModeManager == null)
        {
            gameModeManager = FindFirstObjectByType<GameModeManager>();
            if (gameModeManager == null)
            {
                Debug.LogError("GameModeManager not found in scene!");
            }
        }
    }

    void Update()
    {
        bool isButtonPressed = OVRInput.Get(toggleButton);

        if (isButtonPressed && !wasButtonPressed)
        {
            if (gameModeManager != null)
            {
                gameModeManager.ToggleMode();
            }
        }

        wasButtonPressed = isButtonPressed;
    }
}

