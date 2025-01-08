using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: ColliderRegistry
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-1)]
public class ColliderRegistry : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Static Fields
    //:::::::::::::::::::::::::::::://
    
    private static ColliderRegistry _instance;
    
    //:::::::::::::::::::::::::::::://
    // Properties
    //:::::::::::::::::::::::::::::://
    
    private readonly Dictionary<Component, Collider[]> _registry = new();

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
    // STATIC: Registration
    //------------------------------//
    
    public static void Register(Component component) {
        if (_instance) {
            // there is a ColliderRegistry in the scene; add component and colliders to registry
            _instance._registry.Add(component, component.GetComponentsInChildren<Collider>());
        } else {
            // no ColliderRegistry in the scene; log error
            Debug.Log("ColliderRegistry instance is null");
        }
    }

    public static void Deregister(Component component) {
        // if instance is not null add component to registry (no alert required)
        if (_instance) _instance._registry.Remove(component);
    }
    
    //------------------------------//
    // STATIC: Querying Registry
    //------------------------------//

    public static Component GetColliderParent(Collider childCollider) {
        // if instance hasn't been instantiated OR no collider is passed, we're done
        if (!_instance || !childCollider) return null;
        
        // attempt to find collider parent in registry
        foreach (var keyValuePair in _instance._registry) {
            foreach (var registeredCollider in keyValuePair.Value) {
                if (registeredCollider == childCollider) return keyValuePair.Key;
            }
        }
        
        // collider parent not found; return null
        return null;
    }
}