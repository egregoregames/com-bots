using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ComBots.UI.src
{
    public class AutoFocusFirstSelectOption : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(FocusFirstButtonNextFrame());
        }

        private IEnumerator FocusFirstButtonNextFrame()
        {
            yield return null; // Wait one frame for UI to be fully set up

            // Find the first selectable child (should be one of the response buttons)
            var firstSelectable = GetComponentInChildren<Selectable>();

            if (firstSelectable != null)
            {
                EventSystem.current.SetSelectedGameObject(null); // Clear current selection
                firstSelectable.Select();
            }
            
            // Soft reset InputSystemUIInputModule to re-enable navigation
            var inputModule = EventSystem.current.currentInputModule as InputSystemUIInputModule;
            if (inputModule != null)
            {
                inputModule.enabled = false;
                yield return null; // wait 1 frame
                inputModule.enabled = true;
            }
        }
    }
}