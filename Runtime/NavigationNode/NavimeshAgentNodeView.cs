using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Bear
{
    //[RequireComponent(typeof(NavMeshAgent))]
    public class NavimeshAgentNodeView : NodeView,IMovementInputReceiver,IReceiveNavigationScan
    {
        public NavigatorInputNodeData pointInputNode;
        public DirectionalMovementInputNodeData directionalInputNode;

        public MovementNodeData movementData;
        public MovementObserverNodeData movementObserver;
        
        public NavMeshAgent agent;

        public Action<Vector3> DOnReceiveMovementInput => this.MoveAndRotate;

        public Action DOnReceive => this.MoveToMouseClick;

        private void Awake()
        {
            
            Init(GetComponent<NavMeshAgent>());
            
        }

        public void Init(NavMeshAgent agent){
            this.agent = agent;
            directionalInputNode.DMove += this.Move;
            directionalInputNode.DRotate += this.Rotate;

            pointInputNode.DMoveTo += this.MoveTo;
        }

        private void FixedUpdate()
        { 
            this.SnapTurn();
            this.CheckSpeed();
            this.NotifySpeed();
        }

    }

    public static class NavimeshAgentNodeViewSystem{
        internal static void SnapTurn(this NavimeshAgentNodeView view){

            var agent = view.agent;
            if (agent.velocity.sqrMagnitude > 1f)
            {
                Quaternion dir = Quaternion.LookRotation(agent.velocity.normalized);
                dir.x = 0;
                dir.z = 0;
                agent.transform.rotation = dir;
            }
        }

        internal static void CheckSpeed(this NavimeshAgentNodeView view){
            
            //var movementData = view.movementData;
            var mond = view.movementObserver;
            
            var agent = view.agent;
            
            var dir = view.movementData.dir;
            if (dir.sqrMagnitude > 0f || agent.velocity.sqrMagnitude>0)
            {
                if (!view.movementData.isMoving)
                {
                    mond.DOnStartMove?.Invoke();
                }

                view.movementData.isMoving = true;
            }
            else
            {
                if (view.movementData.isMoving)
                {
                    mond.DOnStop?.Invoke();
                }
                view.movementData.isMoving = false;
            }
        }

        public static void Stop(this NavimeshAgentNodeView view){
            var agent = view.agent;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            view.movementData.dir = Vector3.zero;
        }

        public static void MoveAndRotate(this NavimeshAgentNodeView view,Vector3 dir){
            view.Rotate(dir);
            view.Move(dir);
        }   

        public static void Move(this NavimeshAgentNodeView view,Vector3 dir)
        {
            bool hadInput = view.movementData.dir.sqrMagnitude !=0;
            bool hasInput = dir.sqrMagnitude>0;
            var agent = view.agent;

            if(!agent.isStopped && dir.sqrMagnitude>0){
                view.Stop();
            }

            Vector3 move = dir * view.movementData.speedMulti;

            if((!hadInput) && hasInput){
                view.movementObserver.DOnStartMove.Invoke();
            }

            view.movementData.dir = move;
            if(!hasInput){
                view.movementData.dir = agent.velocity;
            }

            
            agent.Move(move * Time.deltaTime);
        }

        public static void Rotate(this NavimeshAgentNodeView view, Vector3 faceDir){
            if(faceDir.sqrMagnitude>0){
                view.transform.forward = faceDir;
            }
            
        }

        public static void MoveTo(this NavimeshAgentNodeView view,Vector3 des)
        {   

            var agent = view.agent;
            agent.isStopped = false;
            agent.SetDestination(des);
        }

        public static void NotifySpeed(this NavimeshAgentNodeView view){
            float speed = view.movementData.dir.magnitude;
            view.movementObserver.DOnMove?.Invoke(speed);
        }

        public static void MoveToMouseClick(this NavimeshAgentNodeView view){
            var mask = LayerMask.GetMask("Ground");
            var location = MouseRaycastHelper.RayCastToGround(1000f,mask,out var rs);
            if(location){
                view.pointInputNode.DMoveTo?.Invoke(rs.point);
            }
        }
    }
}