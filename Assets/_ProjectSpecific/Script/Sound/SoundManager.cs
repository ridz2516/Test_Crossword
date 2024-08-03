
using System;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SFXType[] m_AllType;
    [SerializeField] private AudioSource m_SFXPlayer;

    public void PlayClip(eSoundEffect i_Type)
    {
        foreach (var sfxType in m_AllType)
        {
            if(sfxType.Type == i_Type)
            {
                m_SFXPlayer.PlayOneShot(sfxType.SFX);
            }
        }
    }

}

[Serializable]
public class SFXType
{
    public eSoundEffect Type;
    public AudioClip SFX;
}
