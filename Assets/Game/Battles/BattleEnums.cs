namespace Game.Battles
{
    public enum AuraType 
    {
        Melee,
        Focus,
        Neutral,
        Guard,
        Heal,
        Tech,
        Beam,
        Blast
    }

    public enum EffectVerb
    {
        Give,
        Remove
    }
   
    

    public enum TargetType
    {
        NoTarget = 0,
        Self = 1,
        OneNonSelfTeammate = 2,
        OneTeammate = 3,
        AllNonSelfTeammates = 4,
        AllTeammates = 5,
        OneOpponent = 6,
        AllOpponents = 7,
        OneNonSelfCombatant = 8,
        OneCombatant = 9,
        AllNonSelfCombatants = 10,
        All = 11
    }

    public enum Priority
    {
        Flee = 0,
        Item = 1,
        Guard = 2,
        PreAttackEffect = 3,
        FirstStrikeAttack = 4,
        NormalAttack = 5,
        LastStrikeAttack = 6
    }

    public enum TurnEffect
    {
        EndBotTurn
    }
    public enum Result
    {
        Nullify,
        Inflict
    }
    public enum TestType
    {
        Damage,
        Condition
    }

    public enum Dissolves
    {
        Naturally,
        ByCure
    }

    public enum DissolutionTime
    {
        EndOfTurn
    }
}