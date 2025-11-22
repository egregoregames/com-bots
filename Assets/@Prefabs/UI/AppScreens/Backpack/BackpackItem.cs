using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackpackItem : MonoBehaviour
{
    [field: SerializeField]
    private GameObject SelectedIndicator { get; set; }

    [field: SerializeField]
    private Image ImageIcon { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextItemAmount { get; set; }
}
