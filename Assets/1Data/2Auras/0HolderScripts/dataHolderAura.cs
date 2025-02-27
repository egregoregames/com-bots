using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderAura", menuName = "Scriptable Objects/dataHolderAura")]

public class dataHolderAura : ScriptableObject
{
	public string auraName;
	public GameObject icon;
	public string auraColorHex;
	public ScriptableObject advantagedAgainst;
	public ScriptableObject disadvantagedAgainst;
}
