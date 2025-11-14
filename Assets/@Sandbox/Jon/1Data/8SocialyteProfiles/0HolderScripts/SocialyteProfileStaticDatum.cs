using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Contains immutable data for individual entires in the Socialyte App in the 
/// pause menu. Accessible via <see cref="StaticGameData.SocialyteData"/>
/// </summary>
[CreateAssetMenu(fileName = "SocialyteProfileStaticDatum", 
	menuName = "Scriptable Objects/Socialyte Profile Static Datum")]
public class SocialyteProfileStaticDatum : ScriptableObject
{
	/// <summary>
	/// Must be unique
	/// </summary>
	[field: SerializeField, Tooltip("Must be unique")]
	public int ProfileId { get; private set; }

	[field: SerializeField, FormerlySerializedAs("profileName")]
	public string ProfileName { get; private set; }

	[field: SerializeField, FormerlySerializedAs("modelPortrait")]
	public GameObject ModelPortrait { get; private set; }

    [field: SerializeField, FormerlySerializedAs("modelOverworld")]
    public GameObject ModelOverworld { get; private set; }

    /// <summary>
    /// Whether this NPC can also be a teammate
    /// </summary>
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
