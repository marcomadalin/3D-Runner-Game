using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour 
{ 
    //Sound
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeValueText = null;
    [SerializeField] private Image volumeIcon = null;
    [SerializeField] private Sprite sound = null;
    [SerializeField] private Sprite mute = null;
    private const float defaultVolume = (float)40;
    private float settingsVolume = (float)40;
    private float prevVolume = (float)40;
    private float mutedVol = (float)40;
    private bool muted = false;
    private bool fromToggle = false;

    //Graphics
    [SerializeField] private Dropdown resolutionDropdown = null;
    [SerializeField] private Toggle fullscreenToggle = null;
    private bool settingsFullscreen = true;
    private bool fullscreen = true;
    private Resolution[] resolutions = null;
    private int settingsResolution = 0;
    private int currentResolution = 0;

    //Controls
    [SerializeField] private Button[] controlsInput = null;
    [SerializeField] private playerController playerController = null;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation = null;
    private int controlButtonIndex = 0;
    private string prevControls = string.Empty;
    

    //Levels
    [SerializeField] private Button[] levelsButton = null;
    [SerializeField] private TMP_Text levelSelectedText = null;
    private int nextLevel = 0;

    //Music
    [SerializeField] AudioSource music = null;
    [SerializeField] AudioSource caserio = null;
    [SerializeField] AudioSource defaultMusic = null;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("cheats")) PlayerPrefs.SetInt("cheats", 0);
        if (!PlayerPrefs.HasKey("caserio")) PlayerPrefs.SetInt("caserio", 0);
        if (!PlayerPrefs.HasKey("levelsCheat")) PlayerPrefs.SetInt("levelsCheat", 0);
        if (!PlayerPrefs.HasKey("Level1"))
        {
            PlayerPrefs.SetInt("Level1", 1);
            for (int i = 1; i < levelsButton.Length; ++i) PlayerPrefs.SetInt("Level" + (i+1).ToString(), 0);
        }
    }


    public void Start()
    {
        if (PlayerPrefs.GetInt("caserio") == 1) CheatMusic();
        else DefaultMusic();

        if (PlayerPrefs.HasKey("volume")) volumeSlider.value = PlayerPrefs.GetFloat("volume") * 100;
        else volumeSlider.value = defaultVolume;

        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (string.IsNullOrEmpty(rebinds)) DefaultControls();
        else
        {
            playerController.PlayerInput.actions.LoadBindingOverridesFromJson(rebinds);
            RefreshControlsText(0, "Move");
            RefreshControlsText(1, "Move");
            RefreshControlsText(2, "Jump");
        }

        if (PlayerPrefs.GetInt("levelsCheat") == 0) UnlockLevels(false);
        else UnlockAllLevelsCheat();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        
        for (int i = 0; i < resolutions.Length; ++i) options.Add(resolutions[i].width + " x " + resolutions[i].height);
        
        if (PlayerPrefs.HasKey("resolution")) currentResolution = PlayerPrefs.GetInt("resolution");
        else currentResolution = resolutions.Length - 1;

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();

        if (PlayerPrefs.HasKey("fullscreen")) fullscreen = PlayerPrefs.GetInt("fullscreen") != 0;
        fullscreenToggle.GetComponent<Toggle>().isOn = fullscreen;

        Screen.SetResolution(resolutions[currentResolution].width, resolutions[currentResolution].height, fullscreen);

        settingsFullscreen = fullscreen;
        settingsResolution = currentResolution;
        settingsVolume = volumeSlider.value;
    }

    public void UnlockAllLevelsCheat()
    {
        UnlockLevels(true);
    }

    private void UnlockLevels(bool cheat)
    {
        for (int i = 0; i < levelsButton.Length; ++i)
        {
            if (cheat || PlayerPrefs.GetInt("Level" + (i + 1).ToString()) != 0)
            {
                levelsButton[i].GetComponent<Button>().interactable = true;
                levelsButton[i].GetComponent<Image>().sprite = null;
                levelsButton[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }
    }

    public void CheatMusic()
    {
        music.Stop();
        music = caserio;
        music.Play();
    }

    public void DefaultMusic()
    {
        music.Stop();
        music = defaultMusic;
        music.Play();
    }

    public void ChangeMusic(AudioSource music)
    {
        this.music = music;
    }

    public void SelectLevel(int i)
    {
        nextLevel = i;
        levelSelectedText.text = (i+1).ToString() + " ?";
    }

    public void LoadNextLevel()
    {
        music.Stop();
        SceneManager.LoadScene("Level" + (nextLevel + 1).ToString());
    }

    public void ChangeVolume(float volume)
    {
        if (!muted) mutedVol = volume;
        if (volume == 0)
        {
            volumeIcon.sprite = mute;
            muted = true;
            if (!fromToggle && prevVolume != 0) mutedVol = volume;
            if (fromToggle) fromToggle = false;
        }
        else if (prevVolume == 0 && volume != 0)
        {
            volumeIcon.sprite = sound;
            muted = false;
        }
        prevVolume = volume;
        if (!muted) mutedVol = volume;
        AudioListener.volume = (float)volume / 100;
        volumeValueText.text = ((int)volume).ToString();
    }

    public void ToggleMute()
    {
        float vol = 0;
        muted = !muted;
        if (muted)
        {
            mutedVol = volumeSlider.value;
            fromToggle = true;
        }
        else vol = mutedVol;
        volumeSlider.value = vol;
    }

    public void ApplyVolume()
    {
        settingsVolume = AudioListener.volume * 100;
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
    }

    public void DefaultVolume()
    {
        volumeSlider.value = defaultVolume;
        ApplyVolume();
    }

    public void ResetAudio()
    {
        volumeSlider.value = settingsVolume;
    }

    public void SelectResolution(int resolutionIndex)
    {
        SetResolution(resolutionIndex, false);
    }

    private void SetResolution(int resolutionIndex, bool refresh)
    {
        currentResolution = resolutionIndex;
        Resolution resolution = resolutions[currentResolution];
        Screen.SetResolution(resolution.width, resolution.height, fullscreen);

        if (refresh)
        {
            resolutionDropdown.value = resolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    public void ToggleFullscreen(bool fullsc)
    {
        SetFullscreen(fullsc, false);
    }

    private void SetFullscreen (bool fullsc, bool refresh)
    {
        fullscreen = fullsc;
        if (refresh) fullscreenToggle.GetComponent<Toggle>().isOn = fullsc;
        Screen.fullScreen = fullscreen;
    }

    public void ApplyGraphics()
    {
        settingsFullscreen = fullscreen;
        settingsResolution = currentResolution;
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
        PlayerPrefs.SetInt("resolution", currentResolution);
    }

    public void DefaultGraphics()
    {
        SetFullscreen(true, true);
        SetResolution(resolutions.Length-1, true);
        ApplyGraphics();
    }

    public void ResetGraphics()
    {
        SetFullscreen(settingsFullscreen, true);
        SetResolution(settingsResolution, true);
    }

    public void ApplyControls()
    {
        string rebinds = playerController.PlayerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        prevControls = rebinds;
    }

    public void SetControlsIndex(int i)
    {
        controlButtonIndex = i;
    }

    public void RebindAction(string action)
    {
        StartRebinding(controlButtonIndex, action);
    }

    public void StartRebinding(int i, string action)
    {
        controlsInput[i].enabled = false;
        controlsInput[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
        controlsInput[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().enabled = true;

        string reset = playerController.PlayerInput.actions.SaveBindingOverridesAsJson();

        rebindingOperation = playerController.PlayerInput.actions[action].PerformInteractiveRebinding();

        if (i < 2)
        {
            var horizontal = playerController.PlayerInput.actions[action].ChangeBinding("Horizontal");
            var binding = horizontal.NextPartBinding("Negative");

            if (i == 1) binding = horizontal.NextPartBinding("Positive");
            rebindingOperation.WithTargetBinding(binding.bindingIndex);
        }

        rebindingOperation.OnMatchWaitForAnother(0.1f);
        rebindingOperation.OnComplete(operation => RebindComplete(i, action, reset));
        rebindingOperation.Start();
    }
    
    private bool CheckRepeated()
    {
        HashSet<string> keys = new HashSet<string>();
        
        foreach(var button in controlsInput)
        {
            if (!keys.Add(button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text)) return false;
        }
        return true;
    }

    private void RebindComplete(int i, string action, string reset)
    {
        RefreshControlsText(i, action);
        rebindingOperation.Dispose();

        if (!CheckRepeated())
        {
            playerController.PlayerInput.actions.LoadBindingOverridesFromJson(reset);
            RefreshControlsText(0, "Move");
            RefreshControlsText(1, "Move");
            RefreshControlsText(2, "Jump");
        }

        controlsInput[i].enabled = true;
        controlsInput[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
        controlsInput[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
    }

    private void RefreshControlsText(int i, string action)
    {
        int bindingIndex = playerController.PlayerInput.actions[action].GetBindingIndexForControl(playerController.PlayerInput.actions[action].controls[0]);

        if (i < 2)
        {
            var horizontal = playerController.PlayerInput.actions[action].ChangeBinding("Horizontal");
            var binding = horizontal.NextPartBinding("Negative");

            if (i == 1) binding = horizontal.NextPartBinding("Positive");
            bindingIndex = binding.bindingIndex;
        }

        controlsInput[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = InputControlPath.ToHumanReadableString(
            playerController.PlayerInput.actions[action].bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void DefaultControls()
    {
        var action = playerController.PlayerInput.actions["Move"];
        var negative = action.bindings.IndexOf(b => b.name == "negative");
        var positive = action.bindings.IndexOf(b => b.name == "positive");

        action.ApplyBindingOverride(negative, "<Keyboard>/a");
        action.ApplyBindingOverride(positive, "<Keyboard>/d");
        playerController.PlayerInput.actions["Jump"].ApplyBindingOverride("<Keyboard>/space");
        RefreshControlsText(0, "Move");
        RefreshControlsText(1, "Move");
        RefreshControlsText(2, "Jump");

        ApplyControls();
    }

    public void ResetControls()
    {
        playerController.PlayerInput.actions.LoadBindingOverridesFromJson(prevControls);
        RefreshControlsText(0, "Move");
        RefreshControlsText(1, "Move");
        RefreshControlsText(2, "Jump");
        ApplyControls();
    }

    public void RollCredits()
    {
        music.Stop();
        SceneManager.LoadScene("Credits");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("cheats", 0);
        PlayerPrefs.SetInt("caserio", 0);
        PlayerPrefs.SetInt("levelsCheat", 0);
    }
}
