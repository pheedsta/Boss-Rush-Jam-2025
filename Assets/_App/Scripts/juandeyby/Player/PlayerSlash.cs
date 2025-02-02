using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] private ParticleSystem slashEffect;
    
    public void Slash()
    {
        slashEffect.Play();
    }
}
