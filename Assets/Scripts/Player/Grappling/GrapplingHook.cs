using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public Tutorial_GrapplingRope grappleRope;
    public Move playerMoving;


    [Header("Object Ref:")]
    public GameObject Player;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public DistanceJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;

    [Header("Grappling State:")]
    public bool canGrappling = false;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    public bool activeGrappling;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && activeGrappling)
        {
            SetGrapplePoint();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && canGrappling)
        {
            StopGrappling();
        }
       
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                {
                    audioManager.PlaySFX(audioManager.grappling);
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)firePoint.position;
                    canGrappling = true;
                    grappleRope.enabled = true;
                }
               /* else
                {
                    grapplePoint = (Vector2)firePoint.position + distanceVector.normalized * maxDistance;
                    grappleRope.enabled = true;
                    canGrappling = false;
                }*/
            }
        }
        /*else
        {
            grapplePoint = (Vector2)firePoint.position + distanceVector.normalized * maxDistance;
            grappleRope.enabled = true;
            canGrappling = false;
        }*/
    }

    public void Grapple()
    {
        m_springJoint2D.autoConfigureDistance = false;

        if (autoConfigureDistance)
        {
            m_springJoint2D.autoConfigureDistance = true;
        }

        m_springJoint2D.connectedAnchor = grapplePoint;
        m_springJoint2D.enabled = true;
    }

    public void StopGrappling()
    {
        audioManager.PlaySFX(audioManager.grappling);
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        canGrappling = false;
    }
    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}
