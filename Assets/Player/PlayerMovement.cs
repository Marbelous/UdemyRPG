using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;


// TODO:  Fix issue with click to move and WASD conflicting

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    bool isInDirectMode = false; //TODO: Consider static

    [SerializeField] float walkStopRadius = .2f;
    [SerializeField] float attackStopRadius = 5f;
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;

    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isInDirectMode = !isInDirectMode;
            currentDestination = transform.position;

        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement(); // Mouse based movement
        }
    }

    private void ProcessDirectMovement()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        // calculate camera relative direction to move:
        var camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        var camMove = v * camForward + h * Camera.main.transform.right;
        thirdPersonCharacter.Move(camMove, false, false);
    }

    // TODO:  msg text does not update canvas in WASD mode?
    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            print("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString());

            clickPoint = cameraRaycaster.hit.point;

            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkStopRadius);  // So not set in default case
                    break;
                case Layer.Explorable:
                    print("This looks interesting...");
                    break;
                case Layer.Enemy:
                    print("Enemy targeted!");
                    currentDestination = ShortDestination(clickPoint, attackStopRadius);  // So not set in default case
                    break;
                case Layer.Ignored:
                    print("Nah.");
                    break;
                default:
                    print("Uknown click target");
                    return;
            }
        }

        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0.08f)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos()
    {
        print("Gizmo draw");
        Gizmos.DrawLine(transform.position, clickPoint);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackStopRadius);


    }
}

