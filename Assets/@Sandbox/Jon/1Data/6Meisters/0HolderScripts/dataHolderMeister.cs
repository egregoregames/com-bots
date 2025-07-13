using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderMeister", menuName = "Scriptable Objects/dataHolderMeister")]

public class dataHolderMeister : ScriptableObject
{
	public string meisterName;
	public GameObject imagePortrait;
	public GameObject imageOverworld;
	public GameObject imagePortraitAlt;
	public GameObject imageOverworldAlt;
	public GameObject imageSolex;
	public bool generic;
	public bool potentialTeammate;
	public int bond;
	public ScriptableObject Bot1;
	public ScriptableObject Bot2;
	public ScriptableObject Bot3;
}
