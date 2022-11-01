using UnityEngine;

public class SpawnPickUp : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public GameObject stumpChompPreFab;
    Vector3 spawnPosition;
    Vector3 playerStart;
    PlayerBall player;
    bool coolDown;
    int coolDownCount;
    int mask;
    bool rotFaster;
    public int NumStumpsGameOver;

    // Start is called before the first frame update
    void Start()
    {
        playerStart = GetRandomPoint();
        player = FindObjectOfType<PlayerBall>();
        player.transform.position = playerStart;
        SpawnPrefab(1);
        coolDown = false;
        coolDownCount = 0;
        mask = LayerMask.GetMask("Default");
        rotFaster = false;
    }

    // Spawn prefabs and make sure they're not overlapping others
    public void SpawnPrefab(int numToSpawn)
    {
        int i = 0;
        while(i < numToSpawn) {
            spawnPosition = GetRandomPoint();
            Collider2D overlapping = Physics2D.OverlapCircle(spawnPosition, .4f, mask);
            if (!overlapping) {
                if (rotFaster) {
                    GameObject newPickUp;
                    newPickUp = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity) as GameObject;
                    newPickUp.GetComponent<PickUp>().setFasterPlay();
                } else {
                    Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                }
                i++;
            }
        }
    }

    // Does what it says on the box
    public Vector3 GetRandomPoint()
    {
        Vector3 point = new Vector3(Random.Range( -6.66f, 12.1f), Random.Range(-6.0f, 6.0f), 0);
        return point;   
    }

    // Check if recently spawned StumpChomp, count Stumps, spawn Stump, inc counter
    public void checkStumps()
    {
        Stump[] allStumps = FindObjectsOfType<Stump>();
        // Check for end game scenario
        if (allStumps.Length >= NumStumpsGameOver) {
            player.gameOver();
        }
        // Check if able to spawn chump stomp
        if (!coolDown) {
            if (allStumps.Length >= 9)
            {
                 spawnStump();
                 coolDown = true;
            }
        } else {
            coolDownCount++;
        }
        if (coolDownCount >= 4) {
            coolDownCount = 0;
            coolDown = false;
        }
    }
    
    void spawnStump() {
        spawnPosition = GetRandomPoint();
        Collider2D overlapping = Physics2D.OverlapCircle(spawnPosition, .4f, mask);
        if (!overlapping) {
            Instantiate(stumpChompPreFab, spawnPosition, Quaternion.identity);
        } else {
            spawnStump();
        }
    }
    public void setRotFaster() {
        rotFaster = true;
    }
}