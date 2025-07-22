using UnityEngine;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour
{
    [SerializeField] private float _upForce = 250f;
    [SerializeField] private float _speed = 10f;

    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private Vector2 _movement;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement = _playerInput.actions["Move"].ReadValue<Vector2>();
        Debug.Log($"El Player se mueve hacia {_movement}");
    }

    private void FixedUpdate()
    {
        _rb.AddForce(new Vector3(_movement.x, 0f, _movement.y) * _speed);
    }

    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            _rb.AddForce(Vector3.up * _upForce);
            Debug.Log("Jump");
            Debug.Log(callbackContext.phase);
        }
    }
}
