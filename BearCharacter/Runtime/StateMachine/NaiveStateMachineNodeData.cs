using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
    public class NaiveStateMachineNodeData : INodeData
    {
        public string CurrentStateName = "";
        public Dictionary<string,NaiveState> states = new Dictionary<string, NaiveState>();
    }

    public static class NaiveStateMachineNodeDataSystem{
        public static void EnterState(this NaiveStateMachineNodeData statemachine, string stateName){
            if(statemachine.CurrentStateName != stateName){
                if(statemachine.states.TryGetValue(stateName,out var newNaiveState)){
                    if(statemachine.states.TryGetValue(statemachine.CurrentStateName,out var naiveState)){
                        naiveState.DOnExitState?.Invoke();
                    }
                    statemachine.CurrentStateName = stateName;
                    newNaiveState.DOnEnterState?.Invoke();
                    Debug.Log("Entered "+stateName);

                }
            }
        }

        public static void RegisterState(this NaiveStateMachineNodeData statemachine, string stateName, NaiveState state){
            
            statemachine.states[stateName] = state;
            
        }

        public static bool TryGetNaiveState(this NaiveStateMachineNodeData statemachine, string stateName, out NaiveState state){
            return statemachine.states.TryGetValue(stateName,out state);
        }

        public static NaiveState GetOrCreateNaiveState(this NaiveStateMachineNodeData statemachine, string stateName){
            if(statemachine.TryGetNaiveState(stateName, out var rs)){
                return rs;
            }else{
                NaiveState ns = new NaiveState();
                statemachine.RegisterState(stateName,ns);
                return ns;
            }

        }

        public static void DeregisterState(this NaiveStateMachineNodeData statemachine, string stateName){
            statemachine.states.Remove(stateName);
        }

        
    }

    public class NaiveState{
        public System.Action DOnEnterState;
        public System.Action DOnExitState;
    }
}
