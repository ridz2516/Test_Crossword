using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public static Action OnInputUp      = delegate { };
    public static Action OnInputDown    = delegate { };

    private bool m_IsInputDown;
    public bool IsInputDown => m_IsInputDown;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputDown();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            inputUp();
        }
    }

    private void inputDown()
    {
        m_IsInputDown = true;
        OnInputDown?.Invoke();
    }

    private void inputUp()
    {
        m_IsInputDown = false;
        OnInputUp?.Invoke();
    }
}
