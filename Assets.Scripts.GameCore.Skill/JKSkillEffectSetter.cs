using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.GameCore.Skill
{
	public class JKSkillEffectSetter : SerializedMonoBehaviour
	{
		[ToggleGroup("catchGroundCircleColorEnable", 0, null)]
		public bool catchGroundCircleColorEnable = true;

		[ToggleGroup("catchGroundCircleColorEnable", 0, null)]
		public Color catchGroundCircleColor;

		[ToggleGroup("catchGroundCircle2ColorEnable", 0, null)]
		public bool catchGroundCircle2ColorEnable = true;

		[ToggleGroup("catchGroundCircle2ColorEnable", 0, null)]
		public Color catchGroundCircle2Color;

		[ToggleGroup("catchAirCircleColorEnable", 0, null)]
		public bool catchAirCircleColorEnable = true;

		[ToggleGroup("catchAirCircleColorEnable", 0, null)]
		public Color catchAirCircleColor;

		[ToggleGroup("catchAirCircle2ColorEnable", 0, null)]
		public bool catchAirCircle2ColorEnable = true;

		[ToggleGroup("catchAirCircle2ColorEnable", 0, null)]
		public Color catchAirCircle2Color;

		[ToggleGroup("catchCircleScaleEnable", 0, null)]
		public bool catchCircleScaleEnable = true;

		[ToggleGroup("catchCircleScaleEnable", 0, null)]
		public Vector2 catchCircleScale;

		[ToggleGroup("catchCircle2ScaleEnable", 0, null)]
		public bool catchCircle2ScaleEnable = true;

		[ToggleGroup("catchCircle2ScaleEnable", 0, null)]
		public Vector2 catchCircle2Scale;

		[ToggleGroup("localPositionEnable", 0, null)]
		public bool localPositionEnable = true;

		[ToggleGroup("localPositionEnable", 0, null)]
		public Vector3 localPosition;

		[ToggleGroup("materialEnable", 0, null)]
		public bool materialEnable = true;

		[ToggleGroup("materialEnable", 0, null)]
		public Material material;

		private Material m_DefaultMaterial;
	}
}
