using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public StarterAssets.StarterAssetsInputs input;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    public GameObject uiCanvas;  // Canvas를 참조하는 변수
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera attackCamera;

    private bool isAttackingIdle = false;
    private Camera mainCameraComponent;

    [Header("Aim")]
    [SerializeField]
    private GameObject aimObj;
    [SerializeField]
    private float aimObjDis = 10f;
    [SerializeField]
    private LayerMask targetLayer;

    private void Awake()
    {
        mainCameraComponent = Camera.main;
    }

    private void Update()
    {
        HandleAttackAndFire();
    }

    private void HandleAttackAndFire()
    {
        if (input.attack)
        {
            if (!isAttackingIdle)
            {
                // AttackIdle 애니메이션을 시작합니다.
                animator.SetBool("IsAttackingIdle", true);
                isAttackingIdle = true;

                // UI Canvas 활성화
                uiCanvas.SetActive(true);

                // 시네머신 카메라 전환
                mainCamera.Priority = 0;
                attackCamera.Priority = 1;
            }

            if (input.fire)
            {
                animator.SetTrigger("Attack");
                FireBullet();
                input.fire = false;
            }
        }
        else
        {
            if (isAttackingIdle)
            {
                // AttackIdle 애니메이션을 중지합니다.
                animator.SetBool("IsAttackingIdle", false);
                isAttackingIdle = false;

                // UI Canvas 비활성화
                uiCanvas.SetActive(false);

                // 시네머신 카메라 전환
                mainCamera.Priority = 1;
                attackCamera.Priority = 0;
            }
        }
    }

    private void FireBullet()
    {
        Vector3 targetPosition = Vector3.zero;
        Transform camTransform = mainCameraComponent.transform;
        RaycastHit hit;

        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer, QueryTriggerInteraction.Ignore))
        {
            targetPosition = hit.point;
            aimObj.transform.position = hit.point;
        }
        else
        {
            targetPosition = camTransform.position + camTransform.forward * aimObjDis;
            aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
        }

        Vector3 direction = (targetPosition - firePoint.position).normalized;

        // 플레이어를 타겟 방향으로 회전시킵니다.
        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // 총알을 생성하고 발사합니다.
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }
}
