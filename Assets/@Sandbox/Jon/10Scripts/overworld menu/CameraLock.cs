using UnityEngine;

public class CameraLock : MonoBehaviour
{
	public GameObject follower;     

	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = follower.transform.position + new Vector3(0, 3, -6);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follower.transform.position + new Vector3(0, 3, -6);
    }
    void LateUpdate()
    {
        //transform.position = follower.transform.position + new Vector3(0, 3, -6);
    }
}
