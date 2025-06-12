using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    REGION,
    ACTION,
    ITEMS,
    OBSTACLES,
    MONSTERS,
    BOSS,
    UI,
    COUNT
}


public class SoundManager : MonoBehaviour
{
    #region Instance
    public static SoundManager instance;

    // Launch the persistence of the gameObject
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        instance = this;
    }
    #endregion

    #region Variable
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundsActionList;
    [SerializeField] private AudioClip[] soundsItemsList;
    [SerializeField] private AudioClip[] soundsUIList;
    #endregion



    public void PlaySound(SoundType sound, int indexSound, float volume = 1)
    {
        AudioClip clipSelected;

        if (sound == SoundType.ACTION)
            clipSelected = soundsActionList[indexSound];
        else if (sound == SoundType.ITEMS)
            clipSelected = soundsItemsList[indexSound];
        else if (sound == SoundType.UI)
            clipSelected = soundsUIList[indexSound];
        else
            return;

        instance.audioSource.PlayOneShot(clipSelected, volume);
    }

    public void PlaySound(AudioClip audioClip, AudioSource audioSource, float volume = 1)
    {
        if (audioClip == null) return;

        audioSource.PlayOneShot(audioClip, volume);
    }
}