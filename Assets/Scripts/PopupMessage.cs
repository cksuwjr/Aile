using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    public void SetText(string word)
    {
        GetComponent<Text>().text = word;
        StartCoroutine(GoUp());
    }
    IEnumerator GoUp()
    {
        RectTransform rt = GetComponent<RectTransform>();
        for (int i = 0; i < 100; i++)
        {
            rt.position = new Vector3(rt.position.x, rt.position.y + 0.5f, rt.position.z);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
