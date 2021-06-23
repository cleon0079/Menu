using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField, Min(.1f)] float cameraDistance = .1f;
    [SerializeField, Range(.05f, .2f)] float damping = .1f;
    [SerializeField] Transform target;
    [SerializeField] Transform player;
    [SerializeField, Min(.5f)] float minDistanceToPlayer = .5f;

    new Camera camera;

    Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();

        transform.localPosition = new Vector3(0, 0, -cameraDistance);
    }

    // Update is called once per frame
    void Update()
    {
        // Update the camera's position based on the distance.
        // This will give us a smooth zoom effect for transitions.
        Vector3 newPos = Vector3.SmoothDamp(
            transform.position, CalculatePos(), ref velocity, damping);

        transform.position = newPos;
    }

    public bool CanRotate()
    {
        return Vector3.Distance(transform.position, player.position) > minDistanceToPlayer;
    }

    Vector3 CalculatePos()
    {
        Vector3 newPos = target.position - target.forward * cameraDistance;

        // Calculate Actual end point
        Vector3 direction = -transform.forward;

        // Cast a ray from the target to the camera with the length as the distance
        if (Physics.Raycast(target.position, direction, out RaycastHit hit, cameraDistance))
        {
            newPos = hit.point;
        }

        return newPos;
    }
}
