using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Bear
{
    public class InputNodeView : NodeView
    {
        public SafeDelegate<Vector2> inputtarget = new SafeDelegate<Vector2>(); 
        public InputAssociateNodeData buttonInputData = new InputAssociateNodeData();
        public void Update()
        {
            
            inputtarget.invoker?.Invoke(InputHelper.GetMoveDir());
        }

        private void OnDestroy() {
            buttonInputData.Dispose();
        }
    }

    public class InputAssociateNodeData:INodeData{
        public Dictionary<string,List<System.Action<InputAction.CallbackContext>>> actions = new Dictionary<string, List<System.Action<InputAction.CallbackContext>>>();
    }

    public static class InputAssociateNodeDataSystem{
        public static void Register(this InputAssociateNodeData nodedata, string code, System.Action<InputAction.CallbackContext> DOnPerformed){
            var inputaction = InputHelper.GetAction(code);
            if(inputaction != null){
                inputaction.performed += DOnPerformed;
                nodedata.actions.Enqueue<string,System.Action<InputAction.CallbackContext>>(code,DOnPerformed);    
            }
        }

        public static void Deregister(this InputAssociateNodeData nodedata, string code, System.Action<InputAction.CallbackContext> DOnPerformed){
            var inputaction = InputHelper.GetAction(code);
            if(inputaction != null){
                inputaction.performed -= DOnPerformed;
                nodedata.actions.Dequeue<string,System.Action<InputAction.CallbackContext>>(code,DOnPerformed);    
            }
        }

        public static void Dispose(this InputAssociateNodeData nodedata){
            var dic = nodedata.actions;
            nodedata.actions = new Dictionary<string, List<System.Action<InputAction.CallbackContext>>>();
            foreach(var kv in dic){
                foreach(var action in kv.Value)
                    nodedata.Deregister(kv.Key,action);
            }
        }
    }


}