using System;
using Cinemachine;
using StylizedWater2;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardAcceleration = 1;
    [SerializeField] private float rotationAcceleration = 12;
    [Header("Buoyancy")]
    [SerializeField] private float rollStrength = 1;
    [SerializeField] private float floatStrength = 1;
    [SerializeField] private float floatOffset = 0.5f;
    [SerializeField] private WaterObject waterObject;
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera boatmanCam;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector2 cameraFovRange = new Vector2(60, 90);
    [SerializeField] private float maxFovSpeed = 3;
    [SerializeField] private bool fovChange = false;
    [SerializeField] private float maxUpAngle = 80;
    [SerializeField] private float maxDownAngle = -80;
    
    private float _camRotX;
    private float _camRotY;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        InputManager.Instance.OnWASD += Move;
        InputManager.Instance.OnMouse += UpdateViewRot;
    }

    private void Update()
    {
        if (fovChange)
        {
            boatmanCam.m_Lens.FieldOfView =
                Mathf.Lerp(cameraFovRange.x, cameraFovRange.y, _rb.velocity.magnitude / maxFovSpeed);
        }
    }

    private void FixedUpdate()
    {
        var waterHeight = Buoyancy.SampleWaves(transform.position, waterObject, rollStrength, false, out Vector3 waterNormal);
        transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, waterNormal), waterNormal);
        // lerp to water height
        var newY = Mathf.Lerp(transform.position.y, waterHeight + floatOffset, Time.fixedDeltaTime * floatStrength);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void Move(Vector2 input)
    {
        var forwardSpeed = input.y * forwardAcceleration * Time.deltaTime;
        var rotationSpeed = input.x * rotationAcceleration * Time.deltaTime;
        
        _rb.AddForce(transform.forward * forwardSpeed, ForceMode.Force);
        _rb.AddTorque(Vector3.up * rotationSpeed, ForceMode.Force);
    }
    
    private void UpdateViewRot(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        _camRotX -= mouseY;
        _camRotY += mouseX;
        _camRotX = Mathf.Clamp(_camRotX, maxDownAngle, maxUpAngle);

        cameraTransform.localRotation = Quaternion.Euler(_camRotX, _camRotY, 0);
        //cameraTransform.Rotate(Vector3.up * mouseX);
    }
}
