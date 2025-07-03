using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject endGameUI;

    private bool isGameOver = false;

    private void Start()
    {
        if (endGameUI != null)
            endGameUI.SetActive(false); // Tắt UI lúc bắt đầu
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (endGameUI != null)
            endGameUI.SetActive(true); // Bật UI khi game kết thúc
    }

    public void HideEndGameUI()
    {
        if (endGameUI != null)
            endGameUI.SetActive(false); // Dùng nếu cần ẩn UI từ nơi khác
    }
}
