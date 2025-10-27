using System.Collections.Generic;
using ComBots.Utils.EntryPoints;

namespace ComBots.Game.Interactions
{
    public class InteractionManager : EntryPointMono
    {
        public override Dependency Dependency => Dependency.Independent;

        private struct Interaction
        {
            public IInteractor interactor;
            public IInteractable interactable;
        }

        public static InteractionManager I { get; private set; }

        private List<Interaction> _activeInteractions;

        protected override void Init()
        {
            _activeInteractions = new();
            I = this;
        }

        public override void Dispose()
        {
            _activeInteractions = null;
        }

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        /// <summary>
        /// Checks if an interaction can occure before notifying the interactable.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="interactable"></param>
        public void OnInteractorNearby(IInteractor interactor, IInteractable interactable)
        {
            if (!interactable.IsActive)
            {
                return;
            }
            if (!interactable.CanInteract(interactor))
            {
                return;
            }
            interactable.OnInteractorNearby(interactor);
        }

        /// <summary>
        /// Notifies the interactable that the interactor is no longer nearby.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="interactable"></param>
        public void OnInteractorFar(IInteractor interactor, IInteractable interactable)
        {
            interactable.OnInteractorFar(interactor);
        }

        /// <summary>
        /// Starts an interaction between an interactor and an interactable when possible.
        /// </summary>
        /// <returns> Whether the interaction was started successfully. </returns>
        public bool StartInteraction(IInteractor interactor, IInteractable interactable)
        {
            if (!interactable.IsActive)
            {
                return false;
            }
            if (!interactable.CanInteract(interactor))
            {
                return false;
            }

            Interaction interaction = new()
            {
                interactor = interactor,
                interactable = interactable
            };
            _activeInteractions.Add(interaction);
            interactor.OnInteractionStart(interactable);
            interactable.OnInteractionStart(interactor);
            return true;
        }

        /// <summary>
        /// Ends an interaction between an interactor and an interactable.
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="interactable"></param>
        public void EndInteraction(IInteractor interactor, IInteractable interactable)
        {
            for (int i = 0; i < _activeInteractions.Count; i++)
            {
                if (_activeInteractions[i].interactor == interactor && _activeInteractions[i].interactable == interactable)
                {
                    _activeInteractions.RemoveAt(i);
                    break;
                }
            }
            interactor.OnInteractionEnd(interactable);
            interactable.OnInteractionEnd(interactor);
        }

        #endregion
    }
}