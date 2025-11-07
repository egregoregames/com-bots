# Best Practices

This project will be designed with the use of as many standalone, modular systems
as needed. The use of Public APIs will be as minimal as possible, instead relying
on event subscriptions to keep systems clean and reduce "spaghetti code".

## R3

### Observables

To capitalize on event-based coding, we will be employing the use of Observable
patters through R3. I highly recommend reading up on some of the basics of R3 in
C#, especially the Unity implementation we are using, which is here:
[Unity R3](https://github.com/Cysharp/R3?tab=readme-ov-file#unity).

This implementation of R3 offers many extensions and wrappers for most Unity API,
such as SerializedReactiveProperty and Observables for in-game ticks and update 
loops or time-based intervals.

### MonobehaviourR3

Further enhancing our coding philosophy is a new inheritable base class: MonobehaviourR3.
Using this class as a base class for most scripts offers many advantages.
- Simple event subscription and automatic disposal on Destroy
- Hot reload compatibility
- Built in, simplistic Log method with verbosity control built in to the editor

### UnityEventR3

To make Unity Events more compatible with R3, I have created a wrapper and pattern
that should be used when possible. Advantages:
- EventHandler-like access control
- One-line subscribe and auto disposal on Destroy

## Feature Development

### Summary

Before working on a feature, please give your senior a brief summary of the new classes
that will be created, especially any new singletons, and a synopsis of how the new
feature will work. Please get approval before beginning work.

### Branches / Pull requests

There should be only exactly one feature per branch/pull request, unless it is a special
circumstance approved by a senior. Be judicious about separating sub-features
and always consult a senior if you are unsure if you should create a new branch
for something or not.

### Prefabs

To minimize merge collisions, a standard practice in Unity development is to use
a lot of prefabs. In general, follow these two rules:
- Any object that would be a root object in the scene should be a prefab
- Any nested object that is used more than once should be a prefab

Take great care to ensure that only instance modifications exist in a scene.
Check your overrides often to ensure that nothing that is supposed to be a 
permanent change to a prefab ends up as scene data.

### Scenes

In general, when developing a feature, use the following guidelines for scenes:
- If your feature can be completely built and demonstrated in a test scene, it should be
- Do not use production scenes for feature development
- Feature implementation in a production scene should be handled as a separate task on a separate branch

### Commits

Unity likes to sneak in unintended changes to files while working in the editor.
- Always double check that every file being committed to your branch is an intended change.
- Do not commit meaningless changes (i.e. Unity constantly messing with font files)
- When in doubt, stash your current changes and test! If your feature still works as intended, you can discard the stash

### LFS

There is nothing in this project that would need LFS. If you have a scene file that 
is so big that you need LFS, then you aren't prefabbing enough. Remember that you 
can also prefab groups of prefabs into a larger prefab when needed.

Scene files should NEVER use GitLFS!

## Redundant singletons

Old singletons such as InputManager, GameStateMachine, GlobalEntryPoint, PoolManager, etc. stem
from a lack of understanding of existing Unity API and events. These singletons
are currently deprecated and in the process of being phased out. Do not use them
going forward. Instead, do the following:

### Use InputSystem instances

The old InputManager singleton appears to have been a wrapper to provide
a way for classes to subscribe to events through Interfacing. This ended
up creating a lot of extra code, for example when certain methods of the
interface were being unused, leaving blank methods. There was also the need
to create complex if/then or switch statements to determine what kind of input
was coming through, and what stage it was in (when in the vast majority of cases
you only need "performed").

Instead, do what the actual Unity API documentation wants you to do.

Here's an example of how it's used in PauseMenu:

```csharp
public class PauseMenu : MonoBehaviourR3
{
    private InputSystem_Actions Inputs { get; set; }

    protected override void Initialize()
    {
        base.Initialize();
        Inputs = new();

        var onOpenMenu = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.Player.OpenMenu.performed += h,
            h => Inputs.Player.OpenMenu.performed -= h);

        AddEvents(
            onOpenMenu.Subscribe(_ => OnOpenMenuInput());
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

    private void OnOpenMenuInput()
    {
        // Toggle the menu opening
    }
}
```

This does the same thing as using the old InputManager but everything is
self-contained, cleaner and does not rely at all on additional redundant systems,
instead relying only on existing UnityAPI, making it more resistant to changes 
and maintainable in the future, as the functionality stems from a widely documented, 
universally used API rather than some homebrewed megasystem

### Instead of GameStateMachine, use events

GameStateMachine seems to exist because someone didn't have a clear understanding
of how events worked in C#/Unity. It's a bulky system that is completely unneeded, 
and unfortunately undocumented.

Instead, if a certain system needs to "react" to something else that happens, use
events. Specifically, use the new Observable pattern events. For example, using pause
menu again, here's some events that it can invoke:

```csharp
public class PauseMenu : MonoBehaviourR3
{
    public static PauseMenu Instance { get; private set; }

    private static UnityEventR3 _onButtonsVisible = new();
    /// <summary>
    /// Fires when the bottom app buttons become visible (game paused)
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static IDisposable OnButtonsVisible(Action x) => _onButtonsVisible.Subscribe(x);

    private static UnityEventR3 _onButtonsMinimized = new();
    /// <summary>
    /// Invoked when the bottom buttons become partially visible (game unpaused) or hidden (dialog, submenus)
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static IDisposable OnButtonsMinimized(Action x) => _onButtonsMinimized.Subscribe(x);

    /// <summary>
    /// True if the game is paused and the entire HUD is visible
    /// </summary>
    public bool IsOpen { get; private set; }

    private Visibility _currentBottomState = Visibility.Partial;

    private void ToggleIsOpen(bool isOpen)
    {
        IsOpen = isOpen;
        UpdateVisibility();
    }

    private async void UpdateVisibility()
    {
        // Set the gameobject visible or invisible here, then lets
        // invoke some events below

        if (_currentBottomState == Visibility.Visible)
        {
            _onButtonsVisible?.Invoke();
        }
        else
        {
            _onButtonsMinimized?.Invoke();
        }
    }

    private enum Visibility
    {
        Partial,
        Hidden,
        Visible
    }
}
```

And here's the Menu Description Panel, which describes the currently selected
app in the Pause Menu, listens to those events and turns itself on or
off depending on if the pause menu buttons are visible.

```csharp
public class MenuDescriptionPanel : MonoBehaviourR3
{
    public TextMeshProUGUI menuPanelName;
    public TextMeshProUGUI menuPanelDescription;
    public Image background;

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            PauseMenu_AppButton.OnSelected(SetDescription),
            PauseMenu.OnButtonsVisible(() => gameObject.SetActive(true)),
            PauseMenu.OnButtonsMinimized(() => gameObject.SetActive(false)));

        gameObject.SetActive(false);
    }
    
    private void SetDescription(PauseMenu_AppButton info)
    {
        menuPanelName.text = info.DescriptionBoxTitle;
        menuPanelDescription.text = info.DescriptionBoxText;
        background.sprite = info.DescriptionBoxBackgroundSprite;
    }
}
```

This entirely cuts out the need for a Game State Machine, keeping the scripts
self contained, event based and free of API calls between same-level scripts (spaghetti code),
keeping the repository clean, reducing the possibility of merge conflicts, adding better
performance and more.

### Instead of GlobalEntryPoint, singletons should self-initialize and use async/await

It seems that global entry point existed to ensure that all singletons initialized
at the same time so there wouldn't be any null reference exceptions. But this
is exactly why async/await exists in C#. Let's look at an example, using PauseMenu again.

```csharp
public class PauseMenu : MonoBehaviourR3
{
    protected override void Initialize()
    {
        base.Initialize();
        SubscribeToDialogueManagerEvents();
    }

    private async void SubscribeToDialogueManagerEvents()
    {
        while (DialogueManager.instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        DialogueManager.instance.conversationStarted += ConversationStarted;
        DialogueManager.instance.conversationEnded += ConversationEnded;
    }
}
```

If a system runs into or COULD run into a null reference exception while
other systems are initializing (in this case DialogeManager), we can use
a simple async/await pattern to ensure the instance is ready before
we call it.

Some APIs, like PersistentGameData, have convenient code installed to
allow you to easily await for it to be initialized.

```csharp
    var instance = await PersistentGameData.GetInstanceAsync();
    // Do something with the instance
```

These simple patterns allow us to bypass the need for us to make an additional 
"level" of functionality that looks down and calls a bunch of same-level APIs.
This keeps out code self contained, free of merge conflicts and easy to maintain.

### Instead of PoolManager, systems should manage their own pools

Pool management isn't a new concept in C#/Unity, and while you could possibly
consider PoolManager a mid/lower level API, it has some issues.

The biggest problem with the PoolManager system is that it relies strictly
on a dictionary for functionality, which is not hot-reload safe without a
serialized list as a backing. Also, the dictionary is string key based,
which leaves the opportunity for typos and errors.

Futhermore, the "Pull" method is misleading as you're actually using "Pop" to
get an item.

Pools also have a tendancy to have changes in needs and functionality on a case
to case basis, and a one-size-fits-all solution usually ends up failing at some
point.

So instead, let's have our systems manage their own pools. Let's take a look
at AudioManager as an example:

```csharp
public class AudioManager : MonoBehaviourR3
{
    /// <summary>
    /// Should only ever include objects spawned on runtime. Is cleared on Awake
    /// </summary>
    [field: SerializeField, ReadOnly]
    private List<AudioSource> Pool { get; } = new();

    private AudioSource GetAudioSource()
    {
        AudioSource source = Pool.FirstOrDefault(x => !x.isPlaying);

        if (source == null)
        {
            var obj = new GameObject("AudioSource", typeof(AudioSource));
            DontDestroyOnLoad(obj);
            source = obj.GetComponent<AudioSource>();
            source.loop = false;
            Pool.Add(source);
        }

        return source;
    }
}
```

This entire pool is managed with basically a property and a method, as the 
individual needs of this particular system dictate simplicity over performance. 

As there's not going to ever be a lot of simultaneously playing sound effects, the 
performance considerations of FirstOrDefault is negligible. This system simply
scans the list for an audiosource that isn't playing and creates a new one when needed,
adding it to the pool. 

In actual gameplay, you wouldn't ever expect this pool to be
bigger than 1-5 items. The memory allocation considerations of List are therefore
also negligible and this is good to go with a very small amount of code. Plus, 
because the list is serialized, this is hot-reload safe.