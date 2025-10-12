using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderAbility", menuName = "Scriptable Objects/dataHolderAbility")]

public class dataHolderAbility : ScriptableObject
{
	public string abilityName;
	public bool effectInOverworld;
	public bool effectInBattle;
	[TextArea(3, 10)]
	public string flavorText;
	[TextArea(3, 10)]
	public string flavorTextBattle;
}
