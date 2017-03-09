using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public float speed = 100f;

    private void Start()
    {
        Invoke("Disappear", 2f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Disappear();
    }

    private void OnBecameInvisible()
    {
        Disappear();
    }

    void Disappear()
    {
        Destroy(this.gameObject);
    }
}
