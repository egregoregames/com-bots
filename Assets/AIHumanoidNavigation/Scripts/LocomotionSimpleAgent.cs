using UnityEngine;
using UnityEngine.AI;

namespace AIHumanoidNavigation.Scripts
{
    [RequireComponent (typeof (NavMeshAgent))]
    [RequireComponent (typeof (Animator))]
    public class LocomotionSimpleAgent : MonoBehaviour
    {

        [SerializeField] AgentMoveMode AgentMoveMode;
    
        Animator _anim;
        NavMeshAgent _agent;
        Vector2 _smoothDeltaPosition = Vector2.zero;
        Vector2 _velocity = Vector2.zero;

        void Start ()
        {
            _anim = GetComponent<Animator> ();
            _agent = GetComponent<NavMeshAgent> ();
            // Don’t update position automatically
            _agent.updatePosition = false;
        }
    
        void Update ()
        {
            Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot (transform.right, worldDeltaPosition);
            float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2 (dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
            _smoothDeltaPosition = Vector2.Lerp (_smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                _velocity = _smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = _velocity.magnitude > 0.5f && _agent.remainingDistance > _agent.radius;

            // Update animation parameters
            _anim.SetBool("move", shouldMove);
            _anim.SetFloat ("velx", _velocity.x);
            _anim.SetFloat ("vely", _velocity.y);
        
            LookAt lookAt = GetComponent<LookAt> ();
            if (lookAt)
                lookAt.lookAtTargetPosition = _agent.steeringTarget + transform.forward;

            if (AgentMoveMode == AgentMoveMode.PullAgentTowardsCharacter)
            {
                // Pull agent towards character
                if (worldDeltaPosition.magnitude > _agent.radius)
                    _agent.nextPosition = transform.position + 0.9f*worldDeltaPosition;
            }
            else
            {
                // Pull character towards agent
                if (worldDeltaPosition.magnitude > _agent.radius)
                    transform.position = _agent.nextPosition - 0.9f*worldDeltaPosition;
            }
        
       
        }

        void OnAnimatorMove ()
        {
            // Update position based on animation movement using navigation surface height
            Vector3 position = _anim.rootPosition;
            position.y = _agent.nextPosition.y;
            transform.position = position;
        }
    }

    public enum AgentMoveMode
    {
        PullCharacterTowardsAgent,
        PullAgentTowardsCharacter
    }
}