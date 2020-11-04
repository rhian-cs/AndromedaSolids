using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorListaSolidos : MonoBehaviour {
	private List<string> listaSolidos;
	public int indiceSolido = 0;
	public int facesPrisma = 0;

    // Start is called before the first frame update
    void Start() {
        listaSolidos = new List<string>();
		listaSolidos.Add("cubo.json");
		listaSolidos.Add("piramide_triangular.json");
    }

	void atualizarSolido() {
		GeneratedMeshFromJSON meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();

		meshScript.facesPrisma = facesPrisma;
		meshScript.fileName = listaSolidos[indiceSolido];
		
		meshScript.inicializarObjeto();
	}
}
