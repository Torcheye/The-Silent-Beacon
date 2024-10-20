using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    public float mouseSensitivity = 1.5f;

    public event Action<Vector2> OnWASD;
    public event Action<Vector2> OnMouseMove;
    public event Action OnMouseClick;
    
    private bool _enableMovementInput = true;
    private bool _enableMouseMoveInput = true;
    private bool _enableMouseClickInput = true;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleWASDInput();
        HandleMouseMoveInput();
        HandleMouseClickInput();
    }
    
    public void ToggleMovementInput(bool toggle)
    {
        _enableMovementInput = toggle;
    }
    
    public void ToggleMouseMoveInput(bool toggle)
    {
        _enableMouseMoveInput = toggle;
    }
    
    public void ToggleMouseClickInput(bool toggle)
    {
        _enableMouseClickInput = toggle;
    }

    private void HandleWASDInput()
    {
        if (!_enableMovementInput) return;
        
        float moveX = Input.GetAxis("HorizontalWASD");
        float moveY = Input.GetAxis("VerticalWASD");
        
        var input = new Vector2(moveX, moveY);
        
        OnWASD?.Invoke(input);
    }
    
    private void HandleMouseMoveInput()
    {
        if (!_enableMouseMoveInput) return;
        
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        var input = new Vector2(moveX, moveY);
        
        OnMouseMove?.Invoke(input);
    }
    
    private void HandleMouseClickInput()
    {
        if (!_enableMouseClickInput) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick?.Invoke();
        }
    }
}