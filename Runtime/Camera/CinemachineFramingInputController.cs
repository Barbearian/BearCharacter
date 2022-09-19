using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bear{
    
    [RequireComponent(typeof(CinemachineVirtualCamera),typeof(CinemachineInputProvider))]
    public class CinemachineFramingInputController : MonoBehaviour
    {
        [Min(0)]
        public float maxCamDistance = 10;

        [Min(0)]
        public float RollMultiPlayer = 1;

        [Min(0)]
        public float minCamDistance = 0;
        private CinemachineFramingTransposer cft;
        private CinemachineVirtualCamera cam;
        private CinemachineInputProvider provider;
        public InputActionReference CameraZoom;
        public InputActionReference CameraXY;

        private void Awake() {
            cam = GetComponent<CinemachineVirtualCamera>();
            cft = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
            provider = GetComponent<CinemachineInputProvider>();

            //CameraXY.action.Enable();
            provider.XYAxis = CameraXY;
            CameraZoom.action.Enable();

            cft.m_MinimumDistance = minCamDistance;
            cft.m_MaximumDistance = maxCamDistance;
        }
        

        private void UpdateDistance(InputAction.CallbackContext context){
            var speed = context.ReadValue<float>();
            var target = cft.m_CameraDistance+speed*RollMultiPlayer*Time.deltaTime;
            target = Mathf.Clamp(target,minCamDistance,maxCamDistance);
            cft.m_CameraDistance = target;
            
        }

        private void OnEnable() {
            CameraZoom.action.performed += UpdateDistance;
        }

        private void OnDisable() {
            CameraZoom.action.performed -= UpdateDistance;
        }
    }
}