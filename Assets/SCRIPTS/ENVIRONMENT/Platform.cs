using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public Transform[] pathPoint = new Transform[] { };
    public float[] delays;
    public float speed;
    public iTween.EaseType ease;
    private int pointIdx;


    // Use this for initialization
    void Start () {
        pointIdx = -1;
        NextPath();
    }

    void OnDrawGizmos()
    {
        if (pathPoint.Length < 1)
            return;

        Gizmos.color = new Color(1, 0.8f, 1);
        Transform[] tPath = new Transform[pathPoint.Length + 1];
        for (int i = 0; i < tPath.Length; i++)
        {
            if (i != pathPoint.Length)
            {
                if (pathPoint[i] == null)
                {
                    List<Transform> positions = new List<Transform>();
                    foreach (Transform pos in pathPoint)
                    {
                        if (pos != null)
                            positions.Add(pos);
                    }
                    pathPoint = positions.ToArray();
                    return;
                }
                tPath[i] = pathPoint[i];
                Gizmos.DrawWireSphere(pathPoint[i].position, 0.3f);
            }
            else
                tPath[i] = pathPoint[0];
        }
        iTween.DrawLine(tPath, new Color(0.8f, 1, 0.8f));
    }

    public void NextPath()
    {
        if (GetComponent<iTween>() != null)
        {
            iTween tween = GetComponent<iTween>();
            iTween.tweens.Remove(tween);
            Destroy(tween);
        }

        pointIdx++;
        if (pointIdx >= pathPoint.Length)
            pointIdx = 0;

        iTween.MoveTo(gameObject, iTween.Hash("delay", delays[pointIdx], "speed", speed, "position", pathPoint[pointIdx], "easetype", ease, "oncomplete", "NextPath"));
    }
}
