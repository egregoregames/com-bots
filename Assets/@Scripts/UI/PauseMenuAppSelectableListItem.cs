using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PauseMenuAppSelectableListItem<T> : PauseMenuAppSelectableItem<T>
{
    [field: SerializeField]
    protected TextMeshProUGUI TextMain { get; set; }

    [field: SerializeField]
    private GameObject SelectedIndent { get; set; }

    [field: SerializeField]
    private GameObject BackgroundSelected { get; set; }

    public new virtual void Select()
    {
        base.Select();
        BackgroundSelected.SetActive(true);
        SelectedIndent.SetActive(true);
    }

    public new virtual void Deselect()
    {
        base.Deselect();
        BackgroundSelected.SetActive(false);
        SelectedIndent.SetActive(false);
    }
}