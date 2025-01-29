using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public class Arena : MonoBehaviour
    {
        [SerializeField] NavMeshSurface wormNavMeshSurface;
        private Coroutine _refreshNavMeshCoroutine;
        
        private void Start()
        {
            _refreshNavMeshCoroutine = StartCoroutine(RefreshNavMesh());
        }

        private IEnumerator RefreshNavMesh()
        {
            while (true)
            {
                wormNavMeshSurface.BuildNavMesh();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
