using System;
using UnityEngine;

namespace Sirenix.OdinInspector.Demos
{
	[HideMonoScript]
	[AddComponentMenu("Nice/Vision/TextureMotion")]
	public class TextureMotion : MonoBehaviour
	{
		[Serializable]
		public class MyTabObject
		{
			public enum SomeEnum
			{
				Normal,
				Multiply,
				Overlay,
				Lighten
			}

			public enum SomeEnum1
			{
				Move,
				Rotation
			}

			public bool on;

			public Sprite texture;

			[EnumPaging]
			public SomeEnum superimposition;

			public bool showMaskGraphic;

			[Space(5f)]
			[EnumPaging]
			public SomeEnum1 MontionType;

			[ShowIf("MontionType", SomeEnum1.Rotation, true)]
			public Vector2 pivot;

			[ShowIf("MontionType", SomeEnum1.Rotation, true)]
			public float roataeSpeed;

			[ShowIf("MontionType", SomeEnum1.Move, true)]
			public Vector2 offsetSpeed;
		}

		[Space(10f)]
		public Sprite tartget;

		[TabGroup("Texture1", false, 0)]
		[HideLabel]
		public MyTabObject TabA;

		[TabGroup("Texture2", false, 0)]
		[HideLabel]
		public MyTabObject TabB;

		[TabGroup("Texture3", false, 0)]
		[HideLabel]
		public MyTabObject TabC;
	}
}
