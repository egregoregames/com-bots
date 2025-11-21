using R3.Triggers;
using System;
using UnityEngine;
using UnityEngine.UI;
using R3;
using UnityEngine.EventSystems;

public class PauseMenu_AppButton : MonoBehaviourR3
{
    private static UnityEventR3<PauseMenu_AppButton> _onButtonSelected = new();
    public static IDisposable OnSelected(Action<PauseMenu_AppButton> x) 
        => _onButtonSelected.Subscribe(x);

    [field: SerializeField]
    private Button Button { get; set; }

    [field: SerializeField]
    public string DescriptionBoxTitle { get; set; }

    [field: SerializeField, TextArea]
    public string DescriptionBoxText { get; set; }

    [field: SerializeField]
    public Gradient DescriptionBoxBackgroundGradient { get; set; }

    [field: SerializeField]
    public Sprite Icon { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        var onSelected = Button
            .OnSelectAsObservable()
            .Subscribe(_ => _onButtonSelected.Invoke(this));

        AddEvents(
            Inputs.UI_Submit(_ => OnSubmit()),
            onSelected);
    }

    private void OnSubmit()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            return;
        }

        if (!PauseMenu.Instance.IsOpen)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject == Button.gameObject)
        {
            Button.onClick.Invoke();
        }
    }
}
