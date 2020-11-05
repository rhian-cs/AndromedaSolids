using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorListaSolidos : MonoBehaviour {
	private List<string> listaSolidos;
	public int indiceSolido = 0;
	public int facesPrisma = 3;

    void Start() {
        listaSolidos = new List<string>();

		listaSolidos.Add("cubo.json");
		listaSolidos.Add("retangular.json");
		listaSolidos.Add("piramide_quadrada.json");
		listaSolidos.Add("octaedro.json");
		listaSolidos.Add("tetraedro.json");

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

	void atualizarSolido() {
		GeneratedMeshFromJSON meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();

		if(indiceSolido == -1) {
			meshScript.facesPrisma = facesPrisma;
			meshScript.ativarPrisma = true;
		} else {
			meshScript.ativarPrisma = false;
			facesPrisma = 3;
			meshScript.facesPrisma = facesPrisma;
			meshScript.fileName = listaSolidos[indiceSolido];
		}
		
		meshScript.inicializarObjeto();
	}
}
