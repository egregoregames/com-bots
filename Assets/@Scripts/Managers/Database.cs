using System.Collections.Generic;
using ComBots.World.NPCs;
using UnityEngine;

[CreateAssetMenu(menuName = "PersistentData")]
public class Database : ScriptableObject
{
    public List<NPC_Config> Npcs = new ();
}
