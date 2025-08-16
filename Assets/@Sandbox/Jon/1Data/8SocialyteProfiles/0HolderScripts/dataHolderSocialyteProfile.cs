using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderSocialyteProfile", menuName = "Scriptable Objects/dataHolderSocialyteProfile")]

public class dataHolderSocialyteProfile : ScriptableObject
{
	public string profileName;
	public GameObject imagePortrait;
	public GameObject imageOverworld;
	public bool potentialTeammate;
	public int bond;
	public string occupation;
	public string origin;
	public string bio;
	public ScriptableObject location;
	public bool currentConnection;
	public bool newConnection;
	public bool inParty;
}
