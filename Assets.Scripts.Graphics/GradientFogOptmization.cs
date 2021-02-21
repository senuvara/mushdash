using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

namespace Assets.Scripts.Graphics
{
	public class GradientFogOptmization : PostEffectsBase
	{
		public Shader fogShader;

		public Texture fogTexture;

		private Camera m_Cam;

		private Material m_FogMaterial;

		private int m_FogTexId;

		private int m_Height;

		private float m_PassTime;

		private int m_PassTimeId;

		private RenderTexture m_RenderTexture;

		private int m_TotalTimeId;

		private int m_Width;

		public float totalTime;

		private UnityGameManager m_UnityGameManager;

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
			if (!(m_FogMaterial == null))
			{
				m_RenderTexture = RenderTexture.GetTemporary(m_Width, m_Height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
				m_Cam.targetTexture = m_RenderTexture;
			}
		}

		private void OnPostRender()
		{
			if (!(m_FogMaterial == null))
			{
				m_Cam.targetTexture = null;
				m_FogMaterial.SetTexture(m_FogTexId, fogTexture);
				m_FogMaterial.SetFloat(m_TotalTimeId, Mathf.Max(0.01f, totalTime));
				m_FogMaterial.SetFloat(m_PassTimeId, m_PassTime);
				UnityEngine.Graphics.Blit(m_RenderTexture, null, m_FogMaterial, 0);
				RenderTexture.ReleaseTemporary(m_RenderTexture);
			}
		}

		protected override void Start()
		{
			base.Start();
			if (fogTexture == null)
			{
				Debug.LogWarning("Fot texture is null.");
				ReportAutoDisable();
				NotSupported();
				return;
			}
			m_Cam = GetComponent<Camera>();
			if (m_Cam == null)
			{
				ReportAutoDisable();
				NotSupported();
				return;
			}
			m_Width = Screen.width;
			m_Height = Screen.height;
			m_PassTime = 0f;
			m_FogTexId = Shader.PropertyToID("_FogTex");
			m_TotalTimeId = Shader.PropertyToID("_TotalTime");
			m_PassTimeId = Shader.PropertyToID("_PassTime");
		}

		private void Update()
		{
			m_PassTime += Time.unscaledDeltaTime;
		}
	}
}
