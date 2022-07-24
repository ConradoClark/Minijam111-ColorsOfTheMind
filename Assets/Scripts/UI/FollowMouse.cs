using System.Collections;
using System.Collections.Generic;
using Licht.Unity.Objects;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    private InputAction _mousePos;
    private void Awake()
    {
        var mousePosInput = SceneObject<DefaultMouseInputs>.Instance().MousePosInput;
        var playerInput = PlayerInput.GetPlayerByIndex(0);
        _mousePos = playerInput.actions[mousePosInput.ActionName];
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        var mousePos = _mousePos.ReadValue<Vector2>();
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);
    }
}
