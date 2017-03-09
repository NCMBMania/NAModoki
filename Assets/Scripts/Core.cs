using NCMB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    public static Core Instance;

    public string PlayerName { private get; set; }
    public string Message { private get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        OnTitle();
    }

    public void OnTitle()
    {
        SceneManager.LoadScene("SelectWords");
    }

    public void OnMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void SaveFriendDataToNCMB(Vector3 position)
    {
        NCMBObject ncmbObject = new NCMBObject("FriendData");

        // オブジェクトに値を設定//
        ncmbObject["Name"] = PlayerName;
        ncmbObject["Message"] = Message;
        ncmbObject["Position"] = position.ToDoubleArray();

        // データストアへの登録//
        ncmbObject.SaveAsync();
    }

    public void FetchFriendDataFromNCMB(UnityAction<List<FriendData>> callback)
    {
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("FriendData");

        query.OrderByDescending("createDate");
        query.Limit = 20;

        query.FindAsync((List<NCMBObject> childObjList, NCMBException error) =>
        {
            if (error != null)
            {
                //エラー処理
            }
            else
            {
                //成功時の処理
                List<FriendData> friendDataList = new List<FriendData>();

                foreach (NCMBObject obj in childObjList)
                {
                    string name = (string)obj["Name"];
                    string message = (string)obj["Message"];
                    ArrayList doubleArrayPosition = (ArrayList)obj["Position"];
                    Vector3 position = doubleArrayPosition.ToVector3();
                    friendDataList.Add(new FriendData(name, message, position));
                }

                callback(friendDataList);
            }
        });
    }
}