using System.Collections.Generic;
using UnityEngine;

//++++++++++++++++++++++++++++++//
// CLASS: ComponentRegistry
//++++++++++++++++++++++++++++++//

[DefaultExecutionOrder(-2)]
public class ComponentRegistry : MonoBehaviour {
    
    //:::::::::::::::::::::::::::::://
    // Static Fields
    //:::::::::::::::::::::::::::::://
    
    private static ComponentRegistry _instance;
    
    //:::::::::::::::::::::::::::::://
    // Readonly Fields
    //:::::::::::::::::::::::::::::://
    
    private readonly Dictionary<Component, Collider[]> _components = new();

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
    
    //------------------------------//
    // Registration
    //------------------------------//
    
    public static void Register(Component component) {
        if (!_instance) return;
        
        // get an array of all the colliders linked to this component and try adding it to the dictionary
        // NB: TryAdd doesn't return an error if component is already in the dictionary (unlike Add)
        _instance._components.TryAdd(component, component.GetComponentsInChildren<Collider>());
    }

    public static void Deregister(Component component) {
        // if instance is not null; remove the passed component from the dictionary
        if (_instance) _instance._components.Remove(component);
    }
    
    //------------------------------//
    // Fetch Methods
    //------------------------------//

    public static T[] Components<T>() where T : Component {
        // initialise list to hold all components
        var components = new List<T>();

        // if instance is null we're done
        if (!_instance) return components.ToArray();
        
        // cycle through Dictionary and get all components matching passed type
        foreach (var keyValuePair in _instance._components) {
            if (keyValuePair.Key is T component) components.Add(component);
        }
        
        // return components (as an array)
        return components.ToArray();
    }

    public static T[] ColliderComponents<T>(Collider collider) where T : Component {
        // initialise list to hold all components of type
        var components = new List<T>();

        // if instance is null we're done
        if (!_instance) return components.ToArray();
        
        // cycle through Dictionary
        foreach (var keyValuePair in _instance._components) {
            // if the component does not match the type; move on
            if (keyValuePair.Key is not T component) continue;
                
            // cycle through colliders linked to this component
            foreach (var kvpCollider in keyValuePair.Value) {
                // if this collider doesn't match, move on
                if (collider != kvpCollider) continue;
                    
                // this collider does match AND the component type is correct
                // add component to list and break from inner loop
                components.Add(component);
                break;
            }
        }

        // return components (as an array)
        return components.ToArray();
    }

    public static T ColliderComponent<T>(Collider collider) where T : Component {
        var components = ColliderComponents<T>(collider);
        return 0 < components.Length ? components[0] : null;
    }

    public static Component[] ColliderComponents(Collider collider) {
        return ColliderComponents<Component>(collider);
    }
}
