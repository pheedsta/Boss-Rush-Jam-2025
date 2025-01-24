using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private Boss boss;
        
        Coroutine _sweepStrikePhaseCoroutine;

        private void Start()
        {
            boss.SetState(new BossWanderState());
            _sweepStrikePhaseCoroutine = StartCoroutine(SweepStrikePhase());
        }
        
        private IEnumerator SweepStrikePhase()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                boss.SetState(new BossSweepingStrikeState());
                yield return new WaitForSeconds(5f);
                boss.SetState(new BossAerialBarrageState());
                yield return new WaitForSeconds(5f);
                boss.SetState(new BossPortalSummonState());
                yield return new WaitForSeconds(5f);
                boss.SetState(new BossVortexPullState());
            }

            // while (true)
            // {
            //     yield return new WaitForSeconds(30f);
            //     boss.SetState(new BossSweepingStrikeState());
            // }
        }
        
        public void SpectralWarnerPhase()
        {
            boss.SetState(new BossPortalSummonState());
        }

        private void OnDestroy()
        {
            if (_sweepStrikePhaseCoroutine != null)
            {
                StopCoroutine(_sweepStrikePhaseCoroutine);
            }
        }
    }
}
