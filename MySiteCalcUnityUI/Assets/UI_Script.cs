using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UI_Script : MonoBehaviour
{
    private int maxWidth;
    private int maxHeight;
    private JSONUser[] users;
    private JsonRoot jsonRoot;
    int userSelectedId = -1;

    void OnGUI()
    {
        maxWidth = Screen.width;
        maxHeight = Screen.height;

        #region First Box for Users
        GUI.Box(new Rect(5, 5, (maxWidth - 10), (maxHeight - 10) / 2), "Users");

        GUILayout.BeginArea(new Rect(5, 25, (maxWidth - 10), (maxHeight - 10) / 2));

        GUILayout.BeginHorizontal(new GUIStyle() { alignment = TextAnchor.MiddleCenter });
        GUILayout.FlexibleSpace();
        GUILayout.Label("User name");
        GUILayout.FlexibleSpace();
        GUILayout.Label("User email");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Status");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (users != null)
        {
            for (int i = 0; i < users.Length; i++)
            {
                GUILayout.BeginHorizontal(new GUIStyle() { alignment = TextAnchor.UpperLeft });
                GUILayout.FlexibleSpace();
                bool clicked = GUILayout.Button(users[i].name, "Label");
                if (clicked)
                    userSelectedId = i;
                GUILayout.FlexibleSpace();
                GUILayout.Label(users[i].email);
                GUILayout.FlexibleSpace();
                GUILayout.Label(users[i].status);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndArea();
        #endregion

        #region Second Box for Projects
        GUI.Box(new Rect(5, (maxHeight / 3) * 2 - 20, (maxWidth - 10), (maxHeight - 20) / 3), "Projects");

        GUILayout.BeginArea(new Rect(5, (maxHeight / 3) * 2, (maxWidth - 10), (maxHeight) / 3));

        GUILayout.BeginHorizontal(new GUIStyle() { alignment = TextAnchor.MiddleCenter });
        GUILayout.FlexibleSpace();
        GUILayout.Label("Project name");
        GUILayout.FlexibleSpace();
        GUILayout.Label("PM name");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (users != null && userSelectedId != -1)
        {
            foreach (JSONProject p in users[userSelectedId].clients_projects)
            {
                GUILayout.BeginHorizontal(new GUIStyle() { alignment = TextAnchor.MiddleCenter });
                GUILayout.FlexibleSpace();
                GUILayout.Label(p.name);
                GUILayout.FlexibleSpace();
                GUILayout.Label(p.project_manager.name);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            foreach (JSONProject p in users[userSelectedId].pm_projects)
            {
                GUILayout.BeginHorizontal(new GUIStyle() { alignment = TextAnchor.MiddleCenter });
                GUILayout.FlexibleSpace();
                GUILayout.Label(p.name);
                GUILayout.FlexibleSpace();
                GUILayout.Label(p.project_manager.name);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndArea();
        #endregion



    }

    #region GET
    private IEnumerator getUnityWebRequest()
    {
        string url = "http://polinaliola.000webhostapp.com/api/data";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if (www.isError)
        {
            //Debug.Log(www.error + " error code: " + www.responseCode);
        }
        else
        {
            string json = "{\"users\":" + www.downloadHandler.text + "}";
            //Debug.Log(json);
            jsonRoot = JsonUtility.FromJson<JsonRoot>(json);
            users = jsonRoot.users;

            //Debug.Log(_object);
        }
    }
    #endregion



    // Use this for initialization
    void Start()
    {
        //Camera.main.orthographicSize = Mathf.Max(Screen.width, Screen.height) / 2;
        // StartCoroutine(getWWW());
        StartCoroutine(getUnityWebRequest());
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Serializable]
    public class JsonRoot
    {
        public JSONUser[] users;
    }

    #region JSONUser
    [Serializable]
    public class JSONUser
    {
        public int id;
        public string name;
        public string email;
        public string status;
        public DateTime created_at;
        public DateTime updated_at;
        public string avatar;
        public JSONProject[] clients_projects;
        public JSONProject[] pm_projects;
    }

    #endregion
    #region JSON Project
    [Serializable]
    public class JSONProject
    {
        public int id;
        public string name;
        public string description;
        public int client_id;
        public int project_manager_id;
        public string status;
        public DateTime created_at;
        public DateTime updated_at;
        public JSONManager project_manager;

    }
    [Serializable]
    public class JSONManager
    {
        public int id;
        public string name;
        public string email;
        public string status;
        public DateTime created_at;
        public DateTime updated_at;
        public string avatar;
    }
    #endregion
}
