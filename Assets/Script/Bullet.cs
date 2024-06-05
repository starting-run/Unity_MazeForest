using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 3.0f; // 총알이 3초 후에 사라지도록 설정

    private void Start()
    {
        // 일정 시간 후에 총알을 파괴
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 시 총알을 파괴
        if (!(collision.gameObject.CompareTag("Player")))
        {
            Destroy(gameObject);
        }
        
    }
}
