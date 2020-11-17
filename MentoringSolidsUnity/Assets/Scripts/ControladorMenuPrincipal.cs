using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenuPrincipal : MonoBehaviour
{
	public void Iniciar() {
		SceneManager.LoadScene("ApresentacaoSolido");
	}

	public void Controles() {

	}

	public void Opcoes() {

	}

	public void Sair() {
		Application.Quit();
	}

}
