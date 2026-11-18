
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Update()
    {
        if (transform.position.y < -6f)
        {
            gameManager.FailLevel();
        }
    }
}
