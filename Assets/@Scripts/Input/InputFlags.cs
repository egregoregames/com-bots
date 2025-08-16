using UnityEngine;

namespace ComBots.Inputs
{
    [System.Flags]
    public enum InputFlags
    {
        None = 0,
        Movement = 1 << 0,        // Arrows/Joystick for player movement
        Interaction = 1 << 1,     // E key for world interaction
        Speed = 1 << 2,           // S key for speed boost
        Camera = 1 << 3,          // Mouse X/Y for camera
        UI = 1 << 4,              // Arrows/Joystick for UI navigation
        Confirm = 1 << 5,         // A key for confirmation
        Cancel = 1 << 6,          // S key for canceling (overlaps with Speed!)
        Pause = 1 << 7,           // Space for pause/unpause
        Combat = 1 << 8,          // Combat-specific inputs
        Global = 1 << 9,          // Always processed inputs
        All = ~0
    }
}