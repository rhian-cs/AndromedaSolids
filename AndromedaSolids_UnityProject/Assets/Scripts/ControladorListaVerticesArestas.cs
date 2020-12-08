using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorListaVerticesArestas : MonoBehaviour
{
	public Canvas canvas;
	public Color textColor = new Color(255, 255, 255);
	public Camera cam;

	private GeneratedMeshFromJSON meshScript;
	private List<string> labels; // Lista com os nomes de cada vértice
	private List<GameObject> vertexObjs;

	public List<GameObject> textGameObjects = new List<GameObject>();

	GameObject CreateText(Vector2 txtPos, string textToPrint, int fontSize = 36, string GOName = "DynamicTextTMP") {
		// Criando GameObject vazio
		GameObject UItextGO = new GameObject(GOName);
		UItextGO.transform.SetParent(canvas.transform);
		UItextGO.layer = LayerMask.NameToLayer("Ignore Raycast");

		// Criando transormação
		RectTransform trans = UItextGO.AddComponent<RectTransform>();
		trans.anchoredPosition = txtPos; // new Vector2(x, y);

		// Alterando o parent novamente
		// UItextGO.transform.SetParent(transform, true);

		TextMeshProUGUI text = UItextGO.AddComponent<TextMeshProUGUI>();
		// Text text = UItextGO.AddComponent<Text>();
		text.text = textToPrint;
		text.fontSize = fontSize / canvas.scaleFactor;
		text.color = textColor;

		return UItextGO;
	}

    // Start is called before the first frame update
    void Start()
    {
		meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();
		labels = meshScript.labels;
		vertexObjs = meshScript.vertexObjs;

    }

	public void inicializarTextos() {
		if(labels != null && vertexObjs != null) {
			for(int i = 0; i < textGameObjects.Count; i++) {
				GameObject.Destroy(textGameObjects[i]);
			}
			textGameObjects.Clear();

			for(int i = 0; i < labels.Count; i++) {					
				textGameObjects.Add(CreateText(getText2DPosition(vertexObjs[i].transform.position), labels[i], 24, "Text_" + labels[i]));
			}
		}
	}

	Vector2 getText2DPosition(Vector3 position3D) {
		Vector2 pos0 = cam.WorldToScreenPoint(position3D);
		float scaleFactor = canvas.scaleFactor;
		Vector2 txtPosition = new Vector2((pos0.x - 35*Screen.width/80) / scaleFactor, (pos0.y - Screen.height/2) / scaleFactor);
		return txtPosition;
	}

	void Update() {
		// if(textGameObjects[0].activeSelf) {
		for(int i = 0; i < labels.Count; i++) {					
			textGameObjects[i].GetComponent<RectTransform>().anchoredPosition = getText2DPosition(vertexObjs[i].transform.position);
			// textGameObjects[i].GetComponent<TextMeshProUGUI>().fontSize = 36 / canvas.scaleFactor;
		}
		// }
	}
}
