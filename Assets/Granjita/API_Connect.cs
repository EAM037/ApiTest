using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Defective.JSON;

public class API_Connect : MonoBehaviour
{
    public int tractorId;
    public GameObject plantPrefab;
    public int simulationCellSize = 20;
    private Grid Grid;
    public List<(int, int)> values;
    public TextAsset jsonFile;
    public bool connectionFlag = true;

    // Start is called before the first frame update
    public string url = "http://localhost:8000/get_coordinates?id=";
    void Start()
    {
        Grid = GetComponentInParent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoRequest(int tractorId)
    {
        string auxUrl = url + tractorId.ToString();
        Debug.Log(auxUrl);
        StartCoroutine(GetRequest(auxUrl));
        values = new List<(int, int)>();
    }

    void UseBackupJSON()
    {
        string jsonString = jsonFile.text.ToString();
        JSONObject backup = new JSONObject(jsonString);
        AccessBackup(backup);

    }

    void AccessBackup(JSONObject jsonObject)
    {
        jsonObject = jsonObject.list[tractorId];
        switch (jsonObject.type)
        {
            case JSONObject.Type.Object:
                for (var i = 0; i < jsonObject.list.Count; i++)
                {
                    var key = jsonObject.keys[i];
                    var value = jsonObject.list[i];
                    Debug.Log(key);
                    AccessData(value);
                }
                break;
            case JSONObject.Type.Array:
                List<(int, int)> auxList = new List<(int, int)>();
                for (int i = 0; i < jsonObject.list.Count; i++)
                {
                    int x = jsonObject.list[i][0].intValue;
                    int y = jsonObject.list[i][1].intValue;
                    x = (x / 10);
                    y = (y / 10);
                    auxList.Add((x, y));
                    //Instantiate(plantPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    //Debug.Log("x: " + x + " y: " + y);
                }
                values.AddRange(auxList);
                break;
        }
    }

    void AccessData(JSONObject jsonObject)
    {
        switch (jsonObject.type)
        {
            case JSONObject.Type.Object:
                for (var i = 0; i < jsonObject.list.Count; i++)
                {
                    var key = jsonObject.keys[i];
                    var value = jsonObject.list[i];
                    Debug.Log(key);
                    AccessData(value);
                }
                break;
            case JSONObject.Type.Array:
                List<(int, int)> auxList = new List<(int, int)>();
                for (int i = 0; i < jsonObject.list.Count; i++)
                {
                    int x = jsonObject.list[i][0].intValue;
                    int y = jsonObject.list[i][1].intValue;
                    x = (x / 10);
                    y = (y / 10);
                    auxList.Add((x, y));
                    //Instantiate(plantPrefab, new Vector3(x, 0, y), Quaternion.identity);
                    //Debug.Log("x: " + x + " y: " + y);
                }
                values.AddRange(auxList);
                break;
        }
    }



    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Connection Error");
                    connectionFlag = false;
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    connectionFlag = false;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    connectionFlag = false;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    JSONObject res = new JSONObject(webRequest.downloadHandler.text);
                    AccessData(res);
                    break;
            }
        }
        if (!connectionFlag)
        {
            Debug.Log("Using Backup");
            UseBackupJSON();
        }
    }
}
