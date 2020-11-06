using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GerenciadorListaSolidos : MonoBehaviour {
	private List<string> listaSolidos;
	public int indiceSolido = 0;
	public int facesPrisma = 3;

	public GameObject inputParentGO;
	public TMP_InputField inputFacesPrisma;

	private GeneratedMeshFromJSON meshScript;

    void Start() {
        listaSolidos = new List<string>();

		listaSolidos.Add("cubo.json");
		listaSolidos.Add("retangular.json");
		listaSolidos.Add("piramide_quadrada.json");
		listaSolidos.Add("octaedro.json");
		listaSolidos.Add("tetraedro.json");

		// inputFacesPrisma = inputParentGO.GetComponent<TMP_InputField>();
		meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();

		atualizarSolido();
    }

	public void deslocarParaEsquerda() {
		indiceSolido--;

		if(indiceSolido < -1) {
			indiceSolido = listaSolidos.Count - 1;
		}

		Debug.Log("Índice--. Valor = " + indiceSolido);
		atualizarSolido();
	}
	public void deslocarParaDireita() {
		indiceSolido++;
		
		if(indiceSolido > listaSolidos.Count - 1) {
			indiceSolido = -1;
		}

		Debug.Log("Índice++. Valor = " + indiceSolido);
		atualizarSolido();
	}

	public void atualizarEntradaPrisma() {
		int ultimoFacesPrisma = facesPrisma;
		string entradaInputText = inputFacesPrisma.text;

		if(!int.TryParse(entradaInputText, out facesPrisma)) {
			facesPrisma = 0;
		}

		if(ultimoFacesPrisma != facesPrisma) {
			atualizarSolido();
		}
	}

	void atualizarSolido() {
		if(indiceSolido == -1) { // Prisma
			meshScript.ativarPrisma = true;
		} else {
			meshScript.ativarPrisma = false;

			facesPrisma = 3;
			meshScript.fileName = listaSolidos[indiceSolido];
		}

		meshScript.facesPrisma = facesPrisma;
		inputParentGO.SetActive(meshScript.ativarPrisma);
		
		meshScript.inicializarObjeto();
	}
}
