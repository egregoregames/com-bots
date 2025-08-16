using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PersistentData")]
public class Database : ScriptableObject
{
    public List<NpcSo> Npcs = new List<NpcSo>();
}
