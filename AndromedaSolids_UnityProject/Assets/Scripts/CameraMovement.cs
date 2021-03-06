﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CameraMovement : MonoBehaviour
{

	public Camera cam;
	public Transform targetShape;

	private Vector3 camPreviousPosition;
	private Vector3 dragOrigin = Vector3.zero;

	private Vector3 camOffset;

	private Vector3 camStartPosition;
	private Quaternion camStartRotation;
	private Vector3 camStartOffset;


	public float sensibilidadeZoom = 5f;
	public float sensibilidadeMovimentoCamera = 8f;
	public float sensibilidadeMovimentoSetas = 0.2f;

	private float LimiteZoomPositivo = 0;
	private float LimiteZoomNegativo = -13;

	private bool mouseIsDraggingForMovement = false;
	private bool mouseIsDraggingForRotation = false;


	// Resetar a posição da câmera
	void resetTransformations() {
		cam.transform.position = camStartPosition;
		cam.transform.rotation = camStartRotation;
		camOffset = camStartOffset;
	}

	void Start() {
		cam.transform.LookAt(targetShape);
		camOffset = new Vector3(0, 0, -Vector3.Distance(cam.transform.position, targetShape.position));

		// Definir a posição inicial
		camStartPosition = cam.transform.position;
		camStartRotation = cam.transform.rotation;
		camStartOffset = camOffset;
	}

    void Update()
    {
		bool mexendoNaUI = EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null;
		if(mexendoNaUI) {
			// Não pode ter nenhum elemento de Texto!
			mexendoNaUI = EventSystem.current.currentSelectedGameObject.GetComponent<TextMeshProUGUI>() == null;
		}

		// Calculando o zoom da câmera
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

		// Calculando o movimento da câmera com base no mouse
		Vector3 movimento = Vector3.zero;
        if( ( Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) ) && !mexendoNaUI) { // Botão direito ou do meio
			dragOrigin = cam.ScreenToViewportPoint(Input.mousePosition);
			mouseIsDraggingForMovement = true;
		}
		if(( Input.GetMouseButton(1) || Input.GetMouseButton(2) ) && (mouseIsDraggingForMovement)) { // Botão direito ou do meio
			Vector3 pos = cam.ScreenToViewportPoint(Input.mousePosition);
			movimento += ( -(new Vector3(pos.x, pos.y, 0) - dragOrigin)) * sensibilidadeMovimentoCamera;
			dragOrigin = pos;
		} else {
			mouseIsDraggingForMovement = false;
		}

		// Calculando o movimento da câmera com base no teclado
		if(Input.GetKey(KeyCode.LeftArrow)) {
			// camOffset.x -= 0.2f;
			movimento.x -= sensibilidadeMovimentoSetas;
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			// camOffset.x += 0.2f;
			movimento.x += sensibilidadeMovimentoSetas;
		}
		if(Input.GetKey(KeyCode.UpArrow)) {
			// camOffset.x -= 0.2f;
			movimento.y += sensibilidadeMovimentoSetas;
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			// camOffset.x += 0.2f;
			movimento.y -= sensibilidadeMovimentoSetas;
		}
		
		// Aplicando o movimento
		if(movimento != Vector3.zero) {
			camOffset += movimento;
			cam.transform.Translate(movimento);
		}

		// Calculando a movimentação da câmera
        if(Input.GetMouseButtonDown(0) && !mexendoNaUI) { // Botão esquerdo
			camPreviousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
			mouseIsDraggingForRotation = true;
		}
		if(Input.GetMouseButton(0) && mouseIsDraggingForRotation) { // Botão esquerdo
			Vector3 direction = camPreviousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

			cam.transform.position = targetShape.position;
			cam.transform.Rotate(Vector3.right, direction.y * 180);
			cam.transform.Rotate(Vector3.up, -direction.x * 180, Space.World);
			cam.transform.Translate(camOffset);

			camPreviousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
		} else {
			mouseIsDraggingForRotation = false;
		}

		// Resetando a câmera com <espaço>
		if(Input.GetKey(KeyCode.Space)) {
			resetTransformations();
		}
    }
}
