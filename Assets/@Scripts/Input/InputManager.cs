using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using ComBots.Logs;
using ComBots.Utils.EntryPoints;

namespace ComBots.Inputs
{
    public class InputManager : EntryPointMono
    {
        public static InputManager I { get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        private readonly Stack<InputContext> _contextStack = new();
        private readonly Dictionary<string, InputContext> _namedContexts = new();
        
        // Track all active input actions globally
        private readonly Dictionary<InputAction, InputContext> _activeActions = new();

        [Header("Debug")]
        [SerializeField] private MyLogger<InputManager> _logger;

        public event System.Action<InputContext> OnContextChanged;

        protected override void Init()
        {
            I = this;
            _logger.TryInit();
            _logger.Log("Initialized.");
        }

        public override void Dispose()
        {
            // Clean up all active actions
            foreach (var kvp in _activeActions)
            {
                UnsubscribeFromAction(kvp.Key);
            }
            _activeActions.Clear();
            
            I = null;
        }

        public InputContext PushContext(InputContextConfig contextData, IInputHandler handler)
        {
            var context = new InputContext(contextData, handler);

            // Store named contexts
            if (!string.IsNullOrEmpty(context.Name))
            {
                _namedContexts[context.Name] = context;
            }

            // Pause current top context
            if (_contextStack.Count > 0)
            {
                var currentTop = _contextStack.Peek();
                currentTop.Handler.OnInputContextPaused(currentTop);
                currentTop.DisableActions();
                UnsubscribeFromContext(currentTop);
            }

            _contextStack.Push(context);
            SubscribeToContext(context);
            context.EnableActions();
            context.Handler.OnInputContextEntered(context);

            OnContextChanged?.Invoke(context);

            _logger.Log($"Pushed input context: {context.Name}");

            return context;
        }

        public bool PopContext(string contextName = null)
        {
            if (_contextStack.Count == 0)
            {
                _logger.Log("No context to pop!");
                return false;
            }

            InputContext contextToRemove = null;

            if (!string.IsNullOrEmpty(contextName))
            {
                // Pop specific named context
                contextToRemove = PopNamedContext(contextName);
            }
            else
            {
                // Pop top non-persistent context
                while (_contextStack.Count > 0)
                {
                    var context = _contextStack.Peek();
                    if (!context.Persistent)
                    {
                        contextToRemove = _contextStack.Pop();
                        break;
                    }
                    _logger.Log($"Could not pop persistent context<{context.Name}>");
                    return false; // Can't pop persistent context this way
                }
            }

            if (contextToRemove != null)
            {
                contextToRemove.DisableActions();
                UnsubscribeFromContext(contextToRemove);
                contextToRemove.Handler.OnInputContextExited(contextToRemove);
                _namedContexts.Remove(contextToRemove.Name);

                // Resume previous context
                if (_contextStack.Count > 0)
                {
                    var newTop = _contextStack.Peek();
                    SubscribeToContext(newTop);
                    newTop.EnableActions();
                    newTop.Handler.OnInputContextResumed(newTop);
                }

                OnContextChanged?.Invoke(GetActiveContext());

                _logger.Log($"Popped input context: {contextToRemove.Name}");
                return true;
            }

            return false;
        }

        private InputContext PopNamedContext(string contextName)
        {
            if (!_namedContexts.ContainsKey(contextName)) return null;

            var tempStack = new Stack<InputContext>();
            InputContext found = null;

            while (_contextStack.Count > 0)
            {
                var context = _contextStack.Pop();
                if (context.Name == contextName && found == null)
                {
                    found = context;
                }
                else
                {
                    tempStack.Push(context);
                }
            }

            while (tempStack.Count > 0)
            {
                _contextStack.Push(tempStack.Pop());
            }

            return found;
        }

        public InputContext ReplaceContext(InputContextConfig newContextData, IInputHandler newHandler)
        {
            if (_contextStack.Count > 0)
            {
                PopContext();
            }
            return PushContext(newContextData, newHandler);
        }

        public InputContext GetActiveContext()
        {
            return _contextStack.Count > 0 ? _contextStack.Peek() : null;
        }

        public bool HasContext(string name)
        {
            return _namedContexts.ContainsKey(name);
        }

        // New methods to manage input action subscriptions per context
        private void SubscribeToContext(InputContext context)
        {
            var actions = context.GetAllActions();
            foreach (var action in actions)
            {
                SubscribeToAction(action, context);
            }
        }

        private void UnsubscribeFromContext(InputContext context)
        {
            var actions = context.GetAllActions();
            foreach (var action in actions)
            {
                UnsubscribeFromAction(action);
            }
        }

        private void SubscribeToAction(InputAction action, InputContext context)
        {
            // Don't double-subscribe
            if (_activeActions.ContainsKey(action)) return;

            action.performed += OnInputPerformed;
            action.started += OnInputStarted;
            action.canceled += OnInputCanceled;
            
            _activeActions[action] = context;
        }

        private void UnsubscribeFromAction(InputAction action)
        {
            if (!_activeActions.ContainsKey(action)) return;

            action.performed -= OnInputPerformed;
            action.started -= OnInputStarted;
            action.canceled -= OnInputCanceled;
            
            _activeActions.Remove(action);
        }

        private void OnInputPerformed(InputAction.CallbackContext context)
        {
            ProcessInput(context);
        }

        private void OnInputStarted(InputAction.CallbackContext context)
        {
            ProcessInput(context);
        }

        private void OnInputCanceled(InputAction.CallbackContext context)
        {
            ProcessInput(context);
        }

        private void ProcessInput(InputAction.CallbackContext context)
        {
            //Debug.Log($"InputManager.ProcessInput: action={context.action.name}, phase={context.phase}, value={context.ReadValueAsObject()}");
            string actionName = context.action.name;

            // Handle global inputs first (search through entire stack)
            foreach (InputContext ctx in _contextStack)
            {
                if (ctx.CanHandleAction(actionName))
                {
                    var flag = ctx.GetActionFlag(actionName);
                    if ((flag & InputFlags.Global) != 0)
                    {
                        if (ctx.Handler.HandleInput(context, actionName.ToLower(), flag))
                        {
                            //_logger.Log($"Phase: {context.phase}, Handled global input: {actionName} in context: {ctx.Name}, value: {context.ReadValueAsObject()}");
                            return;
                        }
                    }
                }
            }

            // Process from top of stack down
            foreach (InputContext ctx in _contextStack)
            {
                if (ctx.CanHandleAction(actionName))
                {
                    var flag = ctx.GetActionFlag(actionName);
                    if (ctx.Handler.HandleInput(context, actionName.ToLower(), flag))
                    {
                        //_logger.Log($"Phase: {context.phase}, Handler: {ctx.Name}, ActionName: {actionName}, Value: {context.ReadValueAsObject()}");
                        if (ctx.BlockLowerContexts)
                        {
                            return;
                        }
                    }
                }
            }

            //_logger.Log($"Unhandled input: {actionName}, value: {context.ReadValueAsObject()}");
        }

        // Debug method
        [System.Serializable]
        public class DebugContextInfo
        {
            public string name;
            public string handlerType;
            public InputFlags allowedInputs;
            public bool blocking;
            public bool persistent;
        }

        public DebugContextInfo[] GetDebugInfo()
        {
            return _contextStack.Select(ctx => new DebugContextInfo
            {
                name = ctx.Name,
                handlerType = ctx.Handler.GetType().Name,
                allowedInputs = ctx.AllowedInputs,
                blocking = ctx.BlockLowerContexts,
                persistent = ctx.Persistent
            }).ToArray();
        }
    }
}