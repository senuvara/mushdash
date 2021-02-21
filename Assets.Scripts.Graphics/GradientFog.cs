using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Graphics
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("PeroPeroGames/Rendering/Gradient Fog")]
	public class GradientFog : PostEffectsBase
	{
		[InfoBox("使用前须知：\n· Fog Gradient的颜色帧和透明度帧 数量分别不可多于4个（包括头和尾），大于4的关键帧不会起作用。\n· 暂时不持支Fog Gradient 的Mode选为Fixed模式。", InfoMessageType.Warning, null)]
		[InfoBox("本脚本用于实现全局梯度雾效。\n雾效起始点为摄像机的近平面，终点为摄像机的远平面。", InfoMessageType.Info, null)]
		public Gradient fogGradient;

		public Shader fogShader;

		private Material m_FogMaterial;

		private RenderTexture m_RenderTexture;

		public override bool CheckResources()
		{
			CheckSupport(true);
			m_FogMaterial = CheckShaderAndCreateMaterial(fogShader, m_FogMaterial);
			if (!isSupported)
			{
				ReportAutoDisable();
			}
			return isSupported;
		}

		private void OnPreRender()
		{
			m_RenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
			Camera component = GetComponent<Camera>();
			component.targetTexture = m_RenderTexture;
			Matrix4x4 value = default(Matrix4x4);
			for (int i = 0; i < Mathf.Min(fogGradient.colorKeys.Length, 4); i++)
			{
				Color color = fogGradient.colorKeys[i].color;
				float time = fogGradient.colorKeys[i].time;
				value.SetRow(i, new Vector4(color.r, color.g, color.b, time));
			}
			Matrix4x4 value2 = default(Matrix4x4);
			for (int j = 0; j < Mathf.Min(fogGradient.alphaKeys.Length, 4); j++)
			{
				float alpha = fogGradient.alphaKeys[j].alpha;
				float time2 = fogGradient.alphaKeys[j].time;
				value2.SetRow(j, new Vector4(alpha, time2, 0f, 0f));
			}
			m_FogMaterial.SetMatrix("_FogColors", value);
			m_FogMaterial.SetMatrix("_FogAlphas", value2);
		}

		private void OnPostRender()
		{
			Camera component = GetComponent<Camera>();
			component.targetTexture = null;
			UnityEngine.Graphics.Blit(m_RenderTexture, null, m_FogMaterial, 0);
			RenderTexture.ReleaseTemporary(m_RenderTexture);
		}
	}
}
