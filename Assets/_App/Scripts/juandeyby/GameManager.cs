using System;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class GameManager : MonoBehaviour
    {
        private GamePhase _gamePhase;
        public event Action<GamePhase> OnGamePhaseChanged;
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<GameManager>();
        }

        private void Awake()
        {
            // Start the game in phase 1
            _gamePhase = GamePhase.Phase1;
            Pause();
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }
        
        public void Resume()
        {
            Time.timeScale = 1;
        }
        
        public GamePhase GetGamePhase()
        {
            return _gamePhase;
        }
        
        public void SetGamePhase(GamePhase gamePhase)
        {
            _gamePhase = gamePhase;
            OnGamePhaseChanged?.Invoke(_gamePhase);
        }
    }
    
    public enum GamePhase
    {
        Phase1, // The game starts in 3th ring
        Phase2, // The game starts in 2nd ring
        Phase3 // The game starts in 1st ring
    }
}

