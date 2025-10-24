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
    }
}