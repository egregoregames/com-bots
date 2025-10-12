using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderBotBasic", menuName = "Scriptable Objects/dataHolderBotBasic")]

public class dataHolderBotBasic : ScriptableObject
{
	public string botName;
	public GameObject modelAlly;
	public GameObject modelFoe;
	public GameObject blueprintImage;
	public ScriptableObject combatForm1;
	public ScriptableObject combatForm2;
	public ScriptableObject combatForm3;
	public ScriptableObject ability1;
	public ScriptableObject ability2;
	public ScriptableObject ability3;
	public int weightBase;
	public int ram;
	public int load;
	public int attackMeleeBase;
	public int attackBeamBase;
	public int attackBlastBase;
	public int defenseMeleeBase;
	public int defenseBeamBase;
	public int defenseBlastBase;
	public int speedBase;
	public int EnduranceBase;
	public int energyBase;
	public ScriptableObject softwareSignature;
	public ScriptableObject moveset;
	public ScriptableObject drops;
}
