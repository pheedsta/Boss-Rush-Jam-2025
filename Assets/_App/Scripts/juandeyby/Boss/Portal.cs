using System;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    public class Portal : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 2f);
        }
    }
}
