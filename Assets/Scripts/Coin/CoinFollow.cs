using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFollow : MonoBehaviour
{
    public GameObject ghost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Lấy vị trí hiện tại của đồng xu
        Vector2 currentPosition = transform.position;
        // Lấy vị trí hiện tại của con ma
        Vector2 ghostPosition = ghost.transform.position;
        // Chỉ cập nhật trục x, giữ nguyên trục y của đồng xu
        Vector2 newPosition = new Vector2(
            Mathf.MoveTowards(currentPosition.x, ghostPosition.x, 4f * Time.deltaTime),
            currentPosition.y
        );
        // Cập nhật vị trí mới cho đồng xu
        transform.position = newPosition;
    }
}
