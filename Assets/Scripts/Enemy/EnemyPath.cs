using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public Color color = Color.white;

// This code draws lines between the enemy waypoints and allows to change the color of it
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawPath();
    }

    private void DrawPath()
    {
        if (waypoints == null || waypoints.Count < 2)
            return;

        Gizmos.color = color;

        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform current = waypoints[i];
            Transform next = waypoints[(i + 1) % waypoints.Count]; 

            if (current != null && next != null)
                Gizmos.DrawLine(current.position, next.position);
        }
    }
#endif
}
