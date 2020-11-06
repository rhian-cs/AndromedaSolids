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

	public int numVertices;
	public int numArestas;
	public int numFaces;

	// Construtor que inicializa os atributos e define o nome como o do parâmetro
	public CustomShape(string newName, int version=0) {
		name = newName;
		vertices = new List<Vector3>();
		connections = new List<int>();
	}

	public void AddVertice(float x, float y, float z) {
		vertices.Add(new Vector3(x, y, z));
	}
	public void AddVertice(Vector3 v) {
		vertices.Add(v);
	}

	public void AddConnection(int indiceA, int indiceB) {
		connections.Add(indiceA);
		connections.Add(indiceB);
	}
}
