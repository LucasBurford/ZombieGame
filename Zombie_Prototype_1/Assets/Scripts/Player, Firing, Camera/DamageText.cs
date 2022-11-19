using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public GameObject damageText;
    public TextMeshProUGUI text;

    public float removeTime;
    public float zDistance;
    public float yDistance;

    private void Start()
    {
        text = GameObject.Find("DamageText").GetComponent<TextMeshProUGUI>();
    }

    public void ShowDamageNumber(Vector3 hitPos, Quaternion rotation, float damage)
    {
        damageText.SetActive(true);
        text.text = damage.ToString();
        damageText.transform.localRotation = new Quaternion(rotation.x, rotation.y *= -1, rotation.z, 1);
        damageText.transform.position = new Vector3(hitPos.x, hitPos.y + yDistance, hitPos.z);
        StartCoroutine(WaitToRemoveHitMarker());
    }

    public void RemoveDamageNumber()
    {
        damageText.SetActive(false);
    }

    IEnumerator WaitToRemoveHitMarker()
    {
        yield return new WaitForSeconds(removeTime);
        RemoveDamageNumber();
    }
}
