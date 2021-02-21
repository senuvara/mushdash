using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class SerializedUIBehaviour : SerializedMonoBehaviour
	{
		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		public virtual bool IsActive()
		{
			return base.isActiveAndEnabled;
		}

		protected virtual void OnValidate()
		{
		}

		protected virtual void Reset()
		{
		}

		protected virtual void OnRectTransformDimensionsChange()
		{
		}

		protected virtual void OnBeforeTransformParentChanged()
		{
		}

		protected virtual void OnTransformParentChanged()
		{
		}

		protected virtual void OnDidApplyAnimationProperties()
		{
		}

		protected virtual void OnCanvasGroupChanged()
		{
		}

		protected virtual void OnCanvasHierarchyChanged()
		{
		}

		public bool IsDestroyed()
		{
			return this == null;
		}
	}
}
