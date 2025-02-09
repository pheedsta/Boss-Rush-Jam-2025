using UnityEngine;

public class BossSlash : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashEffect;
    
    public void Slash()
    {
        slashEffect.Play();
    }
}
