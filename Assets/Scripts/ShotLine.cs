using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShotLine : MonoBehaviour
{

    LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 endPoint)
    {
        line.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = startPoint;
        points[1] = Vector3.MoveTowards(startPoint, endPoint, 4.0f);

        line.SetPositions(points);
    }

    public void EndLine()
    {
        line.positionCount = 0;
    }
}