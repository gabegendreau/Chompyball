using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager soundManager;
    public AudioSource audioSource;
    public AudioClip gagOne;
    public AudioClip gagTwo;
    public AudioClip gagThree;
    public AudioClip gagFour;
    public AudioClip gagFive;
    public AudioClip bite;
    public AudioClip bounce;
    public AudioClip powerUp;
    public AudioClip stumpOne;
    public AudioClip stumpTwo;
    public AudioClip stumpThree;
    public AudioClip stumpFour;
    PlayerBall player;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = this;
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerBall>();
    }

    public void playRandomGag() {
        int randomGag = Random.Range(1,6);
        switch (randomGag) {
            case 1:
                audioSource.PlayOneShot(gagOne);
                break;
            case 2:
                audioSource.PlayOneShot(gagTwo);
                break;
            case 3:
                audioSource.PlayOneShot(gagThree);
                break;
            case 4:
                audioSource.PlayOneShot(gagFour);
                break;
            case 5:
                audioSource.PlayOneShot(gagFive);
                break;
            default:
                audioSource.PlayOneShot(gagOne);
                break;
        }
    }

    public void playRandomStump() {
        int randomStump = Random.Range(1,5);
        switch (randomStump) {
            case 1:
                audioSource.PlayOneShot(stumpOne);
                break;
            case 2:
                audioSource.PlayOneShot(stumpTwo);
                break;
            case 3:
                audioSource.PlayOneShot(stumpThree);
                break;
            case 4:
                audioSource.PlayOneShot(stumpFour);
                break;
            default:
                audioSource.PlayOneShot(stumpOne);
                break;
        }
    }

    public void playPickUpSound(bool rotten) {
        if (player.getChompy()) {
            audioSource.PlayOneShot(bite);
        } else {
            if (!rotten) {
                audioSource.PlayOneShot(bite);
            } else {
                playRandomGag();
            }
        }
        
    }
    public void playBounce() {
        audioSource.PlayOneShot(bounce);
    }

    public void playPowerUp() {
        audioSource.PlayOneShot(powerUp);
    }
}
