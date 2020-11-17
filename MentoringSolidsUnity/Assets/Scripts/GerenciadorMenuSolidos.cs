using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerenciadorMenuSolidos : MonoBehaviour
{

	public GameObject parentPanel_GO;

    // Start is called before the first frame update
    void Start()
    {
        parentPanel_GO.SetActive(true);
    }

	public void toggleMenuDeSolidos() {
		parentPanel_GO.SetActive(!parentPanel_GO.activeSelf);
	}

	public void esconderMenuDeSolidos() {
		parentPanel_GO.SetActive(false);
	}
}
