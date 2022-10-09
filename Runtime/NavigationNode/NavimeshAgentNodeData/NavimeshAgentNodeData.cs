
using UnityEngine.AI;

namespace Bear
{
    public class NavMeshAgentNodeData : INodeData, IOnAttachedToNode,IOnDetachedFromNode, INavMeshAgentController
    {
        public NavMeshAgent agent;

        public NavigatorInputNodeData PointInputNode { get; private set; } = new NavigatorInputNodeData();

        public DirectionalMovementInputNodeData DirectionalMovementInputNode { get; private set; } = new DirectionalMovementInputNodeData();

        public MovementNodeData MovementData { get; private set; } = new MovementNodeData();

        public MovementObserverNodeData MovementObserver { get; private set; } = new MovementObserverNodeData();

        public NavMeshAgent Agent { get; private set; }

        public void Attached(INode node)
        {
            if (node is IOnUpdateUpdater uNode && node is NodeView view)
            {
                Agent = view.GetComponent<NavMeshAgent>();

                uNode.DOnUpdate.Subscribe(MyUpdate);

                Init(node);
            }
        }


        public void Detached(INode node)
        {
            if (node is IOnFixedUpdateUpdater uNode)
            {
                Agent = null;

                uNode.DOnFixedUpdate.Unsubscribe(MyUpdate);

            }
        }

        public void MyUpdate() {
            this.SnapTurn();
            this.CheckSpeed();
            this.NotifySpeed();

            this.Move(DirectionalMovementInputNode.MoveDir);
            this.Rotate(DirectionalMovementInputNode.RotateDir);
        }

        public void Init(INode root) {
            PointInputNode = root.GetOrCreateNodeData<NavigatorInputNodeData>(PointInputNode);
            DirectionalMovementInputNode = root.GetOrCreateNodeData<DirectionalMovementInputNodeData>(DirectionalMovementInputNode);

            MovementData = root.GetOrCreateNodeData<MovementNodeData>(MovementData);
            MovementObserver = root.GetOrCreateNodeData<MovementObserverNodeData>(MovementObserver);

            DirectionalMovementInputNode.DMove += (dir) => { DirectionalMovementInputNode.RotateDir = dir; };
            DirectionalMovementInputNode.DRotate += (dir) => { DirectionalMovementInputNode.MoveDir = dir; };
            PointInputNode.DMoveTo += this.MoveTo;

        }
    }
}