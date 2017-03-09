using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendsContoroller : MonoBehaviour
{
    public List<GameObject> friendGameObjectList;
    public GameObject bulletPrefab;

    public float angleParSecontd = 45;
    public Transform followTransform;

    private void Awake()
    {
        friendGameObjectList.ForEach(gameObject => gameObject.SetActive(false));
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, angleParSecontd * Time.deltaTime, 0));
        transform.position = followTransform.position;

        List<GameObject> friendsPosition = friendGameObjectList.Where(fr => fr.activeSelf == true).ToList();
        if (friendsPosition.Count != 0)
        {
            friendsPosition.ForEach(obj => obj.transform.rotation = followTransform.rotation);
        }
    }

    public void Shot()
    {
        List<GameObject> friendGameObject = friendGameObjectList.Where(fr => fr.activeSelf == true).ToList();
        if (friendGameObject.Count != 0)
        {
            friendGameObject.ForEach(obj => Instantiate(bulletPrefab, obj.transform.position, followTransform.rotation));
        }
    }

    public void ActiveOneFriend()
    {
        GameObject friendObject = friendGameObjectList.FirstOrDefault(fr => fr.activeSelf == false);
        if (friendObject != null)
        {
            friendObject.SetActive(true);
        }
    }
}