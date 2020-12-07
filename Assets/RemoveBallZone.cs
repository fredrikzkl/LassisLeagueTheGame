using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBallZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            other.GetComponent<BallController>().BallOutOfBounds(1f);
        }
    }
    
}
