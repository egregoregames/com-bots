using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "dataHolderSocialyteProfile", 
	menuName = "Scriptable Objects/dataHolderSocialyteProfile")]
public class dataHolderSocialyteProfile : ScriptableObject
{
	[field: SerializeField]
	public int ProfileId { get; private set; }

	[field: SerializeField, FormerlySerializedAs("profileName")]
	public string ProfileName { get; private set; }

	[field: SerializeField, FormerlySerializedAs("modelPortrait")]
	public GameObject ModelPortrait { get; private set; }

    [field: SerializeField, FormerlySerializedAs("modelOverworld")]
    public GameObject ModelOverworld { get; private set; }

	[field: SerializeField, FormerlySerializedAs("potentialTeammate"), 
		Tooltip("Whether this NPC can also be a teammate.")]
	public bool IsPotentialTeammate { get; private set; }

	[field: SerializeField, FormerlySerializedAs("occupation")]
	public string Occupation { get; private set; }

	[field: SerializeField, FormerlySerializedAs("origin")]
    public string Origin { get; private set; }

	[field: SerializeField, FormerlySerializedAs("bio"), TextArea(3, 10)]
	public string Bio { get; private set; }
}
