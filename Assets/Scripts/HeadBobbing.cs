using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    public float movementSpeed;

    [SerializeField] private float baseBobFrequency = 1.5f;  // Base frequency of the bobbing motion
    [SerializeField] private float baseBobHeight = 0.1f;     // Base vertical bobbing amount
    [SerializeField] private float baseBobWidth = 0.05f;     // Base horizontal bobbing amount
    [SerializeField] private float bobSmoothing = 4.0f;      // How smooth the bobbing motion is
    [SerializeField] private float minimalBobAmount = 0.02f; // Minimal bobbing effect when not moving
    [SerializeField] private float noiseScale = 0.02f;       // How much randomness to add to the bobbing

    private float _timer = 0.0f;
    private Vector3 _initialPosition;   // The initial position of the camera
    private float _randomOffset;        // Random seed to introduce more randomness in motion

    void Start()
    {
        _initialPosition = transform.localPosition;   // Save the initial position of the camera
        _randomOffset = Random.Range(0f, 100f);       // Initialize a random seed for Perlin noise
    }

    void Update()
    {
        if (isActiveAndEnabled)
        {
            // Increase timer based on movementSpeed and baseBobFrequency
            _timer += Time.deltaTime * (baseBobFrequency + movementSpeed);

            // Adjust bobbing intensity based on movementSpeed, but apply a minimal bobbing effect
            float adjustedBobHeight = Mathf.Max(baseBobHeight * movementSpeed, minimalBobAmount);
            float adjustedBobWidth = Mathf.Max(baseBobWidth * movementSpeed, minimalBobAmount);

            // Major rhythm from sine wave (structured bobbing)
            float majorBobbingMotionY = Mathf.Sin(_timer) * adjustedBobHeight;
            float majorBobbingMotionX = Mathf.Cos(_timer * 2) * adjustedBobWidth;

            // Minor randomness from Perlin noise (small irregularities in bobbing)
            float noiseBobbingMotionY = (Mathf.PerlinNoise(_timer, _randomOffset) - 0.5f) * 2 * noiseScale;
            float noiseBobbingMotionX = (Mathf.PerlinNoise(_randomOffset, _timer) - 0.5f) * 2 * noiseScale;

            // Combine major bobbing with minor randomness
            float combinedBobbingMotionY = majorBobbingMotionY + noiseBobbingMotionY;
            float combinedBobbingMotionX = majorBobbingMotionX + noiseBobbingMotionX;

            Vector3 targetPosition = new Vector3(_initialPosition.x + combinedBobbingMotionX, _initialPosition.y + combinedBobbingMotionY, _initialPosition.z);

            // Smoothly move the camera to the target bobbing position
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * bobSmoothing);
        }
        else
        {
            // Reset the timer and smoothly return the camera to its original position when the player is not moving
            _timer = 0.0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, _initialPosition, Time.deltaTime * bobSmoothing);
        }
    }
}
