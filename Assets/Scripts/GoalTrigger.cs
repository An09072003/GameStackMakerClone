using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public ParticleSystem[] effects;  // Mảng các hiệu ứng

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var ps in effects)
            {
                if (ps != null)
                ps.Play();
            }

            Debug.Log("Player đã về đích!");
        }
    }
}
