using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class NpcConnectionDatum
{
    [field: SerializeField]
    public int NpcId { get; set; }

    [field: SerializeField]
    public bool HasNewUpdates { get; set; } = true;

    [field: SerializeField]
    public int CurrentBioStep { get; set; }

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
