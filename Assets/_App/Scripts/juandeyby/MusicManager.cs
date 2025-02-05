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
        
        private void OnEnable()
        {
            ServiceLocator.Register<MusicManager>(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<MusicManager>();
        }

        private void Awake()
        {
            // AkUnitySoundEngine.LoadBank("Main", out var bankID);
            // Debug.Log("Bank ID: " + bankID);
            // ServiceLocator.Get<MusicManager>().PlayMainMusic();
            
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/L955tkLgRJitk5A/download/Init.bnk", "Init.bnk"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/rARFeQBPDFg9C88/download/Init.json", "Init.json"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/A4C4EpLmc6ZmJL9/download/Init.txt", "Init.txt"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/2jLNTnX4JtYGSC3/download/Main.bnk", "Main.bnk"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/YZ8XaHnPNQs49Td/download/Main.json", "Main.json"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/DcydRJ3523kb4aP/download/Main.txt", "Main.txt"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/KJAtWfSjzjCp7wy/download/PlatformInfo.json", "PlatformInfo.json"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/YzyaJAWzfHRxidb/download/PluginInfo.json", "PluginInfo.json"));
            //
            //
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/q2bYZ7YWHm9yBjX/download/Wwise_IDs.h", "Wwise_IDs.h", "Audio/GeneratedSoundBanks"));
            // StartCoroutine(DownloadFile("https://lidyi.com/nextcloud/index.php/s/rARFeQBPDFg9C88/download/ProjectInfo.json", "ProjectInfo.json", "Audio/GeneratedSoundBanks"));
        }

        
        private IEnumerator DownloadFile(string url, string fileName = "What.json", String destination = "Audio/GeneratedSoundBanks/Web")
        {
            // Realizar la solicitud para descargar el archivo
            UnityWebRequest request = UnityWebRequest.Get(url);

            // Esperar hasta que la descarga termine
            yield return request.SendWebRequest();

            // Comprobar si hubo algún error
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Ruta de destino en persistentDataPath con la estructura de carpetas deseada
                string folderPath = Path.Combine(Application.persistentDataPath, destination);
            
                // Crear las carpetas necesarias si no existen
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Nombre del archivo a guardar
                string filePath = Path.Combine(folderPath, fileName);

                // Escribir los datos descargados en el archivo
                File.WriteAllBytes(filePath, request.downloadHandler.data);

                Debug.Log("Archivo descargado y guardado en: " + filePath);

                // Cargar el archivo (si es necesario)
                // string text = File.ReadAllText(filePath);
                // Debug.Log("Texto cargado: " + text);
                _currentFilesDownloaded++;
                // OnDownloadComplete();
            }
            else
            {
                Debug.LogError("Error en la descarga: " + request.error);
            }
        }
        
        // private void OnDownloadComplete()
        // {
        //     if (_currentFilesDownloaded < 8) return;
        //     
        //     WwiseGlobal.SetActive(true);
        //     Debug.Log("¡La descarga ha terminado y puedes ejecutar tu código ahora!");
        //     AkUnitySoundEngine.LoadBank("Main", out var bankID);
        //     Debug.Log("Bank ID: " + bankID);
        //     
        //     ServiceLocator.Get<MusicManager>().PlayMainMusic();
        // }
        
        private void Start()
        {
            // AkUnitySoundEngine.LoadBank("Main", out var bankID);
            // Debug.Log("Bank ID: " + bankID);
            ServiceLocator.Get<MusicManager>().PlayMainMusic();
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
            }
        }
        
        public void OnPhase1()
        {
            AkUnitySoundEngine.SetRTPCValue("LayerControl", 1f);
        }

        public void PlayMainMusic()
        {
            // AkUnitySoundEngine.PostEvent("Play_MainMenu", gameObject);
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
        
        public void Jump()
        {
            AkUnitySoundEngine.PostEvent("Jump_Player", gameObject);
        }

        public void PlayEndingMusic()
        {
            AkUnitySoundEngine.PostEvent("Stop_BossMusic", gameObject);
            AkUnitySoundEngine.PostEvent("Play_BossDeath_Music", gameObject);
        }
    }
}
