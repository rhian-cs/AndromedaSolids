using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidKeyManager : MonoBehaviour
{
	public Canvas hud;

	private GeneratedMeshFromJSON meshScript;

	void Start() {
		meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.R)) {
			meshScript.toggleRotacaoAutomatica();
		}
        if(Input.GetKeyUp(KeyCode.Z)) {
			hud.enabled = !hud.enabled;
		}
    }
}
