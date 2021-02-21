using Assets.Scripts.PeroTools.Commons;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class ChangeColor : Action
	{
		[SerializeField]
		[CustomValueDrawer("OnPathGUI")]
		private string m_Path;

		[SerializeField]
		private Color m_Color;

		[SerializeField]
		[CustomValueDrawer("OnDurationGUI")]
		private float m_Duration = 1f;

		[SerializeField]
		[HideIf("m_Duration", 0f, true)]
		private Ease m_Ease = Ease.Linear;

		[SerializeField]
		[HideInInspector]
		private object m_SourceObject;

		[SerializeField]
		[HideInInspector]
		private Renderer m_Renderer;

		private Material m_Material;

		private MemberInfo m_SetMemberInfo;

		private MemberInfo m_GetMemberInfo;

		private Tween m_Tween;

		public override float duration => m_Duration;

		public override void Enter()
		{
			string[] array = m_Path.Split(' ');
			if (array.Length > 1)
			{
				string name = array[1].Split('(')[0];
				MemberInfo[] member = m_SourceObject.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.GetProperty);
				if (member.Length > 0)
				{
					m_GetMemberInfo = member.First();
				}
				MemberInfo[] member2 = m_SourceObject.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetField | BindingFlags.SetProperty);
				if (member2.Length > 0)
				{
					m_SetMemberInfo = member2.First();
				}
			}
		}

		public override void Execute()
		{
			if (m_GetMemberInfo != null && m_SetMemberInfo != null)
			{
				m_Tween = DOTween.To(() => (Color)m_GetMemberInfo.GetMemberValue(m_SourceObject), delegate(Color value)
				{
					m_SetMemberInfo.SetMemberValue(m_SourceObject, value);
				}, m_Color, m_Duration).SetEase(m_Ease);
			}
			else
			{
				if (!m_Renderer)
				{
					return;
				}
				if (!m_Material)
				{
					if (m_Renderer.material.GetTag("id", false) == "change_color")
					{
						m_Material = m_Renderer.material;
					}
					else
					{
						m_Material = Object.Instantiate(m_Renderer.material);
						m_Material.SetOverrideTag("id", "change_color");
						m_Renderer.material = m_Material;
					}
				}
				if ((bool)m_Material)
				{
					string propertyName = m_Path.LastAfter('/');
					m_Tween = DOTween.To(() => m_Material.GetColor(propertyName), delegate(Color value)
					{
						m_Material.SetColor(propertyName, value);
					}, m_Color, m_Duration).SetEase(m_Ease);
				}
			}
		}

		public override void Pause()
		{
			m_Tween.Pause();
		}

		public override void Resume()
		{
			if (!m_Tween.IsComplete())
			{
				m_Tween.Play();
			}
		}
	}
}
