using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 속도 조정 변수수
    [SerializeField]
    private float walkSpeed = 3f;
    [SerializeField]
    private float runSpeed = 10f;
    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isRun = false;
    private bool isGround = true;

    private CapsuleCollider capsuleCollider;

    // 민감도
    // [SerializeField]
    private float lookSensitivity = 3f;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit = 45f;
    private float currentCameraRotationX = 0f;

    // 필요한 컴포넌트트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;


    
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
    }


    void Update()
    {
        isGround();
        TryJump();
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void IsGround()
    {
        isGround = Physics.RayCast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if(Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        myRigid.linearVelocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    // 좌우 캐릭터 회전전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    // 상하 카메라 회전전
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);


        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }


}
