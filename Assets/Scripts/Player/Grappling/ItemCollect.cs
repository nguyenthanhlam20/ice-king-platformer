using UnityEngine;

public class ItemCollect : MonoBehaviour, IDataAction
{

    [SerializeField] GameObject tutorial;

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
        if(tutorial != null) tutorial.SetActive(false);
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
            if (tutorial != null) tutorial.SetActive(true);
            audioManager.PlaySFX(audioManager.collectitem);
            isCollected = true;
            actionable.activeItem();
            Destroy(gameObject);
        }
    }

    public void LoadData(GameData gameData)
    {
        gameData.itemCollected.TryGetValue(id, out isCollected);
        if(isCollected)
        {
            actionable.activeItem();
            Destroy(gameObject);
        }
    }

    public void SaveData(ref GameData gameData)
    {
        if (gameData.itemCollected.ContainsKey(id))
        {
            gameData.itemCollected.Remove(id);
        }
        gameData.itemCollected.Add(id, isCollected);
    }
}
