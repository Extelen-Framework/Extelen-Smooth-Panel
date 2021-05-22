using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Extelen.UI {

    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class SmoothPanel : MonoBehaviour {
        
        //Set Params

            //Non Static
            [Header("Smooth Panel References")]
            [SerializeField] private RectTransform m_rectTransform = null;
            [SerializeField] private CanvasGroup m_canvasGroup = null;
            public CanvasGroup CanvasGroup {get => m_canvasGroup; } 

            [Header("Smooth Panel Values")]
            [SerializeField] private Vector2 m_activePosition = Vector2.zero;
            [SerializeField] private Vector2 m_unactivePosition = new Vector2(0, -32);

            [Header("Smooth Panel Animation")]
            [SerializeField] private bool m_unscaledDeltaTime = true;

            [SerializeField] private float m_inTime = 0.25f;
            public float InTime {set => m_inTime = value; }

            [SerializeField] private float m_outTime = 0.25f;
            public float OutTime {set => m_outTime = value; }

            [SerializeField] private AnimationCurve m_animationCurve = 
                new AnimationCurve( 
                    new Keyframe[2] {
                        new Keyframe(0, 0),
                        new Keyframe(1, 1),
                        }
                    );

            [Header("Smooth Panel Calls")]
            [SerializeField] private UnityEvent m_inCalls = null;
            public UnityEvent InCalls {set => m_inCalls = value; }

            [SerializeField] private UnityEvent m_outCalls = null;
            public UnityEvent OutCalls {set => m_outCalls = value; }

            private Coroutine m_routine = null;
            private bool m_isOpen = false;
                
        //Methods
            
            //MonoBehaviour
            private void OnValidate() {

                if (m_canvasGroup == null) TryGetComponent<CanvasGroup>(out m_canvasGroup);
                if (m_rectTransform == null) TryGetComponent<RectTransform>(out m_rectTransform);
                }
                    
            //Non Static
            public void ToggleActive() => ToggleActive(false);
            public void ToggleActive(bool instant) {

                if (m_isOpen) Close(instant);
                else Open(instant);
                }

            public void Open() => Open(false);
            public void Open(bool instant) {
                
                if (m_isOpen) return;
                if (m_routine != null) return;
                gameObject.SetActive(true);
                m_isOpen = true;

                if (instant) {

                    m_rectTransform.anchoredPosition = m_activePosition;
                    m_canvasGroup.alpha = 1;
                    m_canvasGroup.interactable = true;

                    if (m_inCalls != null) m_inCalls.Invoke();
                    }
                
                else {
                    
                    m_routine = StartCoroutine(PanelIORoutine(true, m_inCalls));
                    }
                }

            public void Close() => Close(false);
            public void Close(bool instant) {
                
                if (!m_isOpen) return;
                if (m_routine != null) return;
                m_isOpen = false;

                if (instant) {

                    m_rectTransform.anchoredPosition = m_unactivePosition;
                    m_canvasGroup.alpha = 0;
                    m_canvasGroup.interactable = false;

                    if (m_outCalls != null) m_outCalls.Invoke();
                    gameObject.SetActive(false);
                    }
                
                else {
                    
                    m_routine = StartCoroutine(PanelIORoutine(false, m_outCalls));
                    }
                }

        //Coroutines.
        private IEnumerator PanelIORoutine(bool active, UnityEvent call) {

            Vector2 m_pStartValue = active ? m_unactivePosition : m_activePosition;
            Vector2 m_pEndValue = active ? m_activePosition : m_unactivePosition;

            float m_cgStartValue = active ? 0 : 1;
            float m_cgEndValue = active ? 1 : 0;

            float m_time = active ? m_inTime : m_outTime;

            for(float i = 0; i < m_time; i += m_unscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime) {
                
                float m_evaluation = m_animationCurve.Evaluate(i / m_time);

                m_rectTransform.anchoredPosition = Vector2.Lerp(m_pStartValue, m_pEndValue, m_evaluation);
                m_canvasGroup.alpha = Mathf.Lerp(m_cgStartValue, m_cgEndValue, m_evaluation);
                yield return null;
                }
            
            m_rectTransform.anchoredPosition = m_pEndValue;
            m_canvasGroup.alpha = m_cgEndValue;
            m_canvasGroup.interactable = active;
            m_routine = null;

            if (call != null) call.Invoke();
            if (!active) gameObject.SetActive(false);
            }
        }
    }