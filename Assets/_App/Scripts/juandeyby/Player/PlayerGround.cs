using _App.Scripts.juandeyby;
using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    [SerializeField] private Ring ring;
    [SerializeField] private LayerMask layerMask;
    
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * 2f, Color.red);
        if (Physics.Raycast(transform.position,
                Vector3.down, out var hit, 2f, layerMask))
        {
            switch (hit.transform.tag)
            {
                case "A":
                    ring = Ring.RingA;
                    break;
                case "B":
                    ring = Ring.RingB;
                    break;
                case "C":
                    ring = Ring.RingC;
                    break;
            }
        }
    }
}
