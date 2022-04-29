using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip trueLetter, wrongLetter, trueCount, wrongCount;
    public static SoundManager instance;

    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        trueLetter = Resources.Load<AudioClip>("trueLetter");
        wrongLetter = Resources.Load<AudioClip>("wrongLetter");
        trueCount = Resources.Load<AudioClip>("trueCount");
        wrongCount = Resources.Load<AudioClip>("wrongCount");
    }

    public void PlaySound(string clip)
    {
        switch (clip)
        {
            case "trueLetter":
                audioSource.PlayOneShot(trueLetter);
                break;
            case "wrongLetter":
                audioSource.PlayOneShot(wrongLetter);
                break;

            case "trueCount":
                audioSource.PlayOneShot(trueCount);
                break;
            case "wrongCount":
                audioSource.PlayOneShot(wrongCount);
                break;


        }
    }


}
