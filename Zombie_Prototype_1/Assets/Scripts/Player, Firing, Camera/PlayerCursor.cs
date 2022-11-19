using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCursor : MonoBehaviour
{
    public GameObject hitMarker;

    public float removeTime;
    public float zDistance;

    public void ShowHitMarker(Vector3 hitPos)
    {
        hitMarker.SetActive(true);
        hitMarker.transform.position = new Vector3(hitPos.x, hitPos.y, hitPos.z - zDistance);
        StartCoroutine(WaitToRemoveHitMarker());
    }

    public void RemoveHitMarker()
    {
        hitMarker.SetActive(false);
    }

    IEnumerator WaitToRemoveHitMarker()
    {
        yield return new WaitForSeconds(removeTime);
        RemoveHitMarker();
    }
}
