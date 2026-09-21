using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFacing : MonoBehaviour
{
    public InputActionReference moveAction;

    public Vector2 FacingDirection { get; private set; } = Vector2.down;

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Only change facing when actually pressing a direction
        if (input == Vector2.zero)
            return;

        // Horizontal has priority
        if (input.x > 0)
            FacingDirection = Vector2.right;
        else if (input.x < 0)
            FacingDirection = Vector2.left;
        else if (input.y > 0)
            FacingDirection = Vector2.up;
        else if (input.y < 0)
            FacingDirection = Vector2.down;
    }
}