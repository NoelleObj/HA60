using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script deals with detection and movement simultaneously. The resetDetection() function
// is triggered with R, but you could run that function somewhere else, like if a timer goes up, 
// similar to Metal Gear.

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public bool showPatrolPointsInGame = true;

    public List<Transform> patrolPoints;
    public Color patrolPointColor = Color.red; // The color for the patrol points
    public GameObject patrolPointPrefab; // Assign your patrol point prefab here
    public float waitTimeAtPatrolPointFallbackValue = 2f; // Default wait time if the patrol point doesn't have the proper script on it
    public float turnSpeed = 5f;  // turn speed when reaching a patrol point.

    public float rayMaxDistance = 10f; // Maximum distance for ray detection
    public float raySpreadAngle = 45f; // Total angle spread for rays
    public int rayCount = 10; // Number of rays to shoot
    public LayerMask detectionLayerMask; // LayerMask for detecting the player

    private int currentPatrolIndex = -1; // Set to -1 because it starts out with incrementing, so it actually starts at 0
    private bool isPlayerDetected;
    private bool isWaiting;
    private bool isAlert;
    private bool canPatrol; // Flag to determine if patrolling is possible

    // Layer mask to ignore enemy layer
    private int layerMask;

    void Start()
    {

        //Hide or Show patrol points on game start.
        foreach (Transform patrolPoint in patrolPoints)
        {
            if(patrolPoint != null)
            {
                PatrolPoint patrolPointComponent = patrolPoint.GetComponent<PatrolPoint>();
                if (showPatrolPointsInGame)
                {
                    patrolPointComponent.Show(); // Show patrol point
                }
                else
                {
                    patrolPointComponent.Hide(); // Hide patrol point
                }
            }
        }
        canPatrol = patrolPoints.Count > 0; // Determine if there are patrol points
        
        //set navmesh agent and start patrol
        agent = GetComponent<NavMeshAgent>();
        GoToNextPatrolPoint();

        // Set up the layer mask to ignore the enemy layer
        int enemyLayer = LayerMask.NameToLayer("Enemy"); // Change "Enemy" to your actual enemy layer name
        layerMask = ~(1 << enemyLayer); // Invert the mask to ignore the enemy layer
    }

    void Update()
    {
        if (isAlert)
        {
            agent.destination = player.position;
        }
        else 
        {
            if (!isPlayerDetected)
            {
                PerformRayDetection();
                if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.1f && canPatrol)
                {
                    StartCoroutine(WaitAndRotateAtPoint());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            resetDetection();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0 || !canPatrol ) {return;} // cancel function if no patrol points are set.
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count; // Increment current patrol point after waiting
    
        if (patrolPoints[currentPatrolIndex] == null) {
            Debug.Log("empty patrol point item in list, skipping item.");
            GoToNextPatrolPoint(); // Skip to the next patrol point
            return;
        } // cancel function if patrol point is null.
        agent.destination = patrolPoints[currentPatrolIndex].position;
    }

    private IEnumerator WaitAndRotateAtPoint()
    {
        isWaiting = true;

        // Check if currentPatrolIndex is valid
        if (currentPatrolIndex < 0 || currentPatrolIndex >= patrolPoints.Count) 
        {
            Debug.LogWarning("Current patrol index is out of bounds. Returning to patrol.");
            isWaiting = false; // Reset waiting state
            GoToNextPatrolPoint(); // Move to the next patrol point
            yield break; // Exit the coroutine
        }

        Transform currentPatrolPoint = patrolPoints[currentPatrolIndex];
        PatrolPoint patrolPointScript = patrolPoints[currentPatrolIndex].GetComponent<PatrolPoint>();
        
        if(currentPatrolPoint!=null)
        {
            // Rotate the patrolling enemy in the direction that the patrol point is facing, on the Z axis
            Vector3 patrolForward = currentPatrolPoint.forward * 100;
            // Set a virtual point 100 units ahead on the current patrol point's forward axis as the look at target
             Vector3 virtualLookTarget = currentPatrolPoint.position + patrolForward;

            // Debug log to verify current patrol point and virtual look target
            // Debug.Log($"Current Patrol Point: {patrolPoints[currentPatrolIndex].position}");
            // Debug.Log($"Virtual Look Target Position: {virtualLookTarget}");

            // Smoothly rotate to face the virtual point
            yield return StartCoroutine(RotateTowardsPoint(virtualLookTarget));

        }

        // Wait for the defined time at the patrol point, or use fallback wait time (2 seconds by default, changeable in enemy inspector)
        if (patrolPointScript != null)
        {
            yield return new WaitForSeconds(patrolPointScript.waitTime); // Get the wait time from the PatrolPoint script on the point's GameObject
        }
        else
        {
            yield return new WaitForSeconds(waitTimeAtPatrolPointFallbackValue); // Use the fallback wait time if PatrolPoint script is not found
        }

        isWaiting = false;
        GoToNextPatrolPoint(); // Now call this to move to the next point
    }

    private IEnumerator RotateTowardsPoint(Vector3 targetPosition)
    {
        // Get the direction to the virtual target point (ignoring Y-axis to keep it horizontal)
        Vector3 directionToLook = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToLook.x, 0, directionToLook.z));

        // Rotate smoothly until facing the target point
        while (Quaternion.Angle(transform.rotation, lookRotation) > 1f) // Consider the rotation "complete" when it's close (e.g. within 1 degree)
        {
            // Rotate faster by adjusting turnSpeed multiplier
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
            yield return null; // Wait until the next frame
        }

        // Snap to the exact rotation at the end to avoid minor inaccuracies.
        transform.rotation = lookRotation;
    }


    // Perform raycast detection in front of the enemy
    private void PerformRayDetection()
    {
        Vector3 forward = transform.forward;
        float halfSpread = raySpreadAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = Mathf.Lerp(-halfSpread, halfSpread, (float)i / (rayCount - 1));
            Vector3 rayDirection = Quaternion.Euler(0, angleOffset, 0) * forward;

            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit hit;

            Debug.DrawRay(transform.position, rayDirection * rayMaxDistance, Color.green);

            if (Physics.Raycast(ray, out hit, rayMaxDistance, layerMask))
            {
                Debug.DrawRay(transform.position, rayDirection * rayMaxDistance, Color.red);

                if (hit.collider.CompareTag("Player"))
                {
                    player = hit.transform; // The enemy's "player" global variable is assigned here.
                    isPlayerDetected = true;
                    isAlert = true;
                    Debug.Log("Player detected!");
                    break; // Exit once player is detected
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform; // The enemy's "player" global variable is assigned here.
            isPlayerDetected = true;
            isAlert = true; // Enter alert mode
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerDetected = false;
        }
    }

    private void resetDetection()
    {
        isAlert = false; // Exit alert mode
        isPlayerDetected = false;
        GoToNextPatrolPoint(); // Resume patrolling
    }


    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    // FUNCTIONS FOR PATROL POINT CREATION TOOLS
    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    // Method to change the colors of existing patrol points
    public void UpdatePatrolPointColors()
    {
        foreach (var patrolPoint in patrolPoints)
        {
            Renderer patrolPointRenderer = patrolPoint.GetChild(0).GetComponent<Renderer>();
            if (patrolPointRenderer != null)
            {
                // Set the patrol point's child quad's color to the enemy's patrolPointColor
                patrolPointRenderer.sharedMaterial.color = patrolPointColor; // Use sharedMaterial instead of material.
            }
        }
    }
}
