using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomShape {
	public string version;
	public string name;
	public List<Vector3> vertices;
	public List<int> connections;
}
