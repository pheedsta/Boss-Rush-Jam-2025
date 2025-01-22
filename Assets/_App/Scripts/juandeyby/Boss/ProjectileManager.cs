using System;
using System.Collections.Generic;
using UnityEngine;

namespace _App.Scripts.juandeyby.Boss
{
    [DefaultExecutionOrder(-100)]
    public class ProjectileManager : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        private readonly Queue<Projectile> _projectiles = new Queue<Projectile>();
        private readonly int _maxProjectiles = 20;

        private void OnEnable()
        {
            ServiceLocator.Register<ProjectileManager>(this);
        }
        
        private void OnDisable()
        {
            ServiceLocator.Unregister<ProjectileManager>();
        }

        private void Start()
        {
            for (var i = 0; i < _maxProjectiles; i++)
            {
                var projectile = Instantiate(projectilePrefab, transform);
                projectile.gameObject.SetActive(false);
                _projectiles.Enqueue(projectile);
            }
        }
        
        public Projectile GetProjectile()
        {
            if (_projectiles.Count == 0)
            {
                var projectile = Instantiate(projectilePrefab, transform);
                _projectiles.Enqueue(projectile);
            }

            var projectileToReturn = _projectiles.Dequeue();
            projectileToReturn.gameObject.SetActive(true);
            return projectileToReturn;
        }

        public void ReturnProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            _projectiles.Enqueue(projectile);
        }
    }
}
