using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private LeaderController selectedLeader;

    private void Update()
    {
        SelectLeader();
        MoveLeader();
    }

    private void SelectLeader()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            LeaderController leader = hit.collider.GetComponent<LeaderController>();

            if (leader != null)
            {
                selectedLeader = leader;
                Debug.Log("Leader selected");
            }
        }
    }

    private void MoveLeader()
    {
        if (selectedLeader == null)
            return;

        if (!Input.GetMouseButtonDown(1))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            selectedLeader.SetTargetPoint(hit.point);
            Debug.Log("Leader moving towards: " + hit.point);
        }
    }
}
