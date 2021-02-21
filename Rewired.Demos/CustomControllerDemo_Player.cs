using UnityEngine;

namespace Rewired.Demos
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(CharacterController))]
	public class CustomControllerDemo_Player : MonoBehaviour
	{
		public int playerId;

		public float speed = 1f;

		public float bulletSpeed = 20f;

		public GameObject bulletPrefab;

		private Player _player;

		private CharacterController cc;

		private Player player
		{
			get
			{
				if (_player == null)
				{
					_player = ReInput.players.GetPlayer(playerId);
				}
				return _player;
			}
		}

		private void Awake()
		{
			cc = GetComponent<CharacterController>();
		}

		private void Update()
		{
			if (ReInput.isReady)
			{
				Vector2 a = new Vector2(player.GetAxis("Move Horizontal"), player.GetAxis("Move Vertical"));
				cc.Move(a * speed * Time.deltaTime);
				if (player.GetButtonDown("Fire"))
				{
					Vector3 b = Vector3.Scale(new Vector3(1f, 0f, 0f), base.transform.right);
					GameObject gameObject = Object.Instantiate(bulletPrefab, base.transform.position + b, Quaternion.identity);
					Rigidbody component = gameObject.GetComponent<Rigidbody>();
					float num = bulletSpeed;
					Vector3 right = base.transform.right;
					component.velocity = new Vector3(num * right.x, 0f, 0f);
				}
				if (player.GetButtonDown("Change Color"))
				{
					Renderer component2 = GetComponent<Renderer>();
					Material material = component2.material;
					material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
					component2.material = material;
				}
			}
		}
	}
}
