using UnityEngine;

public class SocketDebugger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered socket: " + other.name);
    }
}
