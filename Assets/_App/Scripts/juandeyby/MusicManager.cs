using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    [DefaultExecutionOrder(-100)]
    public class MusicManager : MonoBehaviour
    {
        private void OnEnable()
        {
            ServiceLocator.Register<MusicManager>(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<MusicManager>();
        }

        private void Start()
        {
            ServiceLocator.Get<GameManager>().OnGamePhaseChanged += OnGamePhaseChanged;
        }

        private void OnGamePhaseChanged(GamePhase phase)
        {
            SetPhase(phase);
        }

        public void PlayMainMusic()
        {
            AkUnitySoundEngine.PostEvent("Play_MainMenu", gameObject);
        }
        
        public void PlayBossMusic()
        {
            AkUnitySoundEngine.PostEvent("Stop_MainMenu", gameObject);
            AkUnitySoundEngine.PostEvent("Play_BossMusic", gameObject);
            
            var phase = ServiceLocator.Get<GameManager>().GetGamePhase();
            AKRESULT akresult;
            switch (phase)
            {
                case GamePhase.Phase1:
                    akresult = AkUnitySoundEngine.SetRTPCValue("LayerControl", 1);
                    Debug.LogWarning($"<color=red>Phase 1 - {akresult}</color>");
                    break;
                case GamePhase.Phase2:
                    akresult = AkUnitySoundEngine.SetRTPCValue("LayerControl", 2);
                    Debug.LogWarning($"<color=red>Phase 2 - {akresult}</color>");
                    break;
                case GamePhase.Phase3:
                    akresult = AkUnitySoundEngine.SetRTPCValue("LayerControl", 3);
                    Debug.LogWarning($"<color=red>Phase 3 - {akresult}</color>");
                    break;
            }
        }
        
        public void Play()
        {
            AkUnitySoundEngine.SetState("New_State_Group", "Playing");
        }
        
        public void Pause()
        {
            AkUnitySoundEngine.SetState("New_State_Group", "Paused");
        }

        private void SetPhase(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.Phase1:
                    AkUnitySoundEngine.SetRTPCValue("Set_LayerControl", 1);
                    Debug.LogWarning("<color=red>Phase 1</color>");
                    break;
                case GamePhase.Phase2:
                    AkUnitySoundEngine.SetRTPCValue("Set_LayerControl", 2);
                    Debug.LogWarning("<color=red>Phase 2</color>");
                    break;
                case GamePhase.Phase3:
                    AkUnitySoundEngine.SetRTPCValue("Set_LayerControl", 3);
                    Debug.LogWarning("<color=red>Phase 3</color>");
                    break;
            }
        }

        public void PlayEndingMusic()
        {
            AkUnitySoundEngine.PostEvent("Stop_BossMusic", gameObject);
            AkUnitySoundEngine.PostEvent("Play_BossDeath_Music", gameObject);
        }
    }
}
