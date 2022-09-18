
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bear
{
    public static class LocalPlayerControllerFactory
    {
        public static async UniTask<GameObject> MakeLocalPlayer() {
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){

                var nanv = await MakeControllablePlayer();
                if(nanv.TryGetKidNode<InputNodeView>(out var inputgameobj)){
                    
                    //Make Avatar 
                    var avatarPref = await loader.LoadAsync<GameObject>("PlayerAvatar");
                    var avatarAnim = GameObject.Instantiate(avatarPref).GetComponent<AnimatorNodeView>();
                    nanv.Link(avatarAnim);

                    

                    //link input to play animation
                    inputgameobj.Link(nanv,avatarAnim);

                    //Make Camera
                    var camNode = await MakeLocalLookatCamera();
                    
                    
                    camNode.Link(avatarAnim);

                    return nanv.gameObject;   

                    
                }
                
            }
            return null;
            

        }

        public static async UniTask<NavimeshAgentNodeView> MakeControllablePlayer(){
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){
                NavimeshAgentNodeView nanv = await MakePlayerController();

                
                         
                //Make input
                var inputgameobj = new GameObject("InputNodeView").AddComponent<InputNodeView>();
                inputgameobj.Link(nanv);



                return nanv;
            }
            return null;
        }

        public static async UniTask<NavimeshAgentNodeView> MakePlayerController(){
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){
                //Make Player
                var rs = await loader.LoadAsync<GameObject>("Player");
                var obj = GameObject.Instantiate(rs);
                NavimeshAgentNodeView nanv = obj.GetComponent<NavimeshAgentNodeView>();

                //Add state machine
                var naivesm = new NaiveStateMachineNodeData();
                               
                nanv.AddNodeData(naivesm);

                //Trigger when move
                nanv.movementObserver.DOnStartMove += ()=>{naivesm.EnterState("Moving");};

                return nanv;
            }else{
                return null;
            }
        } 

        public static async UniTask<CinemachineVirtualCameraNodeView> MakeLocalLookatCamera(){
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){
                //Make Camera 
                var cam = Camera.main;
                var camview = cam.gameObject.AddNodeView<CameraNodeView>();
                camview.Init();

                //Make Camera look at player
                var cnode = await loader.LoadAsync<GameObject>("CinemachineFreeForm");
                var camNode = GameObject.Instantiate(cnode).GetComponent<CinemachineVirtualCameraNodeView>();
                return camNode;
            }
            return null;
        }

        
    

        private static void Link(this CinemachineVirtualCameraNodeView view, NodeView target){
            var anchor = new GameObject("CameraAnchor").AddNodeView<CameraAnchorNodeView>();
            target.AddNodeViewChild(anchor);
            anchor.transform.localPosition = Vector3.up*1.5f;

            view.dcland.UpdateFollow(anchor.transform);
            view.dcland.UpdateLookAt(anchor.transform);
        }

        private static void Link(this InputNodeView view, NavimeshAgentNodeView nanv){
            view.LinkMovement(nanv);
            view.LinkNavigation(nanv);
            nanv.AddNodeViewChild(view);
            
        }

        private static void LinkMovement(this InputNodeView view, IMovementInputReceiver target){
                        
            //Associate input->move input
            MovementInputNode min = new MovementInputNode();
            view.inputtarget.Link<Vector2>(min.Move);

            //Associate move input -> move
            min.forward = target.DOnReceiveMovementInput;     

            
        }

        private static void LinkNavigation(this InputNodeView view, IReceiveNavigationScan target){
            //Associate with click one target -> move to 
            view.buttonInputData.Register("Player/ClickOnTarget",(x)=>{
                //  Debug.Log("I have clicked");
                target.DOnReceive();
            });
        }

        public static void AddNodeViewChild(this NodeView parent, NodeView kid){
            parent.AddChildrenNode(kid);
            parent.transform.AddChildrenAtZero(kid.transform);
        }

        

        private static void Link(this NavimeshAgentNodeView nanv,AnimatorNodeView anim){
            nanv.transform.AddChildrenAtZero(anim.transform);

            nanv.movementObserver.DOnMove+=(speed)=>{
                float multi = 1;
                if(speed>=6){
                    speed = 6;
                    multi = speed/6;
                }
                anim.anim.SetFloat("Speed",speed);
                anim.anim.SetFloat("MotionSpeed",multi);
            };


            //Add state to statemachine
            if(nanv.TryGetNodeData<NaiveStateMachineNodeData>(out var naiveStateMachineNodeData)){
                //When Play Animation stop moving
                var state = naiveStateMachineNodeData.GetOrCreateNaiveState("PlayStandingGesture");
                state.DOnEnterState+=()=>{
                    nanv.Stop();
                    Debug.Log("I tried to let player stop");
                };

                //When Start Moving Stop Animation
                state = naiveStateMachineNodeData.GetOrCreateNaiveState("Moving");
                state.DOnEnterState+=()=>{
                    anim.StopPlayGesture();
                };
            }
        
        }

        public static void AddChildrenAtZero(this Transform parent, Transform kid){
            kid.parent = parent;

            //kid.SetParent(parent);
            kid.localPosition = Vector3.zero;
            kid.localRotation = Quaternion.identity;
        }

        public static void Link(this InputNodeView view, INode unit,AnimatorNodeView anim){
            if(unit.TryGetNodeData<NaiveStateMachineNodeData>(out var sm)){
                for(int i = 1;i<=2;i++){
                    var key = "UI/UIShortCut"+i;
                    var num = i-1;
                    view.buttonInputData.Register(key,(x)=>{
                        anim.Play(num);
                        sm.EnterState("PlayStandingGesture");
                    });
                }   
                
            }
           
           
        }

        public static void StopPlayGesture(this AnimatorNodeView anim){
            anim.EnterDefaultState();
        }
    }

    
}