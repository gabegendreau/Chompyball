using UnityEngine;

public class StumpChomp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerBall player = FindObjectOfType<PlayerBall>();
            player.addChompy();
            SoundManager.soundManager.playPowerUp();
        }
        Destroy(gameObject);
    }
}
