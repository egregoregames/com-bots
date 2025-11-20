using UnityEngine;

[CreateAssetMenu(
    fileName = "StaticSolexDatum",
    menuName = "Scriptable Objects/Static Solex Datum")]
public class StaticSolexDatum : ScriptableObject
{
    [field: SerializeField]
    public int Id { get; private set; }

    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public Sprite SpriteIconSocialyte { get; private set; }

    [field: SerializeField]
    public Sprite SpriteIconBackpack { get; private set; }

    [field: SerializeField]
    public int StartingBlueprintId { get; private set; }
}