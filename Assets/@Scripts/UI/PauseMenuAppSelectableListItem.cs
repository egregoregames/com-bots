using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PauseMenuAppSelectableListItem<T> : MonoBehaviourR3
{
    private static UnityEventR3<T> _onSelected = new();
    public static IDisposable OnSelected(Action<T> x) => _onSelected.Subscribe(x);

    [field: SerializeField]
    protected TextMeshProUGUI TextMain { get; set; }

    [field: SerializeField]
    private GameObject SelectedIndent { get; set; }

    [field: SerializeField]
    private GameObject BackgroundSelected { get; set; }

    /// <summary>
    /// Check for null or use <see cref="GetDatumAsync"/>
    /// </summary>
    public T Datum { get; private set; }

    public bool IsSelected { get; private set; }

    public virtual void Select()
    {
        IsSelected = true;
        BackgroundSelected.SetActive(true);
        SelectedIndent.SetActive(true);
        _onSelected?.Invoke(Datum);
    }

    public virtual void Deselect()
    {
        IsSelected = false;
        BackgroundSelected.SetActive(false);
        SelectedIndent.SetActive(false);
    }

    /// <summary>
    /// Should only be called once immediately after instantiation
    /// </summary>
    /// <param name="value"></param>
    public virtual async Task SetDatum(T value)
    {
        Datum = value;
    }

    public async Task<T> GetDatumAsync()
    {
        while (Datum == null)
        {
            await Task.Yield();
            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        return Datum;
    }
}