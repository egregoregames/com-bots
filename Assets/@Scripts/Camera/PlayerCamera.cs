using ComBots.Logs;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;
using ComBots.Utils.StateMachines;

namespace ComBots.Cameras
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Camera References")]
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private CinemachineCamera _orbitalCCamera;
        [SerializeField] private CinemachineCamera _dialogueCCamera;
        [SerializeField] private CinemachineOrbitalFollow _orbitalFollow;
        [SerializeField] private CinemachineFollow _dialogueFollow;
        [SerializeField] private CinemachineHardLookAt _dialogueLookAt;
        [SerializeField] private Camera _camera;
        public CameraTarget CameraTarget;
        public Camera Camera => _camera;
        public CinemachineCamera ActiveCinemachineCamera { get; private set; }

        [Header("Camera Blending")]
        [SerializeField] private CinemachineBlendDefinition _blendDefinition = new(CinemachineBlendDefinition.Styles.EaseInOut, 1.5f);
        public float BlendTime => _blendDefinition.BlendTime;

        public CameraTarget CurrentSequence { get; private set; }

        public enum CameraState
        {
            NotSet,
            Orbital,
            Dialogue
        }
        public CameraState State { get; private set; } = CameraState.NotSet;

        public void SetState_Dialogue(CameraTarget target)
        {
            if (State == CameraState.Dialogue)
            {
                MyLogger<PlayerCamera>.StaticLog("Already in Dialogue state. Ignoring SetState_Dialogue call.");
            }

            CurrentSequence = target;
            _dialogueCCamera.transform.position = target.PoseT.position;
            _dialogueCCamera.Target.LookAtTarget = target.LookAtT;

            SetState(CameraState.Dialogue);
            SetActiveCCamera(_dialogueCCamera);
        }

        public void SetState_Orbital()
        {
            if (State == CameraState.Orbital)
            {
                MyLogger<PlayerCamera>.StaticLog("Already in Orbital state. Ignoring SetState_Orbital call.");
                return;
            }

            switch (State)
            {
                case CameraState.Dialogue:
                    CurrentSequence = null;
                    break;
            }

            SetState(CameraState.Orbital);
            SetActiveCCamera(_orbitalCCamera);
        }

        private void SetState(CameraState state)
        {
            MyLogger<PlayerCamera>.StaticLog($"Switching camera state to: {state}");
            State = state;
        }

        private void SetActiveCCamera(CinemachineCamera newCamera)
        {
            if (ActiveCinemachineCamera != newCamera)
            {
                // Set the blend definition on the brain
                _cinemachineBrain.DefaultBlend = _blendDefinition;
                
                // Deactivate the current camera
                if (ActiveCinemachineCamera != null)
                {
                    ActiveCinemachineCamera.gameObject.SetActive(false);
                }
                
                // Activate the new camera (this triggers the brain to blend)
                newCamera.gameObject.SetActive(true);
                ActiveCinemachineCamera = newCamera;
                
                MyLogger<PlayerCamera>.StaticLog($"Activated camera: {newCamera.name}");
            }
        }
    }
}