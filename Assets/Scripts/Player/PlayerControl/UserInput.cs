using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private float horizontalInput;
    private PlayerMovement playerMovement;
    private bool jumPressed;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); //return value 0 to -1 when use left arrow or 0 to 1 when use right arrow
        if (!jumPressed)
        {
            jumPressed = Input.GetButtonDown("Jump");
        }
    }

}
