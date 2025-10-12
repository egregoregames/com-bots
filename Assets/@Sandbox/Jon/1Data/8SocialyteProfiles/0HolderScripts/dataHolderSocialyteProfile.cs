using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderSocialyteProfile", menuName = "Scriptable Objects/dataHolderSocialyteProfile")]

public class dataHolderSocialyteProfile : ScriptableObject
{
	public string profileName;
	public GameObject modelPortrait;
	public GameObject modelOverworld;
	[Tooltip("Whether this NPC can also be a teammate.")]
	public bool potentialTeammate;
	public string occupation;
	public string origin;
	[TextArea(3, 10)]
	public string bio;
}
