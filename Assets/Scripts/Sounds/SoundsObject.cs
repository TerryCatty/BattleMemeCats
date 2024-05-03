using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerSounds;

public class SoundsObject : MonoBehaviour
{
    private AudioSource audioSource;

    private float timeClip;
    private float volumeMultiplier;

   [SerializeField] private TypeSound soundType;

     private float defVolume;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        defVolume = audioSource.volume;


        if (audioSource.playOnAwake)
        {
            PlaySound(audioSource.clip, defVolume, soundType, audioSource.loop);
        }
    }

    public void PlaySound(AudioClip sound, float defaultVolume, TypeSound soundType,  bool isLoop)
    {

        switch (soundType)
        {
            case TypeSound.Sound:
                volumeMultiplier = (float)SoundsManager.instance.GetSoundsVolumeMultiplier();
                break;
            case TypeSound.Music:
                volumeMultiplier = (float)SoundsManager.instance.GetMusicVolumeMultiplier();
                break;
        }

        this.soundType = soundType;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;

        defVolume = defaultVolume;

        SetVolume(defaultVolume);

        timeClip = sound.length;
        audioSource.loop = isLoop;

        audioSource.Play();

        StartCoroutine(DestroySound());
    }

    private void SetVolume(float defaultVolume)
    {
        audioSource.volume = defaultVolume * volumeMultiplier;
    }


    IEnumerator DestroySound()
    {
        if(audioSource.loop == false)
        {
            yield return new WaitForSeconds(timeClip);

            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SoundsManager.ChangeVolume += SetMultiplierVolume;
    }

    private void OnDisable()
    {
        SoundsManager.ChangeVolume -=  SetMultiplierVolume;
    }

    private void SetMultiplierVolume(SoundsManager soundsManag)
    {
        if (soundType == TypeSound.Music) volumeMultiplier = soundsManag.GetMusicVolumeMultiplier();
        else if (soundType == TypeSound.Sound) volumeMultiplier = soundsManag.GetSoundsVolumeMultiplier();

        SetVolume(defVolume);
    }

}
