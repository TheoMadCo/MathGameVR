using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSoundEffects : MonoBehaviour { 

    [Header("Audio Elements")]
    public AudioSource audioSource;     // Audio source component
    public AudioClip winSound;          // Sound played when winning
    public AudioClip loseSound;         // Sound played when losing
    public AudioClip newRoundSound;     // Sound played when winning
    public AudioClip endGameSound;      // Sound played when losing
    
    public void PlayNewRoundSound()
    {
        audioSource.PlayOneShot(newRoundSound);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void PlayEndGameSound()
    {
        audioSource.PlayOneShot(endGameSound);
    }
}
