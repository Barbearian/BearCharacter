using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bear
{
    public class InputHelper
    {
        private static PlayerInput pInput;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Debug.Log("I Initialized player input settings");
            pInput = new PlayerInput();
            pInput.Player.Enable();
            pInput.ShortCut.Enable();
            pInput.UI.Enable();
        }

        public static Vector2 GetMoveDir()
        {
            return pInput.Player.MoveDir.ReadValue<Vector2>();
        }

        public static InputAction GetAction(string code){
            return pInput.FindAction(code);
        }
    }
}