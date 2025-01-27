using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private Boss boss;
        
        private void Start()
        {
            boss.SetState(new BossPortalSummonState());
        }
    }
}
