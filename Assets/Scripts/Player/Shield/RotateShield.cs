using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateShield : MonoBehaviour
{
    private Camera mainCam;
    //current mouse position
    private Vector3 mousePos;
    public Move playerMoving;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); //set the mousePosition whenever mouse cursor moving on the screen

        Vector3 mouseRotation = mousePos - transform.position; //rotate the point where mouse cursor move

        //set the rotation when play not facing right
        if (!playerMoving.facingRight)
        {
            mouseRotation = -mouseRotation;
        }

        float rotate_Z = Mathf.Atan2(mouseRotation.y, mouseRotation.x) * Mathf.Rad2Deg; //calculate the rotate angle value of the point on the z-axis for am angle between y-axis and x-axis

        transform.rotation = Quaternion.Euler(0, 0, rotate_Z); //rotate an angle with value = rotate_Z between y-axis and x-axis
    }
}
