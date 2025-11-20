using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles broadcasting a camera feed of 3D models to the Socialyte App
/// </summary>
public class AppSocialyteNpcBroadcaster : MonoProtectedSingletonR3<AppSocialyteNpcBroadcaster>
{
    [Serializable]
    public class NpcBroadcasterDatum
    {
        [field: SerializeField, ReadOnly]
        public int NpcId { get; set; }

        [field: SerializeField, ReadOnly]
        public GameObject Instance { get; set; }
    }

    [field: SerializeField] 
    private Transform Camera { get; set; }

    [field: SerializeField]
    private List<NpcBroadcasterDatum> NpcBroadcasterData { get; set; } = new();

    protected override void Awake()
    {
        base.Awake();
        Close();
    }

    protected override void Initialize()
    {
        base.Initialize();

        AddEvents(
            AppSocialyte.OnClosed(Close));
    }

    public static void BroadcastNpc(SocialyteProfileStaticDatum datum)
    {
        Instance.gameObject.SetActive(true);

        Instance.NpcBroadcasterData
            .ForEach(data => data.Instance.SetActive(false));

        var existing = Instance.NpcBroadcasterData
            .FirstOrDefault(x => x.NpcId == datum.ProfileId);

        existing ??= AddInstance(datum);

        if (existing == null)
            return;

        existing.Instance.SetActive(true);
    }

    private static NpcBroadcasterDatum AddInstance(SocialyteProfileStaticDatum datum)
    {
        if (datum.ModelPortrait == null)
        {
            Debug.LogWarning($"No model profile set for npc {datum.ProfileId} - {datum.ProfileName}");
            return null;
        }

        var instance = Instantiate(datum.ModelPortrait, Instance.transform);
        RemoveUnneededComponents(instance);

        instance.transform.localPosition = Vector3.zero;

        var newDatum = new NpcBroadcasterDatum()
        {
            Instance = instance,
            NpcId = datum.ProfileId
        };

        Instance.NpcBroadcasterData.Add(newDatum);
        return newDatum;
    }

    private static void RemoveUnneededComponents(GameObject instance)
    {
        var components = instance.GetComponentsInChildren<Component>(true);
        Array.ForEach(components, component =>
        {
            var type = component.GetType();
            if (type != typeof(Transform)
                && type != typeof(SkinnedMeshRenderer)
                && type != typeof(Animator))
            {
                Destroy(component);
            }
            else
            {
                component.gameObject.layer = LayerMask.NameToLayer("SocialyteProfile");
            }
        });
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
