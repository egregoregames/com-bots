using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(
	fileName = "StaticMedalDatum", 
	menuName = "Scriptable Objects/Static Medal Datum")]
public class StaticMedalDatum : ScriptableObject
{
	[field: SerializeField]
    public int Id { get; private set; }

	[field: SerializeField, FormerlySerializedAs("medalName")]
    public string Name { get; private set; }

	[field: SerializeField, FormerlySerializedAs("flavorText"), TextArea(3, 10)]
	public string FlavorText { get; set; }

	[field: SerializeField, FormerlySerializedAs("effectText"), TextArea(3, 10)]
	public string EffectText { get; set; }

	[field: SerializeField, FormerlySerializedAs("image")]
    public Texture2D Image { get; set; }
}