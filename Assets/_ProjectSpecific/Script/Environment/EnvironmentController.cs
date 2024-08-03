using UnityEngine;

public class EnvironmentController : Singleton<EnvironmentController>
{
    [SerializeField] private ParticleSystem[] m_ConfettiParticle;

    public void PlayConfetti()
    {
        foreach (var item in m_ConfettiParticle)
        {
            item.Play();
        }
    }
}
