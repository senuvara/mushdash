using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using UnityEngine;

namespace Assets.Scripts.GameCore
{
	public class Driver : MonoBehaviour
	{
		public Texture2D texture2D;

		protected void Start()
		{
			Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
			SingletonScriptableObject<NoteDataMananger>.instance.PreloadNotePrefabs();
			Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
		}
	}
}
