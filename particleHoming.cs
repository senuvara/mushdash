using UnityEngine;

[ExecuteInEditMode]
public class particleHoming : MonoBehaviour
{
	[Tooltip("Target object. If this parameter is undefined it will assume the attached object itself which creates self chasing particle effect.")]
	public Transform target;

	[Tooltip("How fast the particle is guided to the closest target.")]
	public float speed = 10f;

	[Tooltip("Cap the maximum speed to prevent particle from being flung too far from the missed target.")]
	public float maxSpeed = 50f;

	[Tooltip("How long in the projectile begins being guided towards the target. Higher delay and high particle start speed requires greater distance between attacker and target to avoid uncontrolled orbitting around the target.")]
	public float homingDelay = 1f;

	private ParticleSystem m_System;

	private ParticleSystem.Particle[] m_Particles;

	private void Start()
	{
		if (target == null)
		{
			target = base.transform;
		}
		m_System = GetComponent<ParticleSystem>();
		m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
	}

	private void LateUpdate()
	{
		InitializeIfNeeded();
		int particles = m_System.GetParticles(m_Particles);
		float num = (target.position - base.transform.position).sqrMagnitude + 0.001f;
		for (int i = 0; i < particles; i++)
		{
			Vector3 a = target.position - m_Particles[i].position;
			float sqrMagnitude = a.sqrMagnitude;
			float num2 = Vector3.Dot(m_Particles[i].velocity.normalized, a.normalized);
			float d = Mathf.Abs((num - sqrMagnitude) / num) * num * (num2 + 1.001f);
			float num3 = 0f;
			num3 += Time.deltaTime / (homingDelay + 0.0001f) * 100f;
			m_Particles[i].velocity = Vector3.ClampMagnitude(Vector3.Slerp(m_Particles[i].velocity, m_Particles[i].velocity + a * speed * 0.01f * d, num3), maxSpeed);
		}
		m_System.SetParticles(m_Particles, particles);
	}

	private void InitializeIfNeeded()
	{
		if (target == null)
		{
			target = base.transform;
		}
		if (m_System == null)
		{
			m_System = GetComponent<ParticleSystem>();
		}
		if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
		{
			m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
		}
	}
}
