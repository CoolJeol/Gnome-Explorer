using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBlock : MonoBehaviour
{
    public InputActionReference blockAction;

    public bool IsBlocking { get; private set; }

    void OnEnable()
    {
        blockAction.action.Enable();
    }

    void OnDisable()
    {
        blockAction.action.Disable();
    }

    void Update()
    {
        IsBlocking = blockAction.action.IsPressed();
    }
}