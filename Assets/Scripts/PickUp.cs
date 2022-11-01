using UnityEngine;

public class PickUp : MonoBehaviour
{

    bool rotten;
    bool justTurnt;
    int turnCounter;
    SpriteRenderer spriteRenderer;
    public GameObject prefabToSpawn;
    public Sprite rottenAppleSprite;
    SpawnPickUp spawner;
    Vector3 spawnPosition;
    public int goodPoints;
    public int badPoints;
    public int rotTurns;
    public int stumpTurns;
    public int fasterRotTurns;
    public int fasterStumpTurns;


    // Start is called before the first frame update
    void Start()
    {
        rotten = false;
        justTurnt = false;
        turnCounter = 0;
        spawner = FindObjectOfType<SpawnPickUp>();
        spawnPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (justTurnt)
        {
            justTurnt = false;
            // make it bad ***************
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = rottenAppleSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Inc all pick ups counter
            ScoreBoard[] scoreBoard = FindObjectsOfType<ScoreBoard>();
            if (rotten)
            {
                scoreBoard[0].addPoints(badPoints, spawnPosition);
            } else {
                scoreBoard[0].addPoints(goodPoints, spawnPosition);
            }
            SoundManager.soundManager.playPickUpSound(rotten);
            // Destroy pick up
            Destroy(gameObject);
        }
    }

    public void incTurnCounter()
    {
        if (!rotten)
        {
            turnCounter++;
            if (turnCounter >= rotTurns)
                {
                    rotten = true;
                    justTurnt = true;
                }
        }
        if (rotten)
        {
            turnCounter++;
            if (turnCounter >= stumpTurns)
            {
                spawnStump();
                Destroy(gameObject);
            }
        }
    }

    public void setFasterPlay() {
        rotTurns = fasterRotTurns;
        stumpTurns = fasterStumpTurns;
    }

    public void spawnStump()
    {
        PlayerBall player = FindObjectOfType<PlayerBall>();
        GameObject newStump;
        newStump = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity) as GameObject;
        if (player.getChompy()) {
            newStump.GetComponent<Collider2D>().isTrigger = true;
        }
    }
}
