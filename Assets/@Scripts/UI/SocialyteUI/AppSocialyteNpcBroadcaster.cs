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

    public static void BroadcastNpc(SocialyteProfileStaticDatum datum)
    {
        Instance.NpcBroadcasterData
            .ForEach(data => data.Instance.SetActive(false));

        var existing = Instance.NpcBroadcasterData
            .FirstOrDefault(x => x.NpcId == datum.ProfileId);

        if (existing == null)
        {
            if (datum.ModelPortrait == null)
            {
                Debug.LogWarning($"No model profile set for npc {datum.ProfileName}");
                return;
            }
            var instance = Instantiate(datum.ModelPortrait, Instance.transform);
            var components = instance.GetComponentsInChildren<Component>(true);

            Array.ForEach(components, component => 
            {
                var type = component.GetType();
                Debug.Log(type.ToString());

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

            //Vector3 cameraAt0 = Instance.Camera.transform.localPosition;
            //cameraAt0.y = 0;

            //instance.transform.LookAt(Instance.transform.TransformPoint(cameraAt0));

            existing = new NpcBroadcasterDatum()
            {
                Instance = instance,
                NpcId = datum.ProfileId
            };

            Instance.NpcBroadcasterData.Add(existing);
        }

        existing.Instance.SetActive(true);
    }
}
