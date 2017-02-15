using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class AtlasPacker : EditorWindow
{

    [MenuItem("GameObject/atlasPaker")]
    static private void MakeAtlas()
    {
        string spriteDir = Application.dataPath + "/Resources/Sprite";

        if (!Directory.Exists(spriteDir))
        {
            Directory.CreateDirectory(spriteDir);
        }

        DirectoryInfo rootDirInfo = new DirectoryInfo(Application.dataPath + "/Rec/Atlas");
//        foreach (DirectoryInfo dirInfo in rootDirInfo.GetDirectories())
//        {
		foreach (FileInfo pngFile in rootDirInfo.GetFiles("*.png", SearchOption.AllDirectories))
            {
                string allPath = pngFile.FullName;
                string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
				string subName = pngFile.DirectoryName.Substring(allPath.IndexOf("Atlas") + 5);
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                GameObject go = new GameObject(sprite.name);
                go.AddComponent<SpriteRenderer>().sprite = sprite;
                allPath = spriteDir +subName+ "/" + sprite.name + ".prefab";
                string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
                if (!Directory.Exists(spriteDir  + subName))
                {
                    Directory.CreateDirectory(spriteDir  + subName);
                }

                PrefabUtility.CreatePrefab(prefabPath, go);
                GameObject.DestroyImmediate(go);
            }

		foreach (FileInfo pngFile in rootDirInfo.GetFiles("*.jpg", SearchOption.AllDirectories))
		{
			string allPath = pngFile.FullName;
			string assetPath = allPath.Substring(allPath.IndexOf("Assets"));
			string subName = pngFile.DirectoryName.Substring(allPath.IndexOf("Atlas") + 5);
			Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
			GameObject go = new GameObject(sprite.name);
			go.AddComponent<SpriteRenderer>().sprite = sprite;
			allPath = spriteDir +subName+ "/" + sprite.name + ".prefab";
			string prefabPath = allPath.Substring(allPath.IndexOf("Assets"));
			if (!Directory.Exists(spriteDir  + subName))
			{
				Directory.CreateDirectory(spriteDir  + subName);
			}

			PrefabUtility.CreatePrefab(prefabPath, go);
			GameObject.DestroyImmediate(go);
		}
//        }
    }

    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 500, 500);
        AtlasPacker window = (AtlasPacker)EditorWindow.GetWindowWithRect(typeof(AtlasPacker), wr, true, "AtlasPacker");
        window.Show();

    }

    #region tutorials


    ////输入文字的内容
    //private string text;
    ////选择贴图的对象
    //private Texture texture;

    //public void Awake()
    //{
    //    //在资源中读取一张贴图
    //    texture = Resources.Load("1") as Texture;
    //}

    ////绘制窗口时调用
    //void OnGUI()
    //{
    //    //输入框控件
    //    text = EditorGUILayout.TextField("输入文字:", text);

    //    if (GUILayout.Button("打开通知", GUILayout.Width(200)))
    //    {
    //        //打开一个通知栏
    //        this.ShowNotification(new GUIContent("This is a Notification"));
    //    }

    //    if (GUILayout.Button("关闭通知", GUILayout.Width(200)))
    //    {
    //        //关闭通知栏
    //        this.RemoveNotification();
    //    }

    //    //文本框显示鼠标在窗口的位置
    //    EditorGUILayout.LabelField("鼠标在窗口的位置", Event.current.mousePosition.ToString());

    //    //选择贴图
    //    texture = EditorGUILayout.ObjectField("添加贴图", texture, typeof(Texture), true) as Texture;

    //    if (GUILayout.Button("关闭窗口", GUILayout.Width(200)))
    //    {
    //        //关闭窗口
    //        this.Close();
    //    }

    //}

    ////更新
    //void Update()
    //{

    //}

    //void OnFocus()
    //{
    //    Debug.Log("当窗口获得焦点时调用一次");
    //}

    //void OnLostFocus()
    //{
    //    Debug.Log("当窗口丢失焦点时调用一次");
    //}

    //void OnHierarchyChange()
    //{
    //    Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    //}

    //void OnProjectChange()
    //{
    //    Debug.Log("当Project视图中的资源发生改变时调用一次");
    //}

    //void OnInspectorUpdate()
    //{
    //    //Debug.Log("窗口面板的更新");
    //    //这里开启窗口的重绘，不然窗口信息不会刷新
    //    this.Repaint();
    //}

    //void OnSelectionChange()
    //{
    //    //当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
    //    foreach (Transform t in Selection.transforms)
    //    {
    //        //有可能是多选，这里开启一个循环打印选中游戏对象的名称
    //        Debug.Log("OnSelectionChange" + t.name);
    //    }
    //}

    //void OnDestroy()
    //{
    //    Debug.Log("当窗口关闭时调用");
    //}
    #endregion


    //输入文字的内容
    private string text;
    //选择贴图的对象

    //绘制窗口时调用
    void OnGUI()
    {
        //输入框控件
        text = EditorGUILayout.TextField("输入文字:", text);

        if (GUILayout.Button("打开通知", GUILayout.Width(200)))
        {
            //打开一个通知栏
            this.ShowNotification(new GUIContent("This is a Notification"));
        }

        if (GUILayout.Button("关闭通知", GUILayout.Width(200)))
        {
            //关闭通知栏
            this.RemoveNotification();
        }

        //文本框显示鼠标在窗口的位置
        EditorGUILayout.LabelField("鼠标在窗口的位置", Event.current.mousePosition.ToString());


        if (GUILayout.Button("关闭窗口", GUILayout.Width(200)))
        {
            //关闭窗口
            this.Close();
        }

    }
}
