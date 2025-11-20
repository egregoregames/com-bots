using System;
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

	[field: SerializeField, FormerlySerializedAs("bio"), TextArea(3, 10), Obsolete]
	public string Bio { get; private set; }

    /// <summary>
    /// Allows a different bio to be shown at each step of the relationship/quest
	/// with this NPC. See <see cref="NpcConnectionDatum.CurrentBioStep"/>
    /// </summary>
    [field: SerializeField, TextArea(3, 10)]
	public string[] Bios { get; private set; }

    /// <summary>
    /// Allows different check-in locations to be shown at each step of the quest
	/// with this NPC or from other gameplay events. See 
	/// <see cref="NpcConnectionDatum.CurrentCheckInLocationStep"/>
    /// </summary>
    [field: SerializeField]
	public string[] CheckInLocations { get; private set; }
}
