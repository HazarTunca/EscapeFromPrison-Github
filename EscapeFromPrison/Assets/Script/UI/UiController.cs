using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    #region vars

    public PlayerCamera cam;

    private bool isMouseShown;

    public GameObject settingsUi;
    public Slider mouseSensitivitySlider;
    public TMP_Text sensitivityText;

    private bool isSettingsOpen;

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMouseShown)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        if (isSettingsOpen)
        {
            sensitivityText.SetText(mouseSensitivitySlider.value.ToString("0"));
        }
    }

    public void UpdateSettings()
    {
        cam.sensitivity = mouseSensitivitySlider.value;
    }

    public void ShowSettings()
    {
        settingsUi.SetActive(true);
        isSettingsOpen = true;
    }
    public void HideSettings()
    {
        settingsUi.SetActive(false);
        isSettingsOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
