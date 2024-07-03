using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceCloth : MonoBehaviour, IItemCollection
{
    private Freeze freeze;
    private bool canActiveCloth = false;

    private void Awake()
    {
        freeze = GetComponent<Freeze>();
    }

    private void Update()
    {
        if (canActiveCloth)
        {
            ActiveCloth();
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            frezze.ActiveCloth();
            Destroy(gameObject);
        }
    }*/
    public void ActiveCloth()
    {
        freeze.ActiveCloth();
    }

    public void activeItem()
    {
        freeze.ActiveCloth();
    }
}
