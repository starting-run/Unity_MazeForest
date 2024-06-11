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
    public ScenarioEngine engine;

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

        string script = Resources.Load<TextAsset>("GameStart").ToString();
        StartCoroutine(engine.PlayScript(script));
        Invoke("ShowBtn", 3); 
    }

    public void ShowBtn()
    {
        GameManager.Instance.ShowBtnUI();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        if (GameManager.Instance.isGameOver == false)
        {//기본 상태
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //LeftALT 클릭동안 마우스 활성화
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        HandleAttackAndFire();
        HandleMovement();
    }

    private void HandleAttackAndFire()
    {
        if (input.attack)
        {
            if (!isAttackingIdle)
            {
                animator.SetBool("IsAttackingIdle", true);
                isAttackingIdle = true;

                // 에임 활성화
                uiCanvas.SetActive(true);

                // 시네머신 카메라 전환
                mainCamera.Priority = 0;
                attackCamera.Priority = 1;
            }

            if (input.fire)
            {
                animator.SetFloat("AttackSpeed", 1f);
                FireBullet();
                input.fire = false;
            }
        }
        else
        {
            if (isAttackingIdle)
            {
                animator.SetBool("IsAttackingIdle", false);
                isAttackingIdle = false;

                // 에임 비활성화
                uiCanvas.SetActive(false);

                // 시네머신 카메라 전환
                mainCamera.Priority = 1;
                attackCamera.Priority = 0;
            }
        }
    }

    private void HandleMovement()
    {
        bool isWalking = input.move != Vector2.zero;

        if (isAttackingIdle)
        {
            animator.SetFloat("AttackSpeed", isWalking ? 0.5f : 0f);
        }
        else
        {
            animator.SetBool("isWalking", isWalking);
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

        // 플레이어를 타겟 방향으로 회전
        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // 총 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
    }
}
