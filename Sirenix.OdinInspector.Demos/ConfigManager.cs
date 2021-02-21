using UnityEngine;

namespace Sirenix.OdinInspector.Demos
{
	[HideMonoScript]
	[AddComponentMenu("Nice/Data/ConfigManager")]
	public class ConfigManager : MonoBehaviour
	{
		public enum SomeEnum
		{
			Text,
			Color,
			Image,
			Children
		}

		public enum SomeEnum1
		{
			English,
			Chinese,
			Japanese
		}

		[Title("Basics", null, TitleAlignments.Left, true, true)]
		[EnumPaging]
		public SomeEnum PropertyType;

		[ShowIf("PropertyType", SomeEnum.Text, true)]
		public Component text;

		[ShowIf("PropertyType", SomeEnum.Color, true)]
		public Component color;

		[ShowIf("PropertyType", SomeEnum.Image, true)]
		public Component image;

		[ShowIf("PropertyType", SomeEnum.Children, true)]
		public GameObject children;

		[Title("Config", null, TitleAlignments.Left, true, true)]
		[ShowIf("PropertyType", SomeEnum.Text, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public string English = "NMB!WCNM!!((٩(//\u0300Д/\u0301/)۶))";

		[ShowIf("PropertyType", SomeEnum.Text, true)]
		public Font englishFont;

		[ShowIf("PropertyType", SomeEnum.Text, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public string Chinese = "你忙吧，我吃柠檬。";

		[ShowIf("PropertyType", SomeEnum.Text, true)]
		public Font chinieseFont;

		[ShowIf("PropertyType", SomeEnum.Text, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public string Japanese = "あんた、バガ？";

		[ShowIf("PropertyType", SomeEnum.Text, true)]
		public Font japaneseFont;

		[Title("Config", null, TitleAlignments.Left, true, true)]
		[ShowIf("PropertyType", SomeEnum.Color, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public Color color1 = new Color(0f, 0f, 0f);

		[ShowIf("PropertyType", SomeEnum.Color, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public Color color2 = new Color(0f, 0f, 0f);

		[ShowIf("PropertyType", SomeEnum.Color, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public Color color3 = new Color(0f, 0f, 0f);

		[Title("Config", null, TitleAlignments.Left, true, true)]
		[ShowIf("PropertyType", SomeEnum.Image, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public Sprite sprite1;

		[ShowIf("PropertyType", SomeEnum.Image, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public Sprite sprite2;

		[ShowIf("PropertyType", SomeEnum.Image, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public Sprite sprite3;

		[Title("Config", null, TitleAlignments.Left, true, true)]
		[ShowIf("PropertyType", SomeEnum.Children, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public GameObject perfab1;

		[ShowIf("PropertyType", SomeEnum.Children, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public GameObject perfab2;

		[ShowIf("PropertyType", SomeEnum.Children, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public GameObject perfab3;

		[Title("Default Scheme", null, TitleAlignments.Left, true, true)]
		[InfoBox("以下开关可以在编辑器模式下立即切换标签下的所有配置。设置按钮可以打开设置窗口，培减标签分类和子项。", InfoMessageType.Info, null)]
		[InlineButton("Setup", null)]
		[EnumToggleButtons]
		[HideLabel]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public SomeEnum1 WideEnumField;

		private void Setup()
		{
		}
	}
}
