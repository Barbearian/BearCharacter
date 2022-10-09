using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Bear
{
    //[RequireComponent(typeof(NavMeshAgent))]
    public class NavimeshAgentNodeView : NodeView,IMovementInputReceiver,IReceiveNavigationScan, INavMeshAgentController
    {
        public NavigatorInputNodeData pointInputNode;   
        public DirectionalMovementInputNodeData directionalInputNode;

        public MovementNodeData movementData;
        public MovementObserverNodeData movementObserver;
    
        public NavMeshAgent agent;
        public Action<Vector3> DOnReceiveMovementInput => this.MoveAndRotate;
        public Action DOnReceive => this.MoveToMouseClick;



        public NavigatorInputNodeData PointInputNode => pointInputNode;
        public DirectionalMovementInputNodeData DirectionalMovementInputNode => directionalInputNode;

        public MovementNodeData MovementData => movementData;
        public MovementObserverNodeData MovementObserver => movementObserver;
        public NavMeshAgent Agent => agent;

        private void Awake()
        {
            
            Init(GetComponent<NavMeshAgent>());
            
        }

        public void Init(NavMeshAgent agent){
            this.agent = agent;

            directionalInputNode.DMove += (dir)=>{directionalInputNode.RotateDir = dir;};
            directionalInputNode.DRotate += (dir)=>{directionalInputNode.MoveDir = dir;};
            pointInputNode.DMoveTo += this.MoveTo;
        }

        private void FixedUpdate()
        { 
            this.SnapTurn();
            this.CheckSpeed();
            this.NotifySpeed();

            this.Move(directionalInputNode.MoveDir);
            this.Rotate(directionalInputNode.RotateDir);
        }


    }


}