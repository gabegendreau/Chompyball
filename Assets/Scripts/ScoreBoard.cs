using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    int score;
    int pointsToAdd;
    public Text textElement;
    public Text chompyText;
    PlayerBall player;
    [SerializeField] private Transform PointPopUp;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBall>();
        score = 0;
        pointsToAdd = 0;
    }

    void Update()
    {
        if (player.getChompy())
        {
            chompyText.enabled = true;
        } else {
            chompyText.enabled = false;
        }
    }

    // Add points
    public void addPoints(int value, Vector3 pickUpLocation)
    {
        pointsToAdd = 0;
        if (player.getChompy()){
            pointsToAdd = Mathf.Abs(value);
        } else {
            pointsToAdd = value;
        }
        score += pointsToAdd;
        textElement.text = getScore().ToString();
        spawnPointsPopUp(pickUpLocation);
    }

    // Get Score
    public int getScore()
    {
        return score;
    }

    // Spawn point value
    void spawnPointsPopUp(Vector3 popUpLocation)
    {
        Transform PointsPopUpTransform = Instantiate(PointPopUp, popUpLocation, Quaternion.identity);
        PointsPopUp pointsPopUp = PointsPopUpTransform.GetComponent<PointsPopUp>();
        pointsPopUp.SetUp(pointsToAdd);
    }
}

// Make a numClick score multiplier? ****************************************************************************