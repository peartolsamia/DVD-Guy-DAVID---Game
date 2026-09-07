using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static InputReader Instance { get; private set; }

    public event Action<Vector2> OnDoorDirectionPressed;

    private Controls controls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
    }

    private void OnEnable() => controls?.Player.Enable();
    private void OnDisable() => controls?.Player.Disable();

    public void OnNavigateDoor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            OnDoorDirectionPressed?.Invoke(direction);
        }
    }
}