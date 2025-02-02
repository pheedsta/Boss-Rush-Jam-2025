using UnityEngine;
using UnityEngine.UI;

namespace _App.Scripts.juandeyby.UI
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        private Transform _unit;
        private Camera _mainCamera;
        private RectTransform _rectTransform; 
        private RectTransform _canvasRect;
        private Vector3 _offset;


        private void Awake()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogError("<color=red>Main camera not found!</color>");
            }

            _rectTransform = GetComponent<RectTransform>();
            _canvasRect = _rectTransform.parent as RectTransform;
        }

        public void SetUnit(Transform unit, Vector3 offset)
        {
            _unit = unit;
            _offset = offset;
        }
        
        /// <summary>
        /// Set the health bar fill amount between 0 and 1
        /// </summary>
        /// <param name="health"></param>
        public void SetHealth(float health)
        {
            healthBar.fillAmount = health;
        }

        private void Update()
        {
            if (_unit == null || _mainCamera == null || _canvasRect == null) return;

            var unitViewportPosition = _mainCamera.WorldToViewportPoint(_unit.position + _offset);

            var canvasSize = _canvasRect.sizeDelta;
            var localPosition = new Vector2(
                (unitViewportPosition.x - 0.5f) * canvasSize.x,
                (unitViewportPosition.y - 0.5f) * canvasSize.y
            );

            _rectTransform.localPosition = localPosition;
        }
    }
}
