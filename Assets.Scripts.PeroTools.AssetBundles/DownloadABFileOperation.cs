using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class DownloadABFileOperation : OperationBase
	{
		protected string m_ServerUrl;

		protected string m_LocalUrl;

		protected WWW m_Www;

		private bool m_ExtreOpr;

		public override bool keepWaiting
		{
			get
			{
				if (m_Www != null && m_Www.isDone)
				{
					if (m_Www.bytes.Length == 0)
					{
						Debug.LogErrorFormat("Unable to download file {0}", m_ServerUrl);
						return true;
					}
					Directory.CreateDirectory(Path.GetDirectoryName(m_LocalUrl));
					File.WriteAllBytes(m_LocalUrl, m_Www.bytes);
					m_ExtreOpr = true;
				}
				return !IsDone();
			}
		}

		public DownloadABFileOperation(string serverUrl, string localUrl)
		{
			m_ServerUrl = serverUrl;
			m_LocalUrl = localUrl;
			m_Www = new WWW(m_ServerUrl);
		}

		public override bool IsDone()
		{
			return m_Www != null && m_Www.isDone && m_ExtreOpr;
		}
	}
}
