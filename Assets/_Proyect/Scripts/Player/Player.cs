using UnityEngine;
using UnityEngine.InputSystem; 

public class Player : MonoBehaviour
{
    [SerializeField] private float _upForce = 250f;
    private Rigidbody _rb;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
