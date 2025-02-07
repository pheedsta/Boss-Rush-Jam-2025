using Lean.Gui;
using UnityEngine;

namespace _App.Scripts.juandeyby.UI
{
    public class UIPreStart : MonoBehaviour, IUIPanel
    {
        [SerializeField] private LeanButton preStartButton;

        private void Awake()
        {
            preStartButton.OnClick.AddListener(OnPreStartButtonClicked);
        }

        private void OnPreStartButtonClicked()
        {
            UIServiceLocator.Get<UIManager>().ShowStartPanel();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            ServiceLocator.Get<MusicManager>().PlayMainMusic();
            gameObject.SetActive(false);
        }
    }
}