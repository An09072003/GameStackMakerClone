using UnityEngine;
using System.Collections;

public class GridMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public float checkDistance = 1f;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Transform visual; // phần hiển thị nhân vật (con của player)

    private float stackCount = 0;
    private float objectHigh = 0.3f;
    private bool isSliding = false;
    private Vector3 moveDirection;

    public bool canMove = true;
    public float StackCount => stackCount;

    private void Update()
    {
        if (!canMove || isSliding) return;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector3.forward;
        else if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector3.back;
        else if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector3.right;
        else return;

        StartCoroutine(SlideUntilBlocked());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            other.tag = "Untagged";

            Transform t = other.transform;
            t.SetParent(transform);
            t.localPosition = new Vector3(0, stackCount * objectHigh, 0);
            stackCount++;

            // Tăng visual lên 0.3f
            Vector3 visPos = visual.localPosition;
            visPos.y += 0.3f;
            visual.localPosition = visPos;
        }

        if (other.CompareTag("EmptyPipe") && stackCount > 0)
        {
            other.tag = "Untagged";

            Transform lastBrick = transform.GetChild(transform.childCount - 1);
            lastBrick.SetParent(null);

            Vector3 dropStart = other.transform.position + Vector3.up * 1f;
            if (Physics.Raycast(dropStart, Vector3.down, out RaycastHit hit, 5f, groundLayer))
            {
                float gridX = Mathf.Round(other.transform.position.x);
                float gridZ = Mathf.Round(other.transform.position.z);
                lastBrick.position = new Vector3(gridX, hit.point.y + 0.01f, gridZ);
            }
            else
            {
                Destroy(lastBrick.gameObject);
            }

            stackCount--;

            // Hạ visual xuống 0.35f nhưng không thấp hơn 0
            Vector3 visPos = visual.localPosition;
            visPos.y = Mathf.Max(0, visPos.y - 0.3f);
            visual.localPosition = visPos;
        }

        if (other.CompareTag("FinishLine"))
        {
            canMove = false;
            gameManager.GameOver();
        }
    }

    private void CheckNearbyEmptyPipes()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, 1.1f);

        foreach (Collider col in nearby)
        {
            if (col.CompareTag("EmptyPipe"))
            {
                col.gameObject.layer = (stackCount > 0) ? LayerMask.NameToLayer("Ground") : LayerMask.NameToLayer("Wall");
            }
        }
    }

    private IEnumerator SlideUntilBlocked()
    {
        isSliding = true;

        while (true)
        {
            Vector3 nextPos = transform.position + moveDirection * checkDistance;

            Vector3 rayStart = transform.position + Vector3.up * 0.1f;
            if (Physics.Raycast(rayStart, moveDirection, out RaycastHit hitInfo, checkDistance))
            {
                if (((1 << hitInfo.collider.gameObject.layer) & obstacleLayer) != 0)
                    break;
            }

            Vector3 groundCheckStart = nextPos + Vector3.up * 2f;
            if (!Physics.Raycast(groundCheckStart, Vector3.down, 5f, groundLayer))
                break;

            Vector3 targetPos = new Vector3(nextPos.x, transform.position.y, nextPos.z);
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            CheckNearbyEmptyPipes();

            yield return null;
        }

        isSliding = false;
    }
}
