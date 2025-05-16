namespace Game.UI.src.BackpackUI
{
    public class BackpackCategoryTab : CategoryTab
    {
        const string BackpackSelectedColorString = "#FAC832";
        
        protected override void Awake()
        {
            SelectedColorString = BackpackSelectedColorString;
            base.Awake();
        }
    }
}
