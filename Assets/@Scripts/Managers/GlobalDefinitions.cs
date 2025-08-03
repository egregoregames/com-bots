using ComBots.Battles.Stats;

namespace ComBots.Managers
{
    public class GlobalDefinitions : Singleton<GlobalDefinitions>
    {
        public VitalDefinition EnduranceVital;
        public VitalDefinition EnergyVital;

        public StatDefinition Accuracy;
        public StatDefinition BeamAttack;
        public StatDefinition BeamDefense;
        public StatDefinition BlasAttack;
        public StatDefinition BlastDefense;
        public StatDefinition Evasiveness;
        public StatDefinition MeleeAttack;
        public StatDefinition MeleeDefense;
        public StatDefinition Speed;
    }
}