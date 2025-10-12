using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderMeister", menuName = "Scriptable Objects/dataHolderMeister")]

public class dataHolderMeister : ScriptableObject
{
	public string meisterName;
	public GameObject modelPortrait;
	public GameObject modelOverworld;
	public GameObject modelPortraitAlt;
	public GameObject modelOverworldAlt;
	public bool generic;
	public bool potentialTeammate;
	public ScriptableObject Bot1;
	public ScriptableObject Bot2;
	public ScriptableObject Bot3;
}
