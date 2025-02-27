using System;
using DependencyInjection;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class ColliderProvider : MonoBehaviour, IDependencyProvider
    {
        public BoxCollider collider;
        
        [Provide]
        public BoxCollider ProvideCollider()
        {
            return collider;
        }
    }
}
