using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public Transform startPoint;
    public Transform[] endPoints;

    void OnDrawGizmos()
    {
        if (endPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (var endPoint in endPoints)
            {
                Gizmos.DrawRay(endPoint.position, endPoint.forward * 5); // Длина луча 5 единиц
            }
        }
    }
}