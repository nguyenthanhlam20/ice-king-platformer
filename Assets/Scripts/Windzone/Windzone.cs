using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windzone : MonoBehaviour
{
    public ParticleSystem leafParticleSystem; // Tham chiếu đến Particle System của lá
    public float windForce = 8f; // Lực của gió
    public float changeDirectionInterval = 3f; // Thời gian giữa các lần đổi chiều

    private Vector2 windDirection = Vector2.left; // Hướng gió ban đầu
    private ParticleSystem.VelocityOverLifetimeModule velocityModule;

    void Start()
    {
        velocityModule = leafParticleSystem.velocityOverLifetime;
        StartCoroutine(ChangeWindDirection());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
        {
            windForce+= Time.timeScale * 0.05f;
            if (windForce > 8)
            {
                windForce = 8;
            }
            // Gọi hàm điều chỉnh lại Particle System
            AdjustLeafParticleSystem();
            Move moveScript = other.GetComponent<Move>();
            if (moveScript != null && !moveScript.isGrounded) // Chỉ áp dụng lực gió khi không chạm đất
            {
                other.attachedRigidbody.AddForce(windDirection * windForce);
               
            }
        }
    }

    private IEnumerator ChangeWindDirection()
    {
        while (true)
        {
            yield return new WaitForSeconds(changeDirectionInterval);

            // Đổi chiều gió
            windDirection = -windDirection;
            windForce = 0;


            // Đợi một chút để gió đổi chiều
            yield return new WaitForSeconds(0.05f);

            // Tiếp tục vòng lặp
        }
    }

    void AdjustLeafParticleSystem()
    {
        // Thiết lập tốc độ theo hướng X của gió nhân với windForce
        velocityModule.x = new ParticleSystem.MinMaxCurve(windDirection.x * windForce);

        // Thiết lập tốc độ theo hướng Y của gió nhân với windForce
        velocityModule.y = new ParticleSystem.MinMaxCurve(windDirection.y * windForce);

        // Nếu bạn cần sử dụng hướng Z, bạn có thể thêm vào đây
        // velocityModule.z = new ParticleSystem.MinMaxCurve(...);
    }


}
