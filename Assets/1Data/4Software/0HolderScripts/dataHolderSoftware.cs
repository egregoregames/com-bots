using UnityEngine;

public enum targetType
{
	noTarget,
	self,
	oneNonselfTeammate,
	oneTeammate,
	allNonselfTeammates,
	allTeammates,
	oneOpponent,
	allOpponents,
	oneNonselfCombatant,
	oneCombatant,
	allNonselfCombatants,
	allCombatants
}

public enum priority
{
	flee,
	//item
	guard,
	preAttackEffect,
	firstStrikeAttack,
	normalAttack,
	lastStrikeAttack
}

[CreateAssetMenu(fileName = "dataHolderSoftware", menuName = "Scriptable Objects/dataHolderSoftware")]

public class dataHolderSoftware : ScriptableObject
{
	public string softwareName;
	public GameObject image; //just graphic? not model?
	public ScriptableObject aura;
	public int ramCost;
	public int omegaEnergyCost;
	public int energyCost;
	public int power;
	public int recoilPercent;
	public bool ignoresDrawFire;
	public bool hitsThroughGuard;
	public bool neverMisses;
	public int accuracy;
	public int criticalRate;
	public priority priority; //should be enumerator
	public targetType targetType; //should be enumerator
	public int minHits;
	public int maxHits;
	public string flavorText;

	//Chances of inflicting condition on target(s)

		public bool curesTargetRust;
		public bool curesTargetOverload;
		public bool curesTargetFreeze;		
		public int targetRustChance;
		public int targetOverloadChance;
		public int targetFreezeChance;
		public int targetShockChance;
		public int targetTrappedChance;
		public int targetDrawFireChance;

	//Chances of modifying targets' stats

		public int targetSpeedChangeChance;
		public int targetSpeedChangeVector;
	
		public int targetAttackMeleeChangeChance;
		public int targetAttackMeleeChangeVector;
		public int targetAttackBeamChangeChance;
		public int targetAttackBeamChangeVector;
		public int targetAttackBlastChangeChance;
		public int targetAttackBlastChangeVector;

		public int targetDefenseMeleeChangeChance;
		public int targetDefenseMeleeChangeVector;
		public int targetDefenseBeamChangeChance;
		public int targetDefenseBeamChangeVector;
		public int targetDefenseBlastChangeChance;
		public int targetrDefenseBlastChangeVector;

		public int targetAccuracyChangeChance;
		public int targetAccuracyChangeVector;
		public int targetEvasivenessChangeChance;
		public int targetEvasivenessChangeVector;

	//Chances of inflicting condition on user

		public bool curesUserRust;
		public bool curesUserOverload;
		public bool curesUserFreeze;		
		public int userRustChance;
		public int userOverloadChance;
		public int userFreezeChance;
		public int userShockChance;
		public int userTrappedChance;
		public int userDrawFireChance;

	//Chances of modifying user's stats

		public int userSpeedChangeChance;
		public int userSpeedChangeVector;
	
		public int userAttackMeleeChangeChance;
		public int userAttackMeleeChangeVector;
		public int userAttackBeamChangeChance;
		public int userAttackBeamChangeVector;
		public int userAttackBlastChangeChance;
		public int userAttackBlastChangeVector;

		public int userDefenseMeleeChangeChance;
		public int userDefenseMeleeChangeVector;
		public int userDefenseBeamChangeChance;
		public int userDefenseBeamChangeVector;
		public int useDefenseBlastChangeChance;
		public int userDefenseBlastChangeVector;

		public int userAccuracyChangeChance;
		public int userAccuracyChangeVector;
		public int userEvasivenessChangeChance;
		public int userEvasivenessChangeVector;

	public bool atypicalEffects;
	public bool punchingSoftware;
	public bool uplinkGloveCompatible;
}
