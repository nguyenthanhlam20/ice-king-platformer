using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorAreaController : MonoBehaviour
{
    public Text doorText;
    public Collider2D gateCollider;
    public Animator animator;

    private void Awake()
    {
        doorText.text = "";
        doorText.enabled = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        gateCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Key"))
        {
            StartCoroutine(textTiming());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Key") && Input.GetKey(KeyCode.E))
        {
            StartCoroutine(GateTiming());

        }
    }

    private IEnumerator textTiming()
    {
        doorText.enabled = true;
        if (CoinController.instance.coinCout == 3)
        {
            doorText.text = "Press E to activate the Portal"; // appear text

        }
        else
        {
            doorText.text = "You do not have enough coin!!!"; // appear text

        }
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
        doorText.enabled = false;
    }

    private IEnumerator GateTiming()
    {
        animator.SetBool("IsOpen", true);
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.68f);
        gateCollider.enabled = true;
    }
}
