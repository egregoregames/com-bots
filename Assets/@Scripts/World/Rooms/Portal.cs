using ComBots.Game.Players;
using ComBots.Logs;
using UnityEngine;

namespace ComBots.Game.Portals
{
    public class Portal : MonoBehaviour
    {
        public Player Player { get; protected set; }
        [SerializeField] private Transform portalExit;
        protected virtual void OnPlayerEnter(Player player) { }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Player.TAG_TRIGGER))
            {
                Player = other.GetComponentInParent<Player>();
                OnPlayerEnter(Player);
            }
        }

        public void TeleportPlayer(Player player)
        {
            //player.transform.position =  transform.position + (this.player.transform.forward * 1.5f);
            player.Controller.TeleportTo(portalExit.position, portalExit.rotation);
        }
    }
}