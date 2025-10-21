# PauseMenu API Reference

This document provides an API reference for the public members of the `PauseMenu` class, a singleton in Unity that controls the main HUD and pause menu in the ComBots game.

## Properties

### `static PauseMenu Instance { get; private set; }`
- **Description**: Provides access to the singleton instance of `PauseMenu`.
- **Type**: `PauseMenu`
- **Access**: Read-only

### `bool IsOpen { get; private set; }`
- **Description**: Indicates whether the game is paused and the entire HUD is visible.
- **Type**: `bool`
- **Access**: Read-only

## Methods

### `static IDisposable OnButtonsVisible(Action x)`
- **Description**: Subscribes an action to be called when the bottom app buttons become visible (game paused).
- **Parameters**:
  - `x` (`Action`): The action to invoke when the event occurs.
- **Returns**: `IDisposable` - A disposable object for unsubscribing from the event.

### `static IDisposable OnButtonsMinimized(Action x)`
- **Description**: Subscribes an action to be called when the bottom buttons become partially visible (game unpaused) or hidden (during dialog or submenus).
- **Parameters**:
  - `x` (`Action`): The action to invoke when the event occurs.
- **Returns**: `IDisposable` - A disposable object for unsubscribing from the event.