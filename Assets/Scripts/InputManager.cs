using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    public float mouseSensitivity = 1.5f;

    public event Action<Vector2> OnWASD;
    public event Action<Vector2> OnArrowKeys;
    public event Action<Vector2> OnMouse;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleWASDInput();
        HandleArrowKeysInput();
        HandleMouseInput();
    }

    private void HandleWASDInput()
    {
        float moveX = Input.GetAxis("HorizontalWASD");
        float moveY = Input.GetAxis("VerticalWASD");
        
        var input = new Vector2(moveX, moveY);
        
        OnWASD?.Invoke(input);
    }
    
    private void HandleArrowKeysInput()
    {
        float moveX = Input.GetAxis("HorizontalArrow");
        float moveY = Input.GetAxis("VerticalArrow");
        
        var input = new Vector2(moveX, moveY);
        
        OnArrowKeys?.Invoke(input);
    }
    
    private void HandleMouseInput()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        var input = new Vector2(moveX, moveY);
        
        OnMouse?.Invoke(input);
    }
}