using TMPro;
using UnityEngine;

public class PointsPopUp : MonoBehaviour
{

    public float moveYSpeed;
    public float fadeOutTimer;
    TextMeshPro textObject;
    private Color textColor;
    public float fadeOutSpeed;


    void Start() {

    }
    void Awake() {
        textObject = transform.GetComponent<TextMeshPro>();
    }

    public void SetUp(int points) {
        textObject.SetText(points.ToString());
        if (points <= 0) {
            textObject.color = Color.black;
            textColor = Color.black;
        } else {
            textColor = textObject.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime; 

        fadeOutTimer -= Time.deltaTime;
        if (fadeOutTimer < 0) {
            textColor.a -= fadeOutSpeed * Time.deltaTime;
            textObject.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }

    }
}
