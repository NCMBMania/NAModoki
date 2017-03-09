using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;
    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = this.transform.position;
    }
    private void FixedUpdate()
    {
        this.transform.position = player.position + defaultPosition;
    }
}