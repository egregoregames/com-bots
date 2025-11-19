using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppSocialyte : PauseMenuAppSingleton<AppSocialyte>
{
    [field: SerializeField]
    private GameObject ItemTemplate { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextBio { get; set; }

    [field: SerializeField]
    private TextMeshProUGUI TextContactName { get; set; }

    [field: SerializeField]
    private GameObject[] BondHearts { get; set; }

    [field: SerializeField]
    private PauseMenuAppScrollList<NpcConnectionDatum> ScrollList { get; set; }

    private SocialyteTab CurrentTab { get; set; }
}

public enum SocialyteTab
{
    Connections,
    Feed
}