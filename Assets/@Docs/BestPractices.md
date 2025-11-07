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