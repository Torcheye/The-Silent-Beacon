using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private bool enableSprint;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float fallSpeed = 1.2f;
    [SerializeField] private float maxUpAngle = 80;
    [SerializeField] private float maxDownAngle = -80;
    [SerializeField] private float footstepDist;
    
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private HeadBobbing headBobbing;

    private const float GRAVITY = -9.81f;
    private CharacterController _controller;
    private float _velocityY;
    private float _camRotX;
    private bool _isGroundedLastFrame;
    private Vector3 _lastFootPos;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _lastFootPos = transform.position;
        InputManager.Instance.OnWASD += UpdateMove;
        InputManager.Instance.OnMouse += UpdateViewRot;
    }

    private void Update()
    {
        // footstep
        if (Vector3.SqrMagnitude(transform.position - _lastFootPos) > footstepDist * footstepDist 
            && _controller.isGrounded 
            && _controller.velocity.magnitude > 0.1f)
        {
            //int id = Random.Range(0, footsteps.Count);
            //_source.PlayOneShot(footsteps[id]);
            _lastFootPos = transform.position;
        }
    }

    private void UpdateMove(Vector2 input)
    {
        if (!_controller.enabled)
            return;
        
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        if (move.sqrMagnitude > 1)
            move.Normalize();
        move *= Input.GetKey(KeyCode.LeftShift) && enableSprint ? moveSpeed * 1.5f : moveSpeed;
        
        headBobbing.movementSpeed = move.magnitude;
        
        if (_controller.isGrounded && _velocityY < 0)
        {
            _velocityY = 0f;
        }
        _velocityY += GRAVITY * Time.deltaTime * fallSpeed;
        
        move.y = _velocityY;
        _controller.Move( Time.deltaTime * move);
    }

    private void UpdateViewRot(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        _camRotX -= mouseY;
        _camRotX = Mathf.Clamp(_camRotX, maxDownAngle, maxUpAngle);

        cameraTransform.localRotation = Quaternion.Euler(_camRotX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}