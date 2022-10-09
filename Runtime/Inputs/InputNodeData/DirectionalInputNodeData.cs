using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInput;

namespace Bear
{
    public class DirectionalInputNodeData : INodeData,IPlayerActions,IOnAttachedToNode,IOnDetachedFromNode
    {
        public Vector2 dir;
        public SafeDelegate<Vector2> inputtarget = new SafeDelegate<Vector2>();

        public void Attached(INode node)
        {
            
            if (node is IOnUpdateUpdater uNode) {
                InputHelper.pInput.Player.SetCallbacks(this);

                uNode.DOnUpdate.Subscribe(Move);
            }
        }

        public void Detached(INode node)
        {
            if (node is IOnUpdateUpdater uNode)
            {
                uNode.DOnUpdate.Unsubscribe(Move);
            }
        }

        public void OnClickOnTarget(UnityEngine.InputSystem.InputAction.CallbackContext context){}

        public void OnMoveDir(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                dir = context.ReadValue<Vector2>();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                dir = Vector2.zero;
            }
        }

        private void Move() {
            inputtarget.invoker.Invoke(dir);
        }
    }
}