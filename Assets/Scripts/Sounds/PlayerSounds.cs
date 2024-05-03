using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private TypeSound soundType;

    [SerializeField] private SoundsObject soundObject;


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

    private void Start()
    {
        switch (soundType)
        {
            case TypeSound.Sound:
                volumeMultiplier = SoundsManager.instance.GetSoundsVolumeMultiplier();
                break;
            case TypeSound.Music:
                volumeMultiplier = SoundsManager.instance.GetMusicVolumeMultiplier();
                break;
        }

    }


    public void PlaySound(AudioClip sound, float defaultVolume, bool isLoop = false)
    {
        SpawnSound().PlaySound(sound, defaultVolume, soundType, isLoop);
        Debug.Log(defaultVolume);
    }
    
    private SoundsObject SpawnSound()
    {
        SoundsObject sound = Instantiate(soundObject, transform.position, Quaternion.identity);

        return sound;
    }

}
