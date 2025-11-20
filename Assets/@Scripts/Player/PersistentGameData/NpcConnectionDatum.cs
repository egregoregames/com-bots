using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Contains persistent data about an NPC connection that appears in the 
/// Socialyte App
/// </summary>
[Serializable]
public class NpcConnectionDatum
{
    [field: SerializeField]
    public int NpcId { get; set; }

    /// <summary>
    /// If true, will display a "new updates" indicator for this connection in 
    /// the Socialyte App
    /// </summary>
    [field: SerializeField]
    public bool HasNewUpdates { get; set; } = true;

    /// <summary>
    /// If true, this NPC connection is visible in the Socialyte App
    /// </summary>
    [field: SerializeField]
    public bool IsVisible { get; set; }

    /// <summary>
    /// Is incremented to show a different bio for this NPC depending on 
    /// quest or relationship progression (or some other game event)
    /// </summary>
    [field: SerializeField]
    public int CurrentBioStep { get; set; }

    /// <summary>
    /// Is incremented to show a different check-in location for this NPC 
    /// depending on quest or relationship progression (or some other game 
    /// event)
    /// </summary>
    [field: SerializeField]
    public int CurrentCheckInLocationStep { get; set; }

    public async Task<SocialyteProfileStaticDatum> GetStaticDataAsync()
    {
        return (await StaticGameData.GetInstanceAsync()).SocialyteData
            .First(x => x.ProfileId == NpcId);
    }

    /// <summary>
    /// Make sure <see cref="StaticGameData.Instance"/> is not null or 
    /// use <see cref="GetStaticData"/>
    /// </summary>
    /// <returns></returns>
    public SocialyteProfileStaticDatum GetStaticData()
    {
        return StaticGameData.Instance.SocialyteData
            .First(x => x.ProfileId == NpcId);
    }
}
