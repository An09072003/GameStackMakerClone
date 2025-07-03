using UnityEngine;

public class NextLevelLoader : MonoBehaviour
{
    public GameObject nextLevelPrefab;   // Gán prefab của map tiếp theo
    public Transform player;             // Gán Player
    public GameObject endGameUI;         // Gán UI cần ẩn khi sang map mới

    private GameObject currentLevelInstance; // Lưu level hiện tại trong scene

    public void LoadNextLevel()
    {
        // Ẩn UI nếu có
        if (endGameUI != null)
        {
            endGameUI.SetActive(false);
        }

        // Xoá map cũ (nếu có)
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        // Spawn map mới
        if (nextLevelPrefab != null)
        {
            currentLevelInstance = Instantiate(nextLevelPrefab, Vector3.zero, Quaternion.identity);

            // Move player đến PlayerStart
            Transform startPoint = currentLevelInstance.transform.Find("PlayerStart");

            if (startPoint != null)
            {
                player.position = startPoint.position;
                player.rotation = startPoint.rotation;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy PlayerStart trong map.");
            }
        }
        else
        {
            Debug.LogError("Chưa gán prefab map mới.");
        }
    }
}
