using Assets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageAlbumPointerEnter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	public GameObject parent;

	public void OnPointerEnter(PointerEventData data)
	{
		parent.GetComponent<PnlStage>().SetMouseWheelBinding(parent.GetComponent<PnlStage>().musicFancyScrollView, false);
		parent.GetComponent<PnlStage>().SetMouseWheelBinding(parent.GetComponent<PnlStage>().albumFancyScrollViewNs, true);
	}

	public void OnPointerExit(PointerEventData data)
	{
		parent.GetComponent<PnlStage>().SetMouseWheelBinding(parent.GetComponent<PnlStage>().musicFancyScrollView, true);
		parent.GetComponent<PnlStage>().SetMouseWheelBinding(parent.GetComponent<PnlStage>().albumFancyScrollViewNs, false);
	}
}
