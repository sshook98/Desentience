using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class Pause_UIPanel : Base_UIPanel
{
    public Button resumeButton;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Button menuButton;

    public override void OpenBehavior()
    {
        base.OpenBehavior();

        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(() => { ResumeButtonPressed(); });

        bool resultMaster = AudioManager.Instance.masterAudioMixerGroup.audioMixer.GetFloat("volumeMaster", out float masterVolume);
        if (resultMaster)
        {
            ChangeVolumeSliderValue(masterVolumeSlider, masterVolume);
        } else
        {
            Debug.LogWarning("Failed to GetFloat(\"volumeMaster\")");
        }
        masterVolumeSlider.onValueChanged.RemoveAllListeners();
        masterVolumeSlider.onValueChanged.AddListener(delegate { MasterVolumeSliderChanged(); });
        bool resultMusic = AudioManager.Instance.musicAudioMixerGroup.audioMixer.GetFloat("volumeMusic", out float musicVolume);
        if (resultMusic)
        {
            ChangeVolumeSliderValue(musicVolumeSlider, musicVolume);
        }
        else
        {
            Debug.LogWarning("Failed to GetFloat(\"volumeMusic\")");
        }
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.AddListener(delegate { MusicVolumeSliderChanged(); });
        bool resultSfx = AudioManager.Instance.sfxAudioMixerGroup.audioMixer.GetFloat("volumeSfx", out float sfxVolume);
        if (resultSfx)
        {
            ChangeVolumeSliderValue(sfxVolumeSlider, sfxVolume);
        }
        else
        {
            Debug.LogWarning("Failed to GetFloat(\"volumeSfx\")");
        }
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SfxVolumeSliderChanged(); });
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(() => { MenuButtonPressed(); });
    }

    private void Awake()
    {
        
    }

    void ResumeButtonPressed()
    {
        GameManager.Instance.TogglePause();
    }

    void MasterVolumeSliderChanged()
    {
        AudioManager.Instance.masterAudioMixerGroup.audioMixer.SetFloat("volumeMaster", Mathf.Log10(masterVolumeSlider.value) * 20);
    }

    void MusicVolumeSliderChanged()
    {
        AudioManager.Instance.musicAudioMixerGroup.audioMixer.SetFloat("volumeMusic", Mathf.Log10(musicVolumeSlider.value) * 20);
    }

    void SfxVolumeSliderChanged()
    {
        AudioManager.Instance.sfxAudioMixerGroup.audioMixer.SetFloat("volumeSfx", Mathf.Log10(sfxVolumeSlider.value) * 20);
    }

    void MenuButtonPressed()
    {
        GameManager.Instance.ReturnToMainMenu();
    }

    void ChangeVolumeSliderValue(Slider slider, float volume_in_decibels)
    {
        slider.value = Mathf.Pow(10.0f, (volume_in_decibels / 20));
    }
}