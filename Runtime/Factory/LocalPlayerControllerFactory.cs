
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bear
{
    public static class LocalPlayerControllerFactory
    {
        public static async UniTask<GameObject> Make(string key){
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){

                var pref = await loader.LoadAsync<GameObject>(key);
                return GameObject.Instantiate(pref);
                
                
            }
            return null;
        }

        public static async UniTask<NodeView> MakeLocalPlayer(GameObject avatarPref = null) {
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){

                var nanv = await MakeControllablePlayer();
                if(nanv.TryGetKidNode<InputNodeView>(out var inputgameobj)){
                    
                    //Make Avatar 
                    var avatarAnim = await MakeAnimatorNode(6,"Speed","MotionSpeed",avatarPref);
                    nanv.Link(avatarAnim);

                    //link input to play animation
                    inputgameobj.Link(avatarAnim);

                    return nanv;   
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

                //stop moving when play guesture
                var state = naivesm.GetOrCreateNaiveState("PlayStandingGesture");
                state.DOnEnterState+=()=>{
                    nanv.Stop();
                    Debug.Log("I tried to let player stop");
                };


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

        
        public static async UniTask<AnimatorNodeView> MakeAnimatorNode(int maxSpeedBlend,string SpeedAttribute,string SpeedMultiAttribute,GameObject avatarPref = null){
            if(INodeSystem.GlobalNode.TryGetNodeData<ResourceLoaderNodeData>(out var loader)){
                AnimatorNodeView avatarAnim;
                if(avatarPref == null){
                    avatarPref = await loader.LoadAsync<GameObject>("PlayerAvatar");
                    avatarAnim = GameObject.Instantiate(avatarPref).GetComponent<AnimatorNodeView>();
                }else{
                    avatarAnim = avatarPref.GetComponent<AnimatorNodeView>();
                }
                 
                

                avatarAnim.AddNodeData(new AnimatorMovementSpeedInputStreamReceiverNodeData(){
                    maxSpeedBlend = maxSpeedBlend,
                    SpeedAttribute = SpeedAttribute,
                    SpeedMultiAttribute = SpeedMultiAttribute
                });

                
                return avatarAnim;
            }

            return null;

        }

        public static void Link(this CinemachineVirtualCameraNodeView view, NodeView target){
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


        private static void Link(this NavimeshAgentNodeView nanv,IAnimatorNode anim){
           // nanv.transform.AddChildrenAtZero(anim.transform);
            
            if(anim is INode node && node.TryGetNodeData<AnimatorMovementSpeedInputStreamReceiverNodeData>(out var receiver)){
                nanv.AddNodeOrNodeViewChild(anim);
                nanv.movementObserver.DOnMove += (speed)=>{receiver.UpdateSpeedAndMulti(speed);};
            }

            //Add state to statemachine
            if(nanv.TryGetNodeData<NaiveStateMachineNodeData>(out var naiveStateMachineNodeData)){
                var state = naiveStateMachineNodeData.GetOrCreateNaiveState("Moving");
                state.DOnEnterState+=()=>{
                    anim.EnterDefaultState();
                };
            }
        
        }



        public static void Link(this InputNodeView view,IAnimatorClipsPlayer anim){
            if(view.TryGetParentNode(out var unit)){
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

           
           
        }

        public static void StopPlayGesture(this AnimatorNodeView anim){
            anim.EnterDefaultState();
        }
    }

    
}