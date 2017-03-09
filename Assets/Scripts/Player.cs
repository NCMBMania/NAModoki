using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public FriendsContoroller friendsContoroller;
    private Rigidbody thisRigidBody;
    private List<MeshRenderer> rendererList;
    private Collider thisCollider;

    public GameObject targetPoint;
    public LayerMask targetHitMask;

    public float speed = 40f;

    private bool isBulletIntervalLock = false;
    public float bulletIntervalTime = 0.3f;

    public bool IsPaused { get; private set; }

    private UnityAction callbackWhenPlayerDead;
    private UnityAction<string, string> callbackWhenGetFriend;

    private bool isDead = false;

    public bool muteki = false;

    private void Awake()
    {
        thisCollider = GetComponent<Collider>();
        thisRigidBody = GetComponent<Rigidbody>();
        rendererList = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    public void SetDeathCallback(UnityAction callback)
    {
        callbackWhenPlayerDead = callback;
    }

    public void SetFriendGetCallback(UnityAction<string, string> callback)
    {
        callbackWhenGetFriend = callback;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        //Playerの移動//
        float horizontal = Input.GetAxis("Horizontal");
        thisRigidBody.AddForce(Vector3.right * horizontal * speed);

        float vertical = Input.GetAxis("Vertical");
        thisRigidBody.AddForce(Vector3.forward * vertical * speed);

        //ターゲットの移動//
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, 100f, targetHitMask))
        {
            targetPoint.transform.position = new Vector3(hit.point.x, 0.1f, hit.point.z);
        }

        this.transform.LookAt(new Vector3(targetPoint.transform.position.x, 1f, targetPoint.transform.position.z));

        //弾の発射//
        if (Input.GetButton("Fire1"))
        {
            if (isBulletIntervalLock == false)
            {
                Instantiate(bulletPrefab, transform.position, this.transform.rotation);

                friendsContoroller.Shot();

                isBulletIntervalLock = true;
                Invoke("ReleaseIntervalLock", bulletIntervalTime);
            }
        }

        if (this.transform.position.y < -10)
        {
            Core.Instance.OnMainGame();
        }
    }

    private void ReleaseIntervalLock()
    {
        isBulletIntervalLock = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!muteki && !isDead && collision.gameObject.CompareTag("Enemy"))
        {
            isDead = true;

            thisRigidBody.velocity = Vector3.zero;
            thisRigidBody.angularVelocity = Vector3.zero;
            rendererList.ForEach(renderer => renderer.enabled = false);
            thisCollider.enabled = false;

            callbackWhenPlayerDead();
        }

        if (!isDead && collision.gameObject.CompareTag("DeadFriend"))
        {
            DeadFriend deadFriend = collision.gameObject.GetComponent<DeadFriend>();
            friendsContoroller.ActiveOneFriend();

            string name = deadFriend.FriendData.Name;
            string message = deadFriend.FriendData.Message;

            callbackWhenGetFriend(name, message);

            Destroy(collision.gameObject);
        }
    }
}