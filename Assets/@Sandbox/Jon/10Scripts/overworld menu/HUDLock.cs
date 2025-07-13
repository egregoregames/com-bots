using UnityEngine;

public class HUDLock : MonoBehaviour
{
	public GameObject follower;     

	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = follower.transform.position + new Vector3 (0,-1,2);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = follower.transform.position + new Vector3 (0,-1,2);
    }
    void LateUpdate()
    {
        transform.position = follower.transform.position + new Vector3 (0,-1,2);
    }
}
