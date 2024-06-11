using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;
    int optionNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get and filter supported resolutions
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            // Check if this resolution matches the current screen resolution
            if (item.width == Screen.currentResolution.width && item.height == Screen.currentResolution.height)
                resolutionDropdown.value = optionNum;

            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();

        // Set fullscreen toggle state based on current screen mode
        fullscreenToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen;
        screenMode = Screen.fullScreenMode;
    }

    // Called when the resolution dropdown value changes
    public void DropdownOptionChange(int index)
    {
        resolutionNum = index;
    }

    // Called when the fullscreen toggle value changes
    public void FullScreenToggleChange(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    // Called when the "OK" button is clicked
    public void OkButtonClick()
    {
        Resolution selectedResolution = resolutions[resolutionNum];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, screenMode);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
