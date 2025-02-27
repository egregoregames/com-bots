using System;
using UnityEngine;

namespace Game.src
{
    public class Relay : MonoBehaviour
    {
        [SerializeField] InputSO inputSo;
        [SerializeField] UISo uiSo;

        
        private void Awake()
        {
            uiSo.OnRoomTransition += OnRoomTransition;
        }

        public void OnRoomTransition(Room room)
        {
            
        }
    }
}
