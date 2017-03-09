using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 20f;

    private void Update()
    {
        this.transform.LookAt(new Vector3(target.position.x, 1f, target.position.z));
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(this.gameObject);
        }
    }

    public void SetTarget(Transform enemyTarget)
    {
        target = enemyTarget;
    }
}