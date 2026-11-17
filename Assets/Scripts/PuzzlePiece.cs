using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private float followSpeed = 10f;

    [Header("Target Snapping")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private float snapAngle = 10f;
    [SerializeField] private PuzzlePiece[] neighbors;

    private Rigidbody rb;
    private Camera cam;
    private bool isHeld = false;

    public bool IsSnapped { get; private set; }

    public void ForceSnap()
    {
        if (!targetPoint) return;

        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;

        IsSnapped = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.layer = LayerMask.NameToLayer("SnappedPiece");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        PuzzleManager.Instance.RegisterPiece(this);
    }

    private void Update()
    {
        if (IsSnapped)
            return;

        if (Input.GetMouseButtonDown(0))
            TryPickUp();

        if (Input.GetMouseButtonUp(0) && isHeld)
            Drop();

        if (isHeld)
            FollowMouse();

        TrySnapToTarget();
    }

    private void TryPickUp()
    {
        if (isHeld || IsSnapped) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            isHeld = true;
            rb.isKinematic = true;
            transform.rotation = targetPoint.rotation;
            OrbitCamera.IsInteractingWithPiece = true;
        }
    }

    private void Drop()
    {
        isHeld = false;
        rb.isKinematic = false;
        OrbitCamera.IsInteractingWithPiece = false;
    }

    private void FollowMouse()
    {
        Plane plane = new Plane(Vector3.up, targetPoint.position);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float hitDist))
        {
            Vector3 hitPoint = ray.GetPoint(hitDist);
            Vector3 targetPos = hitPoint;
            targetPos.y = targetPoint.position.y;

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }
    }

    private void TrySnapToTarget()
    {
        if (!targetPoint || IsSnapped) return;

        float distance = Vector3.Distance(transform.position, targetPoint.position);
        float angle = Quaternion.Angle(transform.rotation, targetPoint.rotation);
        bool hasSnappedNeighbor = neighbors.Any(n => n && n.IsSnapped);

        if (distance <= snapDistance && angle <= snapAngle && hasSnappedNeighbor)
        {
            ForceSnap();
            PuzzleManager.Instance.CheckPuzzleComplete();
            OrbitCamera.IsInteractingWithPiece = false;
        }
    }
}
