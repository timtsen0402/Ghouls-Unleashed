using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Minimap : MonoBehaviour
{
    public GameObject player;

    private void LateUpdate()
    {
        Vector3 newPos = player.transform.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
        transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0f);
    }
}
