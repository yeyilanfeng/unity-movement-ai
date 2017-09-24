using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


namespace FinishedScript
{
	public class AssetBookmarkFinGUI : EditorWindow
	{
		#region Variables Declaration
		// Public Variables
		public string newFavName = "";					// New name for the asset Bookmark
		public Vector2 scrollPosition = Vector2.zero;	// The position of the Scroll

		// Private Variables
		private GUISkin gSkin;
		private GUIStyle titleStyle;
		private GUIStyle highlightBttnStyle;
		private GUIStyle removeBttnStyle;

		private List<Object> bookmarkedObjects = new List<Object>();
		private List<string> bookmarkedNames = new List<string>();
		#endregion

		#region Menu Item
		[MenuItem ("Assets/Asset Bookmark FIN", false, 1050)]
		static void Init()
		{
			AssetBookmarkFinGUI assetBookmarkGUI = (AssetBookmarkFinGUI)EditorWindow.GetWindow(typeof(AssetBookmarkFinGUI));

			assetBookmarkGUI.titleContent = new GUIContent("Asset Bookmarks");
			assetBookmarkGUI.minSize = new Vector2(200, 200);

			assetBookmarkGUI.Show();
		}

		#endregion

		void OnEnable()
		{
			LoadDatabase();
			InitStyle();
		}

		void OnGUI()
		{
			GUILayout.BeginVertical();
			{
				GUILayout.Space(10);
				EditorGUILayout.LabelField("Asset Bookmarks", titleStyle);
				GUILayout.Space(10);

				EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(position.width - 5));
				{
					scrollPosition = GUILayout.BeginScrollView(scrollPosition);	// To hold the scroll position
					{
						if (bookmarkedObjects.Count > 0)
						{
							for (int i = 0; i < bookmarkedObjects.Count; i++)
							{
								GUILayout.BeginHorizontal(GUILayout.Height(20));
								{
									if (GUILayout.Button(bookmarkedNames[i], highlightBttnStyle, GUILayout.Height(20)))
										HighlightObject(i);

									GUILayout.Space(2);

									if (GUILayout.Button("", removeBttnStyle, GUILayout.Width(20), GUILayout.Height(20)))
										RemoveObject(i);
								}
								GUILayout.EndHorizontal();
								GUILayout.Space(3);
							}
						}
					}
					GUILayout.EndScrollView();
				}
				EditorGUILayout.EndVertical();

				GUILayout.Space(10);
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Name: ", GUILayout.Width(40));

					GUI.SetNextControlName("NameField");				// Set a name for this field
					newFavName = EditorGUILayout.TextField(newFavName);

					if (GUILayout.Button("Add", GUILayout.Width(50)))
						AddObjects();
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(10);
			}
			GUILayout.EndVertical();

			Repaint();
		}

		void HighlightObject (int assetIndex)
		{
			EditorGUIUtility.PingObject(bookmarkedObjects[assetIndex]);
		}

		#region Add / Remove Objects
		void AddObjects()
		{
			if (Selection.activeObject && AssetDatabase.Contains(Selection.activeObject))
			{
				if (bookmarkedObjects.Contains(Selection.activeObject))	// Check if the selected object is allready added in the Bookmark list
				{
					Debug.LogWarning("*** Object is allready in the list");
					return;
				}

				if (newFavName == "")	// Check if the name field is empty
				{
					Debug.LogWarning("*** Please Add Name for the Highlighted Object");
					return;
				}

				bookmarkedObjects.Add(Selection.activeObject);	// Add the objec to the list with Bookmarks
				bookmarkedNames.Add(newFavName);
				newFavName = "";
			}
			else
			{
				Debug.LogWarning("*** Please Select an Asset");
				return;
			}

			SaveDatabase();	// Save the database
		}

		void RemoveObject (int assetIndex)
		{
			bookmarkedObjects.RemoveAt(assetIndex);
			bookmarkedNames.RemoveAt(assetIndex);

			SaveDatabase();
		}

		#endregion

		void LoadDatabase()
		{
			if (File.Exists("Assets/AssetBookmark/Resources/Data.txt"))
			{
				string loadData = File.ReadAllText("Assets/AssetBookmark/Resources/Data.txt");
				string[] elements = loadData.Split(new string[1] { "*" }, System.StringSplitOptions.RemoveEmptyEntries);

				bookmarkedObjects.Clear();
				bookmarkedNames.Clear();

				for (int i = 0; i < elements.Length; i++)
				{
					string[] objectData = elements[i].Split(new string[1] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

					bookmarkedObjects.Add(AssetDatabase.LoadAssetAtPath(objectData[0], typeof(Object)) as Object);
					bookmarkedNames.Add(objectData[1]);
				}
			}
		}

		void SaveDatabase()
		{
			string saveData = "";

			for (int i = 0; i < bookmarkedNames.Count; i++)
			{
				saveData += AssetDatabase.GetAssetPath(bookmarkedObjects[i]) + "," + bookmarkedNames[i] + "*";
			}

			// Save the string in a text file
			File.WriteAllText("Assets/AssetBookmark/Resources/Data.txt", saveData);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		void InitStyle()
		{
			GUISkin gSkin = (GUISkin)Resources.Load("ABGuiSkin");

			titleStyle = gSkin.GetStyle("Title");
			highlightBttnStyle = gSkin.GetStyle("HighlightBttn");
			removeBttnStyle = gSkin.GetStyle("RemoveBttn");
		}
	}
}