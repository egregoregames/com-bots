using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Allows a simple pattern to block inputs during sensitive async/await operations.
/// Meant to be used with a <see cref="using"/> block before an await.
/// <para />
/// Example: 
/// <para />
/// using var block = <see cref="InputBlocker.GetBlock(string)"/>;<br />
/// await <see cref="PersistentGameData.GetInstanceAsync()"/>;
/// </summary>
public static class InputBlocker
{
    private const bool _debug = false;

    private static List<Block> _items = new();

    private static UnityEventR3 _onStatusUpdated = new();
    /// <summary>
    /// Is invoked when input should be blocked or unblocked. See <see cref="IsInputBlocked"/>
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static IDisposable OnStatusUpdated(Action x) => _onStatusUpdated.Subscribe(x);

    /// <summary>
    /// Should be read by any code that uses <see cref="InputSystem"/> inputs
    /// </summary>
    public static bool IsInputBlocked => _items.Count > 0;

    public class Block : IDisposable
    {
        public Block(string reason) 
        {
            if (_debug)
            {
                Debug.Log($"Adding input block: {reason}");
            }

            Reason = reason;
            _items.Add(this);
        }

        /// <summary>
        /// The reason this block was called. Useful for debugging
        /// </summary>
        public string Reason { get; }

        public void Dispose()
        {
            if (_debug)
            {
                Debug.Log($"Removing input block: {Reason}");
            }

            _items.Remove(this);
        }
    }

    public static Block GetBlock(string reason)
    {
        return new Block(reason);
    }
}