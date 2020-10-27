using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	[SerializeField] private Camera cam = null;

	private Vector3 previousPosition;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) { // Botão esquerdo
			previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
		}

		if(Input.GetMouseButton(0)) { // Botão esquerdo
			Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

			cam.transform.position = new Vector3();
			cam.transform.Rotate(new Vector3(1,0,0), direction.y * 180);
			cam.transform.Rotate(new Vector3(0,1,0), -direction.x * 180);

			previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
		}
    }
}
