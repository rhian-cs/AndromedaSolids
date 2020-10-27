using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	public Camera cam;
	public Transform targetShape;

	private Vector3 camPreviousPosition;
	private Vector3 dragOrigin;

	private Vector3 camOffset;

	private Vector3 camStartPosition;
	private Quaternion camStartRotation;
	private Vector3 camStartOffset;


	private float sensibilidadeZoom = 5f;

	private float LimiteZoomPositivo = 0;
	private float LimiteZoomNegativo = -18;


	void resetTransformations() {
		cam.transform.position = camStartPosition;
		cam.transform.rotation = camStartRotation;
		camOffset = camStartOffset;
	}

	void Start() {
		cam.transform.LookAt(targetShape);
		camOffset = new Vector3(0, 0, -Vector3.Distance(cam.transform.position, targetShape.position));

		camStartPosition = cam.transform.position;
		camStartRotation = cam.transform.rotation;
		camStartOffset = camOffset;
	}

    void Update()
    {

		float zoom = Input.GetAxisRaw("Mouse ScrollWheel");
		if(zoom != 0) {
			camOffset.z += zoom * sensibilidadeZoom;

			if(camOffset.z > LimiteZoomPositivo) {
				camOffset.z = LimiteZoomPositivo;
				zoom = 0;
			}
			if(camOffset.z < LimiteZoomNegativo) {
				camOffset.z = LimiteZoomNegativo;
				zoom = 0;
			}
			
			cam.transform.Translate(0, 0, zoom * sensibilidadeZoom);
		}

        /*if(Input.GetMouseButtonDown(1)) { // Botão direito
			dragOrigin = cam.ScreenToViewportPoint(Input.mousePosition);
			Debug.Log(dragOrigin);
		}
		
		if(Input.GetMouseButton(1)) { // Botão direito
			Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition) - dragOrigin;
			Vector3 move = new Vector3(pos.x, pos.y, 0);
			

		}*/


        if(Input.GetMouseButtonDown(0)) { // Botão esquerdo
			camPreviousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
		}

		if(Input.GetMouseButton(0)) { // Botão esquerdo
			Vector3 direction = camPreviousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

			cam.transform.position = targetShape.position;
			cam.transform.Rotate(Vector3.right, direction.y * 180);
			cam.transform.Rotate(Vector3.up, -direction.x * 180, Space.World);
			cam.transform.Translate(camOffset);

			camPreviousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
		}

		if(Input.GetKey(KeyCode.Space)) {
			resetTransformations();
		}

		
    }
}
