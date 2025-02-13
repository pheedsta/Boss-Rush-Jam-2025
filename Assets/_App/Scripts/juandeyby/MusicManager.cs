using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace _App.Scripts.juandeyby
{
    [DefaultExecutionOrder(-100)]
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private GameObject WwiseGlobal;
        private int _currentFilesDownloaded = 0;
        
        // Footstep
        private bool _isFootstepPlaying;
        private float _footstepMaxTime = 0.2f;
        private float _currentFootstepTimer;
        
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
            StartCoroutine(OnPhaseTransitionChanged(phase));
        }

        private IEnumerator OnPhaseTransitionChanged(GamePhase phase)
        {
            var time = 0f;
            var maxTime = 4f;
            switch (phase)
            {
                case GamePhase.Phase2:
                    while (time < maxTime)
                    {
                        time += Time.deltaTime;
                        AkUnitySoundEngine.SetRTPCValue("LayerControl", Mathf.Lerp(1, 2, time / maxTime));
                        yield return null;
                    }
                    break;
                case GamePhase.Phase3:
                    while (time < maxTime)
                    {
                        time += Time.deltaTime;
                        AkUnitySoundEngine.SetRTPCValue("LayerControl", Mathf.Lerp(2, 3, time / maxTime));
                        yield return null;
                    }
                    break;
                case GamePhase.End:
                    AkUnitySoundEngine.PostEvent("Stop_BossMusic", gameObject);
                    AkUnitySoundEngine.PostEvent("Play_BossDeath_Music", gameObject); 
                    break;
            }
        }
        
        public void OnPhase1()
        {
            AkUnitySoundEngine.SetRTPCValue("LayerControl", 1f);
        }

        public void PlayMainMusic()
        {
            AkUnitySoundEngine.PostEvent("Play_MainMenu", gameObject);
        }
        
        public void PlayBossMusic()
        {
            AkUnitySoundEngine.PostEvent("Stop_MainMenu", gameObject);
            AkUnitySoundEngine.PostEvent("Play_BossMusic", gameObject);
            PlayBossLinePhase1();
        }
        
        public void Play()
        {
            AkUnitySoundEngine.SetState("New_State_Group", "Playing");
        }
        
        public void Pause()
        {
            AkUnitySoundEngine.SetState("New_State_Group", "Paused");
        }

        public void PlaySwordSlashDraw()
        {
            AkUnitySoundEngine.PostEvent("SwordSlash_Draw", gameObject);
        }
        
        public void PlaySwordHit()
        {
            AkUnitySoundEngine.PostEvent("SwordSlash_Hit", gameObject);
        }
        
        public void PlaySwordWhoosh()
        {
            AkUnitySoundEngine.PostEvent("SwordSlash_Whoosh", gameObject);
        }
        
        public void PlayAcidSpray()
        {
            AkUnitySoundEngine.PostEvent("Acid_Spray", gameObject);
        }
        
        public void PlayAcidHit()
        {
            AkUnitySoundEngine.PostEvent("Acid_Hit", gameObject);
        }
        
        public void PlaySizzle()
        {
            AkUnitySoundEngine.PostEvent("Acid_Sizzle", gameObject);
        }
        
        public void StopSizzle()
        {
            AkUnitySoundEngine.PostEvent("Acid_Sizzle_Stop", gameObject);
        }
        
        public void PlayPortalSummon()
        {
            AkUnitySoundEngine.PostEvent("PortalSummon_Start", gameObject);
        }
        
        public void StopPortalSummon()
        {
            AkUnitySoundEngine.PostEvent("PortalSummon_Stop", gameObject);
        }

        public void PlaySpellFire()
        {
            AkUnitySoundEngine.PostEvent("FireSpell_Fire", gameObject);
        }
        
        public void PlaySpellBurn()
        {
            AkUnitySoundEngine.PostEvent("FireSpell_Burn", gameObject);
        }
        
        public void PlaySpellHit()
        {
            AkUnitySoundEngine.PostEvent("FireSpell_Hit", gameObject);
        }
        
        public void PlayPickUpHealth()
        {
            AkUnitySoundEngine.PostEvent("PickUpHealth", gameObject);
        }
        
        public void PlayPickUpShard()
        {
            AkUnitySoundEngine.PostEvent("PickUpShard", gameObject);
        }
        
        public void PlayHurt()
        {
            AkUnitySoundEngine.PostEvent("Hurt_Player", gameObject);
        }
        
        public void PlayBossLinePhase1()
        {
            AkUnitySoundEngine.PostEvent("Play_Boss_Line_Phase1", gameObject);
        }
        
        public void PlayBossLinePhase3()
        {
            AkUnitySoundEngine.PostEvent("Play_Boss_Line_Phase3", gameObject);
        }
        
        public void PlayJumpLand()
        {
            AkUnitySoundEngine.PostEvent("JumpLand_Player", gameObject);
        }

        private void Update()
        {
            Footstep();
        }

        private void Footstep()
        {
            if (!_isFootstepPlaying) return;
            if (_currentFootstepTimer > _footstepMaxTime)
            {
                _currentFootstepTimer = 0;
                PlayFootstep();
            }
            else
            {
                _currentFootstepTimer += Time.deltaTime;
            }
        }

        public void PlayFootstep()
        {
            AkUnitySoundEngine.PostEvent("Footstep_Player", gameObject);
        }
        
        public void StartFootstep()
        {
            _isFootstepPlaying = true;
        }
        
        public void StopFootstep()
        {
            _isFootstepPlaying = false;
        }
        
        public void Jump()
        {
            AkUnitySoundEngine.PostEvent("Jump_Player", gameObject);
        }
    }
}
