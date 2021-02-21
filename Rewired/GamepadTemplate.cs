using System;

namespace Rewired
{
	public sealed class GamepadTemplate : ControllerTemplate, IGamepadTemplate, IControllerTemplate
	{
		public static readonly Guid typeGuid = new Guid("83b427e4-086f-47f3-bb06-be266abd1ca5");

		public const int elementId_leftStickX = 0;

		public const int elementId_leftStickY = 1;

		public const int elementId_rightStickX = 2;

		public const int elementId_rightStickY = 3;

		public const int elementId_actionBottomRow1 = 4;

		public const int elementId_a = 4;

		public const int elementId_actionBottomRow2 = 5;

		public const int elementId_b = 5;

		public const int elementId_actionBottomRow3 = 6;

		public const int elementId_c = 6;

		public const int elementId_actionTopRow1 = 7;

		public const int elementId_x = 7;

		public const int elementId_actionTopRow2 = 8;

		public const int elementId_y = 8;

		public const int elementId_actionTopRow3 = 9;

		public const int elementId_z = 9;

		public const int elementId_leftShoulder1 = 10;

		public const int elementId_leftBumper = 10;

		public const int elementId_leftShoulder2 = 11;

		public const int elementId_leftTrigger = 11;

		public const int elementId_rightShoulder1 = 12;

		public const int elementId_rightBumper = 12;

		public const int elementId_rightShoulder2 = 13;

		public const int elementId_rightTrigger = 13;

		public const int elementId_center1 = 14;

		public const int elementId_back = 14;

		public const int elementId_center2 = 15;

		public const int elementId_start = 15;

		public const int elementId_center3 = 16;

		public const int elementId_guide = 16;

		public const int elementId_leftStickButton = 17;

		public const int elementId_rightStickButton = 18;

		public const int elementId_dPadUp = 19;

		public const int elementId_dPadRight = 20;

		public const int elementId_dPadDown = 21;

		public const int elementId_dPadLeft = 22;

		public const int elementId_leftStick = 23;

		public const int elementId_rightStick = 24;

		public const int elementId_dPad = 25;

		IControllerTemplateButton IGamepadTemplate.actionBottomRow1 => GetElement<IControllerTemplateButton>(4);

		IControllerTemplateButton IGamepadTemplate.a => GetElement<IControllerTemplateButton>(4);

		IControllerTemplateButton IGamepadTemplate.actionBottomRow2 => GetElement<IControllerTemplateButton>(5);

		IControllerTemplateButton IGamepadTemplate.b => GetElement<IControllerTemplateButton>(5);

		IControllerTemplateButton IGamepadTemplate.actionBottomRow3 => GetElement<IControllerTemplateButton>(6);

		IControllerTemplateButton IGamepadTemplate.c => GetElement<IControllerTemplateButton>(6);

		IControllerTemplateButton IGamepadTemplate.actionTopRow1 => GetElement<IControllerTemplateButton>(7);

		IControllerTemplateButton IGamepadTemplate.x => GetElement<IControllerTemplateButton>(7);

		IControllerTemplateButton IGamepadTemplate.actionTopRow2 => GetElement<IControllerTemplateButton>(8);

		IControllerTemplateButton IGamepadTemplate.y => GetElement<IControllerTemplateButton>(8);

		IControllerTemplateButton IGamepadTemplate.actionTopRow3 => GetElement<IControllerTemplateButton>(9);

		IControllerTemplateButton IGamepadTemplate.z => GetElement<IControllerTemplateButton>(9);

		IControllerTemplateButton IGamepadTemplate.leftShoulder1 => GetElement<IControllerTemplateButton>(10);

		IControllerTemplateButton IGamepadTemplate.leftBumper => GetElement<IControllerTemplateButton>(10);

		IControllerTemplateAxis IGamepadTemplate.leftShoulder2 => GetElement<IControllerTemplateAxis>(11);

		IControllerTemplateAxis IGamepadTemplate.leftTrigger => GetElement<IControllerTemplateAxis>(11);

		IControllerTemplateButton IGamepadTemplate.rightShoulder1 => GetElement<IControllerTemplateButton>(12);

		IControllerTemplateButton IGamepadTemplate.rightBumper => GetElement<IControllerTemplateButton>(12);

		IControllerTemplateAxis IGamepadTemplate.rightShoulder2 => GetElement<IControllerTemplateAxis>(13);

		IControllerTemplateAxis IGamepadTemplate.rightTrigger => GetElement<IControllerTemplateAxis>(13);

		IControllerTemplateButton IGamepadTemplate.center1 => GetElement<IControllerTemplateButton>(14);

		IControllerTemplateButton IGamepadTemplate.back => GetElement<IControllerTemplateButton>(14);

		IControllerTemplateButton IGamepadTemplate.center2 => GetElement<IControllerTemplateButton>(15);

		IControllerTemplateButton IGamepadTemplate.start => GetElement<IControllerTemplateButton>(15);

		IControllerTemplateButton IGamepadTemplate.center3 => GetElement<IControllerTemplateButton>(16);

		IControllerTemplateButton IGamepadTemplate.guide => GetElement<IControllerTemplateButton>(16);

		IControllerTemplateThumbStick IGamepadTemplate.leftStick => GetElement<IControllerTemplateThumbStick>(23);

		IControllerTemplateThumbStick IGamepadTemplate.rightStick => GetElement<IControllerTemplateThumbStick>(24);

		IControllerTemplateDPad IGamepadTemplate.dPad => GetElement<IControllerTemplateDPad>(25);

		public GamepadTemplate(object payload)
			: base(payload)
		{
		}
	}
}
