using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private Boss boss;

        private void Start()
        {
            boss.SetState(new BossAerialBarrageState());
        }
    }
}
