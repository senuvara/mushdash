using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Graphics
{
	[RequireComponent(typeof(Renderer))]
	public class ChangeMaterialColor : MonoBehaviour
	{
		[InfoBox("使用前须知：\n· Property Name 用于指示想要修改的材质属性的名字。注意它不是指材质面板下的显示的属性名，而是着色器源代码中定义的属性名。\n建议使用前寻求技术协助，得到需要修改的属性的真正的名字。", InfoMessageType.Warning, null)]
		[InfoBox("本脚本用于协助访问大部分渲染组件的材质颜色属性", InfoMessageType.Info, null)]
		public string propertyName;

		private Material m_Mtr;

		public Color color
		{
			get
			{
				return m_Mtr.GetColor(propertyName);
			}
			set
			{
				m_Mtr.SetColor(propertyName, value);
			}
		}

		private void Awake()
		{
			m_Mtr = GetComponent<Renderer>().material;
		}
	}
}
