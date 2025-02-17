using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private Boss boss;
        
        [Header("ArialAttack")]
        private float _arialCurrentTime = 25f;
        private readonly float _arialMaxTime = 30f;
        
        [Header("VortexAttack")]
        private float _vortexCurrentTime = 25f;
        private readonly float _vortexMaxTime = 30f;
        
        private void Start()
        {
            boss.SetState(new BossPortalSummonState());
        }

        private void Update()
        {
            AerialAttack();

            VortexAttack();
        }

        private void VortexAttack()
        {
            if (ServiceLocator.Get<GameManager>().GetGamePhase() == GamePhase.Phase2)
            {
                if (_vortexCurrentTime >= _vortexMaxTime)
                {
                    _vortexCurrentTime = 0;
                    boss.SetState(new BossVortexPullState());
                }
                else
                {
                    _vortexCurrentTime += Time.deltaTime;
                }
            }
        }

        private void AerialAttack()
        {
            if (ServiceLocator.Get<GameManager>().GetGamePhase() == GamePhase.Phase3)
            {
                if (_arialCurrentTime >= _arialMaxTime)
                {
                    _arialCurrentTime = 0;
                    boss.SetState(new BossAerialBarrageState());
                }
                else
                {
                    _arialCurrentTime += Time.deltaTime;
                }
            }
        }
    }
}
