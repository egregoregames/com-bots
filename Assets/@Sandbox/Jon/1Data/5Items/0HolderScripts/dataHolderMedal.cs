using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "dataHolderMedal", menuName = "Scriptable Objects/dataHolderMedal")]

public class dataHolderMedal : ScriptableObject
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
