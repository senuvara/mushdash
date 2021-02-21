using Rewired.UI;
using System.Text;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	public class PlayerPointerEventData : PointerEventData
	{
		public int playerId
		{
			get;
			set;
		}

		public int inputSourceIndex
		{
			get;
			set;
		}

		public IMouseInputSource mouseSource
		{
			get;
			set;
		}

		public ITouchInputSource touchSource
		{
			get;
			set;
		}

		public PointerEventType sourceType
		{
			get;
			set;
		}

		public int buttonIndex
		{
			get;
			set;
		}

		public PlayerPointerEventData(EventSystem eventSystem)
			: base(eventSystem)
		{
			playerId = -1;
			inputSourceIndex = -1;
			buttonIndex = -1;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Player Id</b>: " + playerId);
			stringBuilder.AppendLine("<b>Mouse Source</b>: " + mouseSource);
			stringBuilder.AppendLine("<b>Input Source Index</b>: " + inputSourceIndex);
			stringBuilder.AppendLine("<b>Touch Source/b>: " + touchSource);
			stringBuilder.AppendLine("<b>Source Type</b>: " + sourceType);
			stringBuilder.AppendLine("<b>Button Index</b>: " + buttonIndex);
			stringBuilder.Append(base.ToString());
			return stringBuilder.ToString();
		}
	}
}
