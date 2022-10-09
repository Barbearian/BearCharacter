using UnityEngine;
using UnityEngine.AI;

namespace Bear
{
    public interface INavMeshAgentController
    {
        public NavigatorInputNodeData PointInputNode { get; }
        public DirectionalMovementInputNodeData DirectionalMovementInputNode { get; }

        public MovementNodeData MovementData { get; }
        public MovementObserverNodeData MovementObserver { get; }

        public NavMeshAgent Agent { get; }
    }

    public static class INavMeshAgentControllerSystem {
        public static void SnapTurn(this INavMeshAgentController view)
        {

            var agent = view.Agent;
            if (agent.velocity.sqrMagnitude > 1f)
            {
                Quaternion dir = Quaternion.LookRotation(agent.velocity.normalized);
                dir.x = 0;
                dir.z = 0;
                agent.transform.rotation = dir;
            }
        }

        public static void CheckSpeed(this INavMeshAgentController view)
        {

            //var movementData = view.movementData;
            var mond = view.MovementObserver;

            var agent = view.Agent;

            var dir = view.MovementData.dir;
            if (dir.sqrMagnitude > 0f || agent.velocity.sqrMagnitude > 0)
            {
                if (!view.MovementData.isMoving)
                {
                    mond.DOnStartMove?.Invoke();
                }

                view.MovementData.isMoving = true;
            }
            else
            {
                if (view.MovementData.isMoving)
                {
                    mond.DOnStop?.Invoke();
                }
                view.MovementData.isMoving = false;
            }
        }

        public static void Stop(this INavMeshAgentController view)
        {
            var agent = view.Agent;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            view.MovementData.dir = Vector3.zero;
        }

        public static void MoveAndRotate(this INavMeshAgentController view, Vector3 dir)
        {
            view.Rotate(dir);
            view.Move(dir);
        }

        public static void Move(this INavMeshAgentController view, Vector3 dir)
        {
            bool hadInput = view.MovementData.dir.sqrMagnitude != 0;
            bool hasInput = dir.sqrMagnitude > 0;
            var agent = view.Agent;

            if (!agent.isStopped && dir.sqrMagnitude > 0)
            {
                view.Stop();
            }

            Vector3 move = dir * view.MovementData.speedMulti;

            if ((!hadInput) && hasInput)
            {
                view.MovementObserver.DOnStartMove.Invoke();
            }

            view.MovementData.dir = move;
            if (!hasInput)
            {
                view.MovementData.dir = agent.velocity;
            }

            if (hasInput)
            {
                var velocity = move * Time.deltaTime;
                agent.velocity = velocity;
                agent.Move(velocity);

            }

        }

        public static void Rotate(this INavMeshAgentController view, Vector3 faceDir)
        {
            if (faceDir.sqrMagnitude > 0)
            {
                view.Agent.transform.forward = faceDir;
            }

        }

        public static void MoveTo(this INavMeshAgentController view, Vector3 des)
        {

            var agent = view.Agent;
            agent.isStopped = false;
            agent.SetDestination(des);
        }

        public static void NotifySpeed(this INavMeshAgentController view)
        {
            float speed = view.MovementData.dir.magnitude;
            view.MovementObserver.DOnMove?.Invoke(speed);
        }

        public static void MoveToMouseClick(this INavMeshAgentController view)
        {
            var mask = LayerMask.GetMask("Ground");
            var location = MouseRaycastHelper.RayCastToGround(1000f, mask, out var rs);
            if (location)
            {
                view.PointInputNode.DMoveTo?.Invoke(rs.point);
            }
        }
    }
}