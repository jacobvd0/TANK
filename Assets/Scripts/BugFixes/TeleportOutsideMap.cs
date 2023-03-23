using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOutsideMap : MonoBehaviour
{
    // add this to an invisible cube with a box collider on it so if the player goes outside it send them to 0, 3, 0 which should be fine

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = new Vector3 (0.0f, 3.0f, 0.0f);
        }
    }
}
