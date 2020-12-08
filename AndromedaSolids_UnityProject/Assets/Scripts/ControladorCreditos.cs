using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCreditos : MonoBehaviour
{
    public void VoltarAoMenuPrincipal() {
		SceneManager.LoadScene("MenuPrincipal");
	}
}
