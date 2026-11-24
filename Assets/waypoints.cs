using UnityEngine;

public class waypoints : MonoBehaviour
{
    [Range(0f, 2f) ]
    [SerializeField] private float waypointSize = 1f;
    [Header("Path Settigs")]
    [SerializeField] private bool canLoop = true;

    [SerializeField] private bool isMovingFoward = true;
  


    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, waypointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);

        }
        if(canLoop)
        {
            Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
        }
    }

    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
       if(currentWaypoint == null)
        {
            return transform.GetChild(0);

        }

        int currentIndex = currentWaypoint.GetSiblingIndex();
        int nextIndex = currentIndex;

        if(isMovingFoward)
        {
            nextIndex += 1;

            if(nextIndex == transform.childCount)
            {
                if (canLoop)
                {
                    nextIndex = 0;
                }
                else
                {
                    nextIndex -= 1;
                }
            }
        }
        else
        {
            nextIndex -= 1;

            if(nextIndex < 0)
            {
                if (canLoop)
                {
                    nextIndex = transform.childCount - 1;
                }
                else
                {
                    nextIndex += 1;
                }
            }
        }
        return transform.GetChild(nextIndex);





    }
}
