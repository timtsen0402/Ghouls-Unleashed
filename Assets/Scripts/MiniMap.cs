using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class MiniMap : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 newPos = player.transform.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
        transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0f);
    }
}
