using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour, IDataAction
{
    public IItemCollection actionable;
    public MonoBehaviour actionScript;
    private bool isCollected;
    AudioManager audioManager;


    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        actionable = actionScript as IItemCollection;
        isCollected = false;
        if (actionable == null)
        {
            Debug.LogError("The assigned script does not implement IActionable");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            audioManager.PlaySFX(audioManager.collectcoin);
            CoinController.instance.coinCout++;
            isCollected = true;
            actionable.activeItem();
            Destroy(gameObject);
        }
    }

    public void LoadData(GameData gameData)
    {
        gameData.itemCollected.TryGetValue(id, out isCollected);
        if (isCollected)
        {
            CoinController.instance.coinCout = gameData.coinNumber;
            actionable.activeItem();
            Destroy(gameObject);
        }
    }

    public void SaveData(ref GameData gameData)
    {
        Debug.Log("Test collected" + isCollected);
        if (gameData.itemCollected.ContainsKey(id))
        {
            gameData.itemCollected.Remove(id);
        }
        gameData.itemCollected.Add(id, isCollected);
        gameData.coinNumber = CoinController.instance.coinCout;
    }
}
