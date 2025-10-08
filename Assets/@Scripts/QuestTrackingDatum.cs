public class QuestTrackingDatum
{
    public string QuestId { get; set; }
    public bool IsActive { get; set; }
    public int CurrentStep { get; set; } = 0;
    public bool IsCompleted => CurrentStep == 100;

    public void Complete()
    {
        CurrentStep = 100;
        IsActive = false;
    }
}