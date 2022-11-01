using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public Rigidbody2D ball;
    public Sprite regularBall;
    public Sprite chompyBall;
    public float scaleDownPerTick;
    public float forceMultiplier;
    public float minDistance;
    public float maxDistance;
    public float defaultScale;
    public TextMeshProUGUI gameOverText;
    public Button imSureButton;
    public Button noButton;
    public Text sureText;
    float dragDistance;
    Vector3 defaultScaleVector;
    Vector3 ballScale;
    Vector3 ballCenter;
    Vector3 scaleDownRate;
    Vector3 mouseLocation;
    Vector3 mouseLocationRelease;
    Vector3 clickLocation;
    Vector3 currentLocation;
    Vector2 dragDirection;
    Vector3 releaseLocation;
    ShotLine shotLine;
    Vector2 forceMultiplierVector;
    SpawnPickUp spawner;
    int numChompy;
    int numClicks;
    public bool newChompy;
    public bool isMoving;
    bool goodClick;
    Camera cam;
    public bool isGameOver;
    public StumpCheck stumpUIChecker;

    void Awake()
    {
        Application.targetFrameRate = 120;
    }

    // Start is called before the first frame update
    void Start()
    {   
        cam = Camera.main;
        shotLine = GetComponent<ShotLine>();
        // Set ball size
        defaultScaleVector = new Vector3(defaultScale, defaultScale, defaultScale);
        ball.transform.localScale = defaultScaleVector;
        // Make vector of rate for scaling down ball
        scaleDownRate = new Vector3(scaleDownPerTick, scaleDownPerTick, 0);
        spawner = FindObjectOfType<SpawnPickUp>();
        numChompy = 0;
        newChompy = false;
        isMoving = false;
        forceMultiplierVector = new Vector2(forceMultiplier, forceMultiplier);
        goodClick = false;
        numClicks = 0;
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {

        // Set bool value for if the ball is in motion
        if (ball.velocity != Vector2.zero) {
            isMoving = true;
        } else {
            isMoving = false;
        }

        // Rotate sprite to face forward
        Vector2 moveDirection = GetComponent<Rigidbody2D>().velocity;
        if (moveDirection != Vector2.zero) {
            if (moveDirection.x > 0) {
                //GetComponent<SpriteRenderer>().flipY = false;
                GetComponent<SpriteRenderer>().flipX = false;
                float angle = (Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg);
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            } else {
                //GetComponent<SpriteRenderer>().flipY = true;
                GetComponent<SpriteRenderer>().flipX = true;
                float angle = (Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg) + 180;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
         }

        // Only do this is ball is stationary, cheating is possible with small clicks while ball is moving (dodging spawner) without this
        if (!getIsMoving()) {
            // Disable gameplay if game is over
            if (!isGameOver) {
                // Run when mouse button is held down
                if (Input.GetMouseButtonDown(0)) {
                    // Store ball center to recenter if scaling down after scaling into object moves off center
                    ballCenter = ball.position;
                    // Get click location and convert to world position
                    Vector3 thisClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                    clickLocation = cam.ScreenToWorldPoint(thisClick);
                    clickLocation.z = -5.0f;
                    newChompy = false;   
                    if (!EventSystem.current.IsPointerOverGameObject()){
                        goodClick = true;;
                    }
                }
            }
        
            // Fire when mouse button is released
            if (Input.GetMouseButtonUp(0) && goodClick){
                // Get screen location of cursor and convert to world position, use it to calculate scale and apply
                mouseLocationRelease = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                releaseLocation = Camera.main.ScreenToWorldPoint(mouseLocationRelease);
                releaseLocation.z = -5.0f;
                // Get drag direction (atually calculated to return shot direction)
                dragDirection = (clickLocation - releaseLocation);
                // Calculate force, clamp it, and launch the ball
                Vector2 forceToApply = dragDirection * forceMultiplierVector;
                forceToApply = Vector2.ClampMagnitude(forceToApply, 928.0f);
                if (ball.transform.localScale.x > 0.4f) {
                    ball.AddForce(forceToApply, ForceMode2D.Force);
                }
                // Disappear shot line
                shotLine.EndLine();
                // Decrease num of turns to rot and stump
                if (numClicks == 50) {
                   spawner.setRotFaster();
                }      
                // Increase numClicks to increase difficulty 
                numClicks++;
                // Reset params
                ballCenter = Vector3.zero;
                clickLocation = Vector3.zero;
                dragDirection = Vector2.zero;
                dragDistance = 0.0f;
                goodClick = false;
            }
                
            if (clickLocation != Vector3.zero && goodClick) {
                // Get screen location of cursor and convert to world position, use it to calculate scale and apply
                mouseLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                currentLocation = cam.ScreenToWorldPoint(mouseLocation);
                currentLocation.z = -5.0f;
                // Get and clamp a drag distance, and cap scale
                dragDistance = Vector3.Distance(clickLocation, currentLocation); 
                dragDistance = Mathf.Clamp(dragDistance, minDistance, maxDistance);
                // Create vector for new ball scale and set it (increased a pinch to prevent movement without stop on default size)
                float bumpedScale = defaultScale + .02f;
                ballScale = new Vector3(bumpedScale * dragDistance, bumpedScale * dragDistance, 1);
                ball.transform.localScale = ballScale;
                // Rotate sprite to face away from drag direction
                if (currentLocation.x > clickLocation.x) {
                    GetComponent<SpriteRenderer>().flipX = true;
                    float dragAngle = (Mathf.Atan2(currentLocation.y - clickLocation.y, currentLocation.x - clickLocation.x) * Mathf.Rad2Deg);
                    transform.rotation = Quaternion.AngleAxis(dragAngle, Vector3.forward);
                } else {
                    GetComponent<SpriteRenderer>().flipX = false;
                    float dragAngle = (Mathf.Atan2(currentLocation.y - clickLocation.y, currentLocation.x - clickLocation.x) * Mathf.Rad2Deg) + 180;
                    transform.rotation = Quaternion.AngleAxis(dragAngle, Vector3.forward);
                }
                // Keeps ball centered if sizing down after moving from scaling into static object
                ball.position = ballCenter;
                // Draw drag line
                shotLine.RenderLine(clickLocation, currentLocation);   
            }
        }

        // Only execute if ball is not yet back to default size
        if (ball.transform.localScale.x > defaultScale)
        {   
            // Shrink down incrementally, catching when it goes under default size
            ball.transform.localScale -= scaleDownRate * Time.deltaTime;
        }
        // Check if at or under default size
        if (ball.transform.localScale.x <= defaultScale && getIsMoving())
        {
            // Set ball to default size because it will try to go under
            ball.transform.localScale = defaultScaleVector;
            ball.velocity = new Vector2(0.0f, 0.0f);  // *** Anything after this line, within this function, will be executed after the ball stops ****************
            // Chompy
            if (numChompy > 0) {
                if(!newChompy) {
                numChompy--;
                    if (numChompy == 0) {
                    deactivateStumps();
                    ball.GetComponent<SpriteRenderer>().sprite = regularBall;
                    }
                } else {
                newChompy = false;
                }
            }
            // Inc all pick ups counter
            PickUp[] allPickUps = FindObjectsOfType<PickUp>();
            foreach (PickUp pickUp in allPickUps)
            {
                pickUp.incTurnCounter();
            }
            // Call function to spawn new pickup(s)
            calcAndSpawn();
            // Check stumps and spawn StumpChomp etc.
            spawner.checkStumps();
            // Update tiny stump UI
            stumpUIChecker.updateStumpUI();
        }
    }

    // Increase number of prefabs spawned as game progresses
    void calcAndSpawn() {
        if (numClicks < 3) {
            spawner.SpawnPrefab(1);
        } else if (numClicks >= 3 &&  numClicks < 10) {
            spawner.SpawnPrefab(2);
        } else if (numClicks >= 10 && numClicks < 20) {
            spawner.SpawnPrefab(3);
        } else if (numClicks >= 20 && numClicks < 35) {
            spawner.SpawnPrefab(4);
        } else if (numClicks >= 35 && numClicks < 50) {
            spawner.SpawnPrefab(5);
        } else if (numClicks >= 50 && numClicks < 75) {
            spawner.SpawnPrefab(6);
        } else if (numClicks >= 75 && numClicks < 100) {
            spawner.SpawnPrefab(7);
        } else if (numClicks >= 100 && numClicks < 125) {
            spawner.SpawnPrefab(8);
        } else if (numChompy >= 125 && numClicks < 150) {
            spawner.SpawnPrefab(9);
        } else {
            spawner.SpawnPrefab(10);
        }
    }

    void OnCollisionEnter2D(Collision2D otherObject) {
        if (otherObject.gameObject.tag == "Stump" || otherObject.gameObject.tag == "Wall") {
            if (getIsMoving()) {
                SoundManager.soundManager.playBounce();
            }
        }
    }

    void deactivateStumps() {
        Stump[] myStumps = FindObjectsOfType<Stump>();
        foreach (Stump stump in myStumps) {
            stump.GetComponent<Collider2D>().isTrigger = false;
        }
    }

    void activateStumps() {
        Stump[] myStumps = FindObjectsOfType<Stump>();
        foreach (Stump stump in myStumps) {
            stump.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    public bool getChompy()
    {
        if (numChompy > 0) {
            return true;
        } else {
            return false;
        }
        
    }

    public void addChompy()
    {
        if (numChompy == 0) {
            activateStumps();
            ball.GetComponent<SpriteRenderer>().sprite = chompyBall;
        }
        newChompy = true;
        numChompy++;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }
    
    public void gameOver() {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void showSureButton() {
        imSureButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
        sureText.gameObject.SetActive(true);
    }

    public void hideButtons() {
        imSureButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        sureText.gameObject.SetActive(false);
    }
}