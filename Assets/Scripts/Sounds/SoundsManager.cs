using System;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private GameObject imageOffSound;
    private bool soundsIsOff = false;
    private float soundsVolumeMultiplier = 1f;

    [SerializeField] private GameObject imageOffMusic;
    private bool musicIsOff = false;
    private float musicVolumeMultiplier = 1f;

    public static SoundsManager instance;

    public static event Action<SoundsManager> ChangeVolume;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SwitchVolumeMusic()
    {
        musicVolumeMultiplier = musicIsOff ? 1.0f : 0.0f;

        musicIsOff = !musicIsOff;
        imageOffMusic.SetActive(musicIsOff);

        ChangeVolume?.Invoke(this);
    }

    public void SwitchVolumeSounds()
    {
        soundsVolumeMultiplier = soundsIsOff ? 1.0f : 0.0f;

        soundsIsOff = !soundsIsOff;
        imageOffSound.SetActive(soundsIsOff);

        ChangeVolume?.Invoke(this);
    }

    public float GetMusicVolumeMultiplier()
    {
        return musicVolumeMultiplier;
    }


    public float GetSoundsVolumeMultiplier()
    {
        return soundsVolumeMultiplier;
    }
}
