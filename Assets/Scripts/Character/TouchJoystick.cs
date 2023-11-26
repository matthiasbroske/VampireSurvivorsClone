using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.Layouts;

namespace Vampire
{
    /// <summary>
    /// Touch interactable joystick. Utilizes IPointer interfaces to ensure
    /// make touches interacting with other UI elements easier to handle.
    /// </summary>
    public class TouchJoystick : OnScreenControl, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private bool permanent = false;
        [SerializeField] private float joystickRadius;
        [SerializeField] private RectTransform joystick, joystickBounds;
        [SerializeField] private UnityEvent<Vector2> onJoystickMoved;
        [SerializeField] private UnityEvent onStartTouch, onEndTouch;
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;
        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private RectTransform controlRect;
        private bool beingTouched = false;
        private Vector2 initialTouchPosition;

        public bool BeingTouched { get => beingTouched; }

        void Awake()
        {
            controlRect = GetComponent<RectTransform>();
        }

        void Update()
        {
            if (beingTouched)
            {
                if (Time.timeScale > 0)
                {
                    Vector2 touchPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(controlRect, Input.mousePosition, null, out touchPosition);
                    UpdateTouch(touchPosition);
                }
                else
                {
                    EndTouch();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 touchPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(controlRect, eventData.position, null, out touchPosition);
            StartTouch(permanent ? joystick.localPosition : touchPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            EndTouch();
        }

        public void StartTouch(Vector2 touchPosition)
        {
            if (Time.timeScale > 0)
            {
                beingTouched = true;
                // Save the initial touch position
                initialTouchPosition = touchPosition;
                // Position the joystick
                joystick.localPosition = initialTouchPosition;
                joystickBounds.localPosition = initialTouchPosition;
                // Size the joystick
                joystickBounds.sizeDelta = Vector2.one * joystickRadius * 2;
                // Enable the joystick
                joystick.gameObject.SetActive(true);
                joystickBounds.gameObject.SetActive(true);
                // Invoke touch started callback
                onStartTouch.Invoke();
            }
        }

        public void UpdateTouch(Vector2 touchPosition)
        {
            Vector2 joystickDelta = (touchPosition - initialTouchPosition);
            Vector2 moveDirection = joystickDelta.normalized;
            // Update the joystick position, locking it within the joystick bounds
            joystick.localPosition = joystickDelta.magnitude > joystickRadius ? initialTouchPosition + moveDirection * joystickRadius : touchPosition;
            // Invoke on move callback
            onJoystickMoved.Invoke(moveDirection);
            //SendValueToControl<Vector2>(moveDirection);
        }

        public void EndTouch()
        {
            joystick.localPosition = joystickBounds.localPosition;
            // Disable the joystick
            joystick.gameObject.SetActive(permanent);
            joystickBounds.gameObject.SetActive(permanent);
            // Invoke touch ended callback
            onJoystickMoved.Invoke(Vector2.zero);
            //SendValueToControl<Vector2>(Vector2.zero);
            onEndTouch.Invoke();
            beingTouched = false;
        }
    }
}
