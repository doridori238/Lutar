using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class PlayersDistance : MonoBehaviour
{
    public GameObject rightTarget;
    public GameObject leftTarget;
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private float mDistance;
    [SerializeField] private LayerMask target;

    public float hight;

    private void FixedUpdate()
    {
        Vector3 vec = rightTarget.transform.position - leftTarget.transform.position;
        
        RaycastHit hit;
        if (Physics.Raycast(leftTarget.transform.position, vec, out hit, 100, target))
        {
            if (Physics.Raycast(hit.point, -vec, out hit, 100, target))
            {
                transform.position = hit.point + vec.normalized * hit.distance / 2;

                if (hit.distance >= mDistance)
                    cameraObj.transform.position = transform.position + new Vector3(0, hight, 0) - cameraObj.transform.forward * hit.distance;

                transform.LookAt(new Vector3(rightTarget.transform.position.x, transform.position.y, rightTarget.transform.position.z));
            }
        }
    }

    

}
