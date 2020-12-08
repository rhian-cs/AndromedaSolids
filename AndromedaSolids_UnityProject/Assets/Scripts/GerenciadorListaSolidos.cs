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
		// Criando e adicionando os sólidos na lista com seus respectivos nomes de arquivo JSON
        listaSolidos = new List<string>();

		listaSolidos.Add("cubo.json");
		listaSolidos.Add("retangular.json");
		listaSolidos.Add("piramide_quadrada.json");
		listaSolidos.Add("octaedro.json");
		listaSolidos.Add("tetraedro.json");

		// inputFacesPrisma = inputParentGO.GetComponent<TMP_InputField>();
		meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();

		// atualizarSolido();
    }

	// Funções de deslocamento invocadas pelas setas ao lado do nome do sólido
	public void deslocarParaEsquerda() {
		indiceSolido--;

		if(indiceSolido < -1) {
			indiceSolido = listaSolidos.Count - 1;
		}
		atualizarSolido();
	}
	public void deslocarParaDireita() {
		indiceSolido++;
		
		if(indiceSolido > listaSolidos.Count - 1) {
			indiceSolido = -1;
		}
		atualizarSolido();
	}

	// Função chamada quando o usuário altera o valor no InputField do número de lados do prisma
	public void atualizarEntradaPrisma() {
		int ultimoFacesPrisma = facesPrisma;
		string entradaInputText = inputFacesPrisma.text;

		// Checando se o usuário de fato digitou um valor int, caso contrário, default é 0.
		if(!int.TryParse(entradaInputText, out facesPrisma)) {
			facesPrisma = 0;
		}

		// Caso o valor não mude, não é necessário atualizar o sólido
		if(ultimoFacesPrisma != facesPrisma) {
			atualizarSolido();
		}
	}

	// Funções chamadas pelo botão de + e - ao lado do InputField do número de lados do prisma
	public void aumentarLadosEntradaPrisma() {
		facesPrisma++;
		inputFacesPrisma.text = facesPrisma + "";
		atualizarSolido();
	}
	public void reduzirLadosEntradaPrisma() {
		if(facesPrisma > 0) {
			facesPrisma--;
			inputFacesPrisma.text = facesPrisma + "";
			atualizarSolido();
		}		
	}

	public void definirSolidoPorIndice(int indice) {
		if(indiceSolido >= -1 && indiceSolido < listaSolidos.Count) {
			if(indice == -1) {
				facesPrisma = 3;
				inputFacesPrisma.text = facesPrisma + "";
			}

			indiceSolido = indice;
			atualizarSolido();

			GameObject.Find("GerenciadorDaCena").GetComponent<GerenciadorMenuSolidos>().esconderMenuDeSolidos();
		}
	}

	// Função que atualiza o sólido e seus vértices e atributos
	void atualizarSolido() {
		if(indiceSolido == -1) { // -1 significa que um prisma foi selecionado
			meshScript.ativarPrisma = true;
		} else {
			meshScript.ativarPrisma = false;

			if(facesPrisma < 3) {
				facesPrisma = 3; // Valor padrão é 3
			}

			// Definindo o nome do arquivo do sólido como o especificado na lista
			meshScript.fileName = listaSolidos[indiceSolido];
		}

		meshScript.facesPrisma = facesPrisma;
		// O InputField dos lados do prisma só deve ser ativo quando o prima for selecionado
		inputParentGO.SetActive(meshScript.ativarPrisma);
		
		// Por fim, re-inicializamos o objeto
		meshScript.inicializarObjeto();
		GameObject.Find("GO_ControladorLetrasSolido").GetComponent<ControladorListaVerticesArestas>().inicializarTextos();
	}
}
