using ComBots.Game.Interactions;
using ComBots.Game.Players;
using ComBots.Game.StateMachine;
using ComBots.UI.OverheadWidgets;
using ComBots.Utils.ObjectPooling;
using R3;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// Interactable instance that invokes events related to sign interaction. 
/// On interaction, a sign UI widget will appear on screen. S
/// ee <see cref="SignUI"/>
/// </summary>
public class Sign : MonoBehaviourR3, IInteractable
{
    private static UnityEventR3<string> _onSignActivated = new();
    public static IDisposable OnSignActivated(Action<string> x) => _onSignActivated.Subscribe(x);

    public Transform T => transform;
    public bool IsActive => true; // ????

    [Header("Sign Text")]
    [TextArea(3, 10)]
    [SerializeField] private string _signText;

    [Header("Interact Widget")]
    [SerializeField] private Vector3 _interactWidgetOffset;
    private const string PK_INTERACT_WIDGET = "Sign_Interact_Widget";
    private OverheadWidget _interactWidget;

    private IInteractor _currentInteractor;

    private InputSystem_Actions Inputs { get; set; }

    #region Monobehaviour

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + _interactWidgetOffset, 0.1f);
    }

    protected override void Initialize()
    {
        base.Initialize();

        Inputs = new();

        //var onInteract = Observable.FromEvent<InputAction.CallbackContext>(
        //    h => Inputs.Player.Interact.performed += h,
        //    h => Inputs.Player.Interact.performed -= h);

        AddEvents(
            SignUI.OnClosed(StateSign_OnExit)
        );
    }

    private new void OnEnable()
    {
        base.OnEnable();
        Inputs.Enable();
    }

    private void OnDisable()
    {
        Inputs.Disable();
    }

    #endregion

    #region IInteractable Interface

    public bool CanInteract(IInteractor interactor)
    {
        return interactor is Player && !SignUI.IsOpen;
    }

    public void OnInteractionStart(IInteractor interactor)
    {
        if (_interactWidget)
        {
            PoolManager.I.Push(PK_INTERACT_WIDGET, _interactWidget);
            _interactWidget = null;
        }
        _currentInteractor = interactor;
        //State_Sign_Args stateArgs = new (_signText, StateSign_OnExit);
        //GameStateMachine.I.SetState<GameStateMachine.State_Sign>(stateArgs);
        _onSignActivated?.Invoke(_signText);
    }

    public void OnInteractorFar(IInteractor interactor)
    {
        if (_interactWidget)
        {
            PoolManager.I.Push(PK_INTERACT_WIDGET, _interactWidget);
            _interactWidget = null;
        }
    }

    public void OnInteractorNearby(IInteractor interactor)
    {
        if (!IsActive)
        {
            return;
        }

        if (!_interactWidget)
        {
            _interactWidget = PoolManager.I.Pull<OverheadWidget>(PK_INTERACT_WIDGET);
            _interactWidget.transform.position = transform.position + _interactWidgetOffset;
        }
    }

    public void OnInteractionEnd(IInteractor interactor)
    {
    }

    #endregion

    #region GameStateMachine API
        
    private void StateSign_OnExit()
    {
        InteractionManager.I.EndInteraction(_currentInteractor, this);
    }
        
    #endregion
}