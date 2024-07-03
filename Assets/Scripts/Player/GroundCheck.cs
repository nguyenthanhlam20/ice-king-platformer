using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGround;
    public bool onIce;
    private CompositeCollider2D box;
    [SerializeField] private LayerMask iceLayerMask;


    void Start()
    {
        box = GetComponent<CompositeCollider2D>();
    }

    void Update()
    {

        if (isOnIce())
        {
            onIce = true;
            Debug.Log("Player is on ice.");
        }
        else
        {
            onIce = false;
            Debug.Log("Player is on ground, but not on ice.");
        }

    }

    private bool isOnIce()
    {
        float extraWeight = 0.05f;
        Vector2 boxCenter = box.bounds.center;
        Vector2 boxSize = new Vector2(0.1f, box.bounds.size.y);

        // Perform the BoxCast
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, extraWeight, iceLayerMask);
        return raycastHit2D.collider != null;
    }


}
