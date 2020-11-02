using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidKeyManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.R)) {
			GeneratedMeshFromJSON meshScript = GameObject.Find("SolidoApresentado").GetComponent<GeneratedMeshFromJSON>();
			meshScript.rotacaoAutomatica = !meshScript.rotacaoAutomatica;
		}
    }
}
