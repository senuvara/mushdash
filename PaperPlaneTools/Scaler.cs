using UnityEngine;

namespace PaperPlaneTools
{
	[ExecuteInEditMode]
	public class Scaler : MonoBehaviour
	{
		public float maxWidth = 1f;

		public float maxHeight = 1f;

		private void Update()
		{
			RectTransform component = GetComponent<RectTransform>();
			RectTransform rectTransform = (!(base.transform.parent != null)) ? null : base.transform.parent.GetComponent<RectTransform>();
			float a = 1f;
			float width = component.rect.width;
			float num = (!(rectTransform != null)) ? 0f : rectTransform.rect.width;
			if (width > 0f)
			{
				a = Mathf.Min(1f, num * maxWidth / width);
			}
			float b = 1f;
			float height = component.rect.height;
			float num2 = (!(rectTransform != null)) ? 0f : rectTransform.rect.height;
			if (width > 0f)
			{
				b = Mathf.Min(1f, num2 * maxHeight / height);
			}
			float num3 = Mathf.Min(a, b);
			base.transform.localScale = new Vector3(num3, num3, 1f);
		}
	}
}
