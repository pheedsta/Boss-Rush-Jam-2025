using UnityEngine;

namespace _App.Scripts.juandeyby.UI
{
    public class UIHealthBar : MonoBehaviour
    {
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

        private void Update()
        {
            if (_unit == null || _mainCamera == null || _canvasRect == null) return;
            var unitScreenPosition = _mainCamera.WorldToScreenPoint(_unit.position + _offset);

            if (unitScreenPosition.z > 0)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvasRect,
                        unitScreenPosition,
                        _mainCamera,
                        out var localPoint))
                {
                    _rectTransform.localPosition = localPoint;
                }
            }
            else
            {
                _rectTransform.gameObject.SetActive(false);
            }
        }
    }
}
