using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private TypeSound soundType;

    [SerializeField] private SoundsObject soundObject;

    private SoundsManager soundsManager;

    [Serializable]
    public enum TypeSound
    {
        Music,
        Sound
    }


    protected float volumeMultiplier = 1f;

   /* [SerializeField] private bool playOnStart = false;
    [SerializeField] private bool playLoop = false;

    private void Start()
    {
        if (playOnStart)
        {
            PlaySound(GetComponent<AudioSource>().clip, GetComponent<AudioSource>().volume, playLoop);
        }
    }*/


    public void PlaySound(AudioClip sound, float defaultVolume, bool isLoop = false)
    {
        SpawnSound().PlaySound(sound, defaultVolume, soundType, soundsManager, isLoop);
    }
    
    private SoundsObject SpawnSound()
    {
        SoundsObject sound = Instantiate(soundObject, transform.position, Quaternion.identity);

        return sound;
    }

    private void OnEnable()
    {
        SoundsManager.ChangeVolume += (SoundsManager soundsManag) => {
            soundsManager = soundsManag;
        };
    }

    private void OnDisable()
    {
        SoundsManager.ChangeVolume -= (SoundsManager soundsManag) => {
            soundsManager = soundsManag;
        };
    }

}
