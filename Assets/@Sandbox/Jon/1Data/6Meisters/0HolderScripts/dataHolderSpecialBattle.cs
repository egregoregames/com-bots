using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderSpecialBattle", menuName = "Scriptable Objects/dataHolderSpecialBattle")]

public class dataHolderSpecialBattle : ScriptableObject
{
	public string meisterName;
	public GameObject modelPortrait;
	public GameObject modelOverworld;
	public int rank;
	public ScriptableObject formBasic;
	public ScriptableObject formCombat;
	public ScriptableObject formAwakened;
	public ScriptableObject abilityActive;
	//Software loadout
	//Hardware loadout
	//Item inventory
	[TextArea(3, 10)]	
	public string description;
}