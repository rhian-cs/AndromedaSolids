﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidKeyManager : MonoBehaviour
{
	public Canvas hud;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.R)) {
			toggleRotacaoAutomatica();
		}
        if(Input.GetKeyUp(KeyCode.Z)) {
			hud.enabled = !hud.enabled;
		}
    }

	public void toggleRotacaoAutomatica() {
		GeneratedMeshFromJSON meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();
		meshScript.rotacaoAutomatica = !meshScript.rotacaoAutomatica;
	}
}