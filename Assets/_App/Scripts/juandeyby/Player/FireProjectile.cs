using System;
using System.Collections;
using _App.Scripts.juandeyby;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Coroutine _destroyCoroutine;

    private void OnEnable()
    {
        _destroyCoroutine = StartCoroutine(DestroyProjectile());
    }

    private IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(3f);
        rb.isKinematic = false;
        rb.useGravity = true;
        
        transform.SetParent(ServiceLocator.Get<FireProjectileManager>().transform);
        ServiceLocator.Get<FireProjectileManager>().ReturnProjectile(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            
            transform.SetParent(other.transform);
        }

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.RangedDamage();
        }
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = false;
            rb.useGravity = true;
            
            transform.SetParent(ServiceLocator.Get<FireProjectileManager>().transform);
            ServiceLocator.Get<FireProjectileManager>().ReturnProjectile(this);
        }
    }
}
