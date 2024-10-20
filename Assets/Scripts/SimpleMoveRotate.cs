using UnityEngine;

public class SimpleMoveRotate : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveMagnitude = 1f;
    public float rotateSpeed = 100f;
    public bool moveHorizontally = true;
    public bool moveVertically = false;
    public bool enableRotation = false;

    private Vector3 startPosition;
    private float movementFactor;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (moveHorizontally)
        {
            movementFactor = Mathf.Sin(Time.time * moveSpeed) * moveMagnitude;
            transform.position = startPosition + new Vector3(movementFactor, 0, 0);
        }
        
        if (moveVertically)
        {
            movementFactor = Mathf.Sin(Time.time * moveSpeed) * moveMagnitude;
            transform.position = startPosition + new Vector3(0, movementFactor, 0);
        }

        if (enableRotation)
        {
            transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        }
    }
}