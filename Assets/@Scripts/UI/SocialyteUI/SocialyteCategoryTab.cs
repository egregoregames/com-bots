namespace Game.UI.src.SocialyteUI
{
    public class SocialyteCategoryTab : CategoryTab
    {
        const string SocialyteSelectedColorString = "#F24C52";
        
        protected override void Awake()
        {
            SelectedColorString = SocialyteSelectedColorString;
            base.Awake();
        }
    }
}
