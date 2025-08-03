namespace ComBots.UI.src.BackpackUI
{
    public class BackpackCategoryTab : CategoryTab
    {
        const string BackpackSelectedColorString = "#FAC832";
        
        protected override void Awake()
        {
            SelectedColorString = BackpackSelectedColorString;
            base.Awake();
        }

        public override void SelectEffect()
        {
            isSelected = true;            
            base.SelectEffect();
        }

        public override void DeselectEffect()
        {
            if (isSelected) return;
            base.DeselectEffect();
        }
    }
}
