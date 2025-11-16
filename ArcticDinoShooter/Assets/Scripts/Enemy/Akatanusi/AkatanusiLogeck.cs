
using UnityEngine;

public class AkatanusiLogeck : MonoBehaviour
{
    [SerializeField] private float _increaseEating;
    [SerializeField] private GameObject _babyPrefab;
    [SerializeField] private Transform safePlace;

    private bool isEating = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Eat"))
        {
            isEating = true;
            if (isEating)
            {
                Feeding(_increaseEating);
            }
        }
    }

    private void Feeding(float size)
    {
        Vector3 newScale = this.gameObject.transform.localScale;
        newScale.x += size;
        newScale.y += size;
        newScale.z += size;
        this.gameObject.transform.localScale = newScale;

        if (newScale.x >= 2f && newScale.y >= 2f && newScale.z >= 2f)
        {
            Instantiate(_babyPrefab, this.transform.position, Quaternion.identity);

            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

   
}
