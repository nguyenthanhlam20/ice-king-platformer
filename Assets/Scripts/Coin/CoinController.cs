using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CoinController : MonoBehaviour, IItemCollection
{
    public static CoinController instance { get; private set; }
    public int coinCout;
    public Text coinText;
    private CircleCollider2D CircleCollider2D;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        instance = this;

    }


    // Start is called before the first frame update
    void Start()
    {
        CircleCollider2D = GetComponent<CircleCollider2D>();
        CircleCollider2D.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = ": " + coinCout.ToString();
        if (coinCout == 3)
        {
            CircleCollider2D.enabled = true;
        }
    }

    public void ResetCoin()
    {
        coinCout = 0;
        CircleCollider2D.enabled = false;
    }

    public void activeItem()
    {
        if (coinCout == 3)
        {
            CircleCollider2D.enabled = true;
        }
    }


}
