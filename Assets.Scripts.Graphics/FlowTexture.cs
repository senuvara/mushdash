using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Graphics
{
	[AddComponentMenu("PeroPeroGames/Rendering/FlowTexture")]
	[ExecuteInEditMode]
	public class FlowTexture : MonoBehaviour
	{
		[InfoBox("使用前须知:\n· 原图和滚动纹理，都暂不支持图集。", InfoMessageType.Warning, null)]
		public Texture flowTexture;

		public Vector2 scale = new Vector2(1f, 1f);

		public Color flowTextureColor = Color.white;

		[Tooltip("单位：纹理长度")]
		[Range(-1f, 1f)]
		public float flowSpeedX;

		[Tooltip("单位：纹理长度")]
		[Range(-1f, 1f)]
		public float flowSpeedY;

		[Range(0f, 1f)]
		public float offsetX;

		[Range(0f, 1f)]
		public float offsetY;

		[ReadOnly]
		public Shader shader;

		private Material m_Material;

		private int m_FlowTexId;

		private int m_FlowColorId;

		private int m_FlowScaleXId;

		private int m_FlowScaleYId;

		private int m_SpeedXId;

		private int m_SpeedYId;

		private int m_OffsetXId;

		private int m_OffsetYId;

		private void Start()
		{
			Image component = GetComponent<Image>();
			if (component != null)
			{
				if (component.material.shader != shader)
				{
					component.material = new Material(shader);
				}
				m_Material = component.material;
			}
			else
			{
				Renderer component2 = GetComponent<Renderer>();
				if (component2 != null)
				{
					if (component2.material.shader != shader)
					{
						component2.material = new Material(shader);
					}
					m_Material = component2.material;
				}
			}
			m_FlowTexId = Shader.PropertyToID("_FlowTex");
			m_FlowColorId = Shader.PropertyToID("_FlowColor");
			m_FlowScaleXId = Shader.PropertyToID("_FlowScaleX");
			m_FlowScaleYId = Shader.PropertyToID("_FlowScaleY");
			m_SpeedXId = Shader.PropertyToID("_FlowSpeedX");
			m_SpeedYId = Shader.PropertyToID("_FlowSpeedY");
			m_OffsetXId = Shader.PropertyToID("_FlowOffsetX");
			m_OffsetYId = Shader.PropertyToID("_FlowOffsetY");
			SetParas();
		}

		private void SetParas()
		{
			if (m_Material != null)
			{
				m_Material.SetTexture(m_FlowTexId, flowTexture);
				m_Material.SetColor(m_FlowColorId, flowTextureColor);
				m_Material.SetFloat(m_FlowScaleXId, scale.x);
				m_Material.SetFloat(m_FlowScaleYId, scale.y);
				m_Material.SetFloat(m_SpeedXId, flowSpeedX);
				m_Material.SetFloat(m_SpeedYId, flowSpeedY);
				m_Material.SetFloat(m_OffsetXId, offsetX);
				m_Material.SetFloat(m_OffsetYId, offsetY);
			}
		}
	}
}
