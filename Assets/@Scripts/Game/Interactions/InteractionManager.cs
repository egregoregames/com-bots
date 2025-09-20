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

        public void StartInteraction(IInteractor interactor, IInteractable interactable)
        {
            Interaction interaction = new Interaction
            {
                interactor = interactor,
                interactable = interactable
            };
            _activeInteractions.Add(interaction);
            interactor.OnInteractionStart(interactable);
            interactable.OnInteractionStart(interactor);
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