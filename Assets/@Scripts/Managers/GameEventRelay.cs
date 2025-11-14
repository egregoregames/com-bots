using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventRelay")]
public class GameEventRelay : ScriptableObject
{
    public Action<SocialyteProfileStaticDatum> ConnectionMade;
    
}
