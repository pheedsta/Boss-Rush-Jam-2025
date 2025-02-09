using System;
using System.Collections.Generic;
using _App.Scripts.juandeyby;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class FireProjectileManager : MonoBehaviour
{
    [SerializeField] private FireProjectile projectilePrefab;    
    private readonly Queue<FireProjectile> _projectiles = new Queue<FireProjectile>();
    private readonly int _maxProjectiles = 5;

    private void OnEnable()
    {
        ServiceLocator.Register<FireProjectileManager>(this);
    }
    
    private void OnDisable()
    {
        ServiceLocator.Unregister<FireProjectileManager>();
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
    
    public FireProjectile GetProjectile()
    {
        if (_projectiles.Count == 0)
        {
            return null;
        }
        var projectile = _projectiles.Dequeue();
        projectile.gameObject.SetActive(true);
        return projectile;
    }
    
    public void ReturnProjectile(FireProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _projectiles.Enqueue(projectile);
    }
}
