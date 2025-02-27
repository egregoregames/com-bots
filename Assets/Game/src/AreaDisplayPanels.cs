using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.src
{
    public class AreaDisplayPanels : MonoBehaviour
    {
        [SerializeField] UISo uiSo;
        [SerializeField] InputSO inputSo;

        [SerializeField] RectTransform areaDisplay;

        [SerializeField] Relay relay;
        public float showTime = 1.5f;
        public float transitionTime = 0.5f;
        Vector2 startPosition;
        Vector2 endPosition;
        
        private void Awake()
        {
            uiSo.AreaSelected += DoTransition;
            //relay.OnRoomTransition();
        }
        
    void Start()
        {
            // Store initial positions
            startPosition = areaDisplay.anchoredPosition;

            // Compute closed positions (panels meet at center)
            float screenWidth = Screen.width;
            endPosition = new Vector2(0, 375);
        }

        public void DoTransition(Action callback, string name)
        {
            transform.GetComponentInChildren<TextMeshProUGUI>().text = name;
            StartCoroutine(TransitionCoRo());
        }

        private bool _isClosed = false;
        IEnumerator TransitionCoRo()
        {
            yield return new WaitForSeconds(.5f);

            In();
            yield return new WaitUntil(() => _isClosed);
            yield return new WaitForSeconds(showTime);
            Out();
            _isClosed = false;
        }

        public void In()
        {
            LeanTween.move(areaDisplay, endPosition, transitionTime).setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() => _isClosed = true);
        }

        public void Out()
        {
            LeanTween.move(areaDisplay, startPosition, transitionTime).setEase(LeanTweenType.easeInOutQuad);
        }
    }
}
