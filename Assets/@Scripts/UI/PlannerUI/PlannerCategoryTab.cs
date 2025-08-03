namespace ComBots.UI.src.PlannerUI
{
    public class PlannerCategoryTab : CategoryTab
    {
        const string PlannerSelectedColorString = "#F7951F";
        
        protected override void Awake()
        {
            SelectedColorString = PlannerSelectedColorString;
            base.Awake();
        }
    }
}
