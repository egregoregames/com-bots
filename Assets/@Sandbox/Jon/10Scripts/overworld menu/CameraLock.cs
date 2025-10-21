using UnityEngine;

public class CameraLock : MonoBehaviour
{
	public GameObject follower;     

	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = follower.transform.position + new Vector3(0, 4.8f, -4.8f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follower.transform.position + new Vector3(0, 4.8f, -4.8f);
    }
    void LateUpdate()
    {
        //transform.position = follower.transform.position + new Vector3(0, 4.8f, -4.8f);
    }
}
