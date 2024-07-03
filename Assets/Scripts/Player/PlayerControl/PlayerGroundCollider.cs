using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCollider : MonoBehaviour
{
    public bool isGrounded = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 0) //0 means the layer ofthe tilemap pre-define in unity which charater interact
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 0) //0 means the layer ofthe tilemap pre-define in unity which charater interact
        {
            isGrounded = false;
        }
    }
}
