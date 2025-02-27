using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderAbility", menuName = "Scriptable Objects/dataHolderAbility")]

public class dataHolderAbility : ScriptableObject
{
	public string abilityName;
	public bool effectInOverworld;
	public bool effectInBattle;
	public string flavorText;
	public string flavorTextBattle;
}
