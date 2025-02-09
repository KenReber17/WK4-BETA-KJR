using UnityEngine;

public class ForceLift : MonoBehaviour
{
    public Camera mainCamera; // If not using Camera.main, you can assign this in the inspector
    private GameObject liftedObject;
    private RaycastHit hit;

    // Public variable for raycast length
    public float rayLength = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On initial click
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}, Tag: {hit.collider.tag}");
                if (hit.collider.CompareTag("Liftable"))
                {
                    liftedObject = hit.collider.gameObject;
                    if (liftedObject != null)
                    {
                        Rigidbody rb = liftedObject.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            // Make the body kinematic to hold it in place
                            rb.isKinematic = true;
                            rb.useGravity = false;
                            
                            // Lift the object
                            Vector3 newPosition = new Vector3(liftedObject.transform.position.x, 
                                                              liftedObject.transform.position.y + 20f, 
                                                              liftedObject.transform.position.z);
                            rb.MovePosition(newPosition);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
        
        if (Input.GetMouseButtonUp(0) && liftedObject != null) // On button release
        {
            Rigidbody rb = liftedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Re-enable gravity and physics interaction
                rb.useGravity = true;
                rb.isKinematic = false;
                rb.linearVelocity = Vector3.zero; // Ensure it starts falling from rest
                liftedObject = null;
            }
        }
    }
}