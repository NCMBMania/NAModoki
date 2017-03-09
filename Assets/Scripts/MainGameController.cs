using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float enemySpawnInterval = 2f;
    public Collider enemySpawnAreaCollider;
    public Player player;

    public Canvas retryCanvas;

    public GameObject deadFriendPrefab;

    public bool isRandomStartPointMode = false;

    public Canvas deadFriendMessageCanvas;
    public Text deadFriendName;
    public Text deadFriendMessage;

    private void Start()
    {
        if (isRandomStartPointMode)
        {
            player.gameObject.transform.position = GetRandomPosFromGameArea();
        }

        player.SetDeathCallback(PlayerDead);
        player.SetFriendGetCallback(ShowDeadFriendMessage);

        Core.Instance.FetchFriendDataFromNCMB(InstantiateDeadFriends);

        StartCoroutine("SpawnEnemy");

        retryCanvas.enabled = false;
        deadFriendMessageCanvas.enabled = false;
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            //プレイヤーから十分遠い場所にスポーンさせる//
            Vector3 newPos;
            float distance;
            do
            {
                newPos = GetRandomPosFromGameArea();
                distance = Vector3.Distance(player.transform.position, newPos);
            } while (distance < 5f);

            GameObject enemyObject = Instantiate(enemyPrefab, newPos, Quaternion.identity);

            enemyObject.GetComponent<Enemy>().SetTarget(player.gameObject.transform);

            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    private Vector3 GetRandomPosFromGameArea()
    {
        float x = Random.Range(enemySpawnAreaCollider.bounds.min.x, enemySpawnAreaCollider.bounds.max.x);
        float z = Random.Range(enemySpawnAreaCollider.bounds.min.z, enemySpawnAreaCollider.bounds.max.z);
        return new Vector3(x, 1f, z);
    }

    public void PlayerDead()
    {
        retryCanvas.enabled = true;

        Core.Instance.SaveFriendDataToNCMB(player.gameObject.transform.position);

        StopCoroutine("SpawnEnemy");
    }

    public void ShowDeadFriendMessage(string name, string message)
    {
        deadFriendMessageCanvas.enabled = true;
        deadFriendName.text = name;
        deadFriendMessage.text = message;

        Invoke("HideDeadFriendMessage", 3);
    }

    private void HideDeadFriendMessage()
    {
        deadFriendMessageCanvas.enabled = false;
    }

    public void RetryGame()
    {
        Core.Instance.OnMainGame();
    }

    public void InstantiateDeadFriends(List<FriendData> friendDataList)
    {
        friendDataList.ForEach(friendData => SetDeadFriend(friendData));
    }

    private void SetDeadFriend(FriendData friendData)
    {
        DeadFriend deadFriend = Instantiate(deadFriendPrefab, friendData.Position, Quaternion.identity).GetComponent<DeadFriend>();
        deadFriend.FriendData = friendData;
    }
}

public class FriendData
{
    public readonly string Name;
    public readonly string Message;
    public readonly Vector3 Position;

    public FriendData(string name, string message, Vector3 position)
    {
        Name = name;
        Message = message;
        Position = position;
    }
}