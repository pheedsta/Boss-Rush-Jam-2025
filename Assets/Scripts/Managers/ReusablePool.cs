using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: ReusablePool
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-2)]
public class ReusablePool : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Static Fields
    //:::::::::::::::::::::::::::::://

    private static ReusablePool _instance;
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Dictionary<string, Queue> _registry = new();
    
    //:::::::::::::::::::::::::::::://
    // Unity Callbacks
    //:::::::::::::::::::::::::::::://
    
    private void Awake() {
        if (!_instance) {
            _instance = this;
        } else {
            Destroy(this);
        }
    }

    private void OnDisable() {
        _registry.Clear();
    }

    //------------------------------//
    // Fetch / Return
    //------------------------------//
    
    public static T FetchReusable<T>(T original, Vector3 position, Quaternion rotation, Transform parent = null) where T : MonoBehaviour, IReusable {
        // if instance not made yet OR no original passed; we're done
        if (!_instance || !original) return null;
            
        if (_instance._registry.TryGetValue(FullReusableIdentifier(original), out var queue) && 0 < queue.Count && queue.Peek() is T) {
            // pool dictionary contains queue with this identifier AND queue is populated
            // dequeue the first object (NB: we can explicitly cast here because we have already checked its type above)
            var reusable = (T)queue.Dequeue();

            // set position, rotation, parent and active
            reusable.transform.SetPositionAndRotation(position, rotation);
            reusable.transform.SetParent(parent, true);
            reusable.gameObject.SetActive(true);
            
            // return dequeued reusable
            return reusable;
        }
        
        // there is no ReusablePool OR there are no objects with this identifier available; return new instantiation
        return Instantiate(original, position, rotation, parent);
    }

    public static void ReturnReusable<T>(T reusable) where T : MonoBehaviour, IReusable {
        //  if instance OR reusable is null we are done (no alert required)    
        if (!_instance || !reusable) return;

        // get full identifier of reusable
        var identifier = FullReusableIdentifier(reusable);

        // deactivate returned reusable
        reusable.gameObject.SetActive(false);
        
        if (_instance._registry.TryGetValue(identifier, out var queue)) {
            // pool dictionary contains queue for this key; enqueue reusable
            queue.Enqueue(reusable);
        } else {
            // pool dictionary does not contain queue for this key; instantiate new Queue
            Queue newQueue = new();

            // enqueue reusable
            newQueue.Enqueue(reusable);

            // add new queue to dictionary
            _instance._registry.Add(identifier, newQueue);
        }
    }
    
    //:::::::::::::::::::::::::::::://
    // Utilities
    //:::::::::::::::::::::::::::::://
    
    private static string FullReusableIdentifier<T>(T reusable) where T : IReusable {
        return $"{reusable.GetType()}:{reusable.Identifier}";
    }
}