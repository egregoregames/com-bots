# README

## Contents

- Install
- Use
- About

## Install

1. Add to your ~/Packages folder.

## Use

### Setup

1. Select the UI element in your scene.
2. Click Add Component > OccaSoftware > UI > Gradient.
3. Configure your Gradient options.

### Runtime API

1. Get a reference to the UIGradient component you want to modify. (e.g., `UIGradient uig = GetComponent<UIGradeint>()`)
2. Change the property (e.g., `uig.radius = 50f`).
3. Call `Recreate()` on the Component (e.g., `uig.Recreate()`).

## Notes

- To enable **Transparency**, set your **Canvas Render Mode** to **Screen Space - Camera**.
