using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 100f, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}