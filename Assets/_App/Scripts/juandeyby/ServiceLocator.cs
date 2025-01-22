using System;
using System.Collections.Generic;
using UnityEngine;

namespace _App.Scripts.juandeyby
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();
        
        public static void Register<T>(T service) where T : class
        {
            if (Services.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"Service {typeof(T)} already registered");
                return;
            }
            Services[typeof(T)] = service;
        }
        
        public static T Get<T>() where T : class
        {
            if (!Services.ContainsKey(typeof(T)))
            {
                Debug.LogError($"Service {typeof(T)} not found");
                return null;
            }
            return Services[typeof(T)] as T;
        }
        
        public static void Unregister<T>() where T : class
        {
            if (!Services.ContainsKey(typeof(T)))
            {
                Debug.LogWarning($"Service {typeof(T)} not found");
                return;
            }
            Services.Remove(typeof(T));
        }
    }
}
