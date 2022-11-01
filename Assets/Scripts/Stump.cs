using UnityEngine;

public class Stump : MonoBehaviour
{
    StumpCheck stumpUIChecker;

    void Start()
    {
        stumpUIChecker = FindObjectOfType<StumpCheck>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
        SoundManager.soundManager.playRandomStump();
        Destroy(gameObject);
        stumpUIChecker.updateStumpUI();
        }
    }
}
