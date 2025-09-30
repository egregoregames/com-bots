# Dialogue System Integration with Utility Lister

## Overview
The Dialogue system has been successfully refactored to use the new UtilityLister component instead of its built-in option management system.

## Setup Instructions

### 1. Dialogue GameObject Setup
- The main Dialogue GameObject should have:
  - `UIDocument` component with source set to `Global.Dialogue.uxml`
  - `DialogueController` component

### 2. Utility Lister GameObject Setup
- Create a separate child GameObject under the Dialogue GameObject
- Add these components:
  - `UIDocument` component with source set to `Utility.Listing.uxml`
  - `WC_Lister` component
- Configure the WC_Lister:
  - Set UI Document reference to the UIDocument on the same GameObject
  - Set Option Template to `Utility.Listing.Option.uxml`
  - Configure max visible options (default: 8)
  - Set include back option to `true`
  - Set back option label to desired text (e.g., "Cancel")

### 3. DialogueController Configuration
- Assign the `WC_Lister` component reference to the DialogueController's `_utilityLister` field

## How It Works

### Dialogue Flow
1. **Dialogue Display**: When `SetActive()` is called, the dialogue text animates in
2. **Option Trigger**: When user presses confirm after text animation completes, if options are available, `ShowOptions()` is called
3. **Option Navigation**: The WC_Lister handles all input navigation (up/down/confirm/cancel)
4. **Option Selection**: When an option is selected, the callback is invoked and the lister is hidden
5. **Cleanup**: When dialogue exits, the lister is properly hidden and cleaned up

### Input Handling
- The DialogueController routes input to the WC_Lister when it's active
- When the lister is not active, DialogueController handles basic dialogue progression
- All navigation, confirmation, and cancellation is handled by the utility lister

### CSS Integration
- The dialogue CSS now includes positioning rules for the utility lister
- The utility lister appears in the same position as the old options system
- Smooth transitions and animations are maintained

## Benefits of This Refactor

1. **Code Reusability**: The option listing logic can now be used anywhere in the project
2. **Maintainability**: All option management logic is centralized in the WC_Lister
3. **Consistency**: All menus using the utility lister will have consistent behavior
4. **Flexibility**: The utility lister can be easily customized for different contexts

## Key Changes Made

### DialogueController.cs
- Removed all option management code (selection, scrolling, visual updates)
- Removed option-related fields and constants
- Added reference to WC_Lister component
- Simplified HandleInput to route to utility lister
- Added ShowOptions() method to interface with utility lister
- Simplified initialization and cleanup

### Global.Dialogue.uxml
- Removed the built-in options container
- Cleaner structure focused on dialogue display

### Global.Dialogue.uss
- Added positioning rules for utility lister in dialogue context
- Maintained visual consistency with original design

## Testing Checklist

- [ ] Dialogue appears correctly
- [ ] Text animation works
- [ ] Options appear when pressing confirm after text completes
- [ ] Navigation (up/down) works in option list
- [ ] Option selection works (confirm)
- [ ] Cancel/back works
- [ ] Scrolling works for long option lists
- [ ] Overflow arrows appear correctly
- [ ] Visual styling matches original design
- [ ] Dialogue cleanup works properly
- [ ] Multiple dialogues in sequence work correctly
