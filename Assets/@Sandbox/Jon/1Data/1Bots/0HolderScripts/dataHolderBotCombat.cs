using UnityEngine;

[CreateAssetMenu(fileName = "dataHolderBotCombat", menuName = "Scriptable Objects/dataHolderBotCombat")]

public class dataHolderBotCombat : ScriptableObject
{
	public string botName;
	public GameObject modelAlly;
	public GameObject modelFoe;
	public ScriptableObject awakenedForm;
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
	public ScriptableObject moveset;
	public ScriptableObject drops;
}
