using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class CardInput : MonoBehaviour
{
    public static CardInput Instance { get; private set; }

    public Vector2 mousePosition { get; private set; }
    public bool onClick { get; private set; }
    public bool onRealize { get; private set; }
    public bool onHeld { get; private set; }
    private bool wasHeld;
    private GameInput input;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("CardInput instance already exist → " + gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        input = new GameInput();
    }

    private void OnEnable()
    {
        input.Card.Enable();
    }

    private void OnDisable()
    {
        input.Card.Disable();
    }

    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        mousePosition = input.Card.Mouse.ReadValue<Vector2>();
        onHeld = input.Card.Click.ReadValue<float>() >= 0.1f ? true : false;
        onClick = (onHeld && !wasHeld) ? true : false;
        onRealize = (!onHeld && wasHeld) ? true : false;
        wasHeld = onHeld;
    }
}
