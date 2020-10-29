using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GeneratedMeshFromJSON : MonoBehaviour {

	public Material unselectedMaterial;
	public Material selectedMaterial;

	public float connectionRadius = 0.1f;
	public string fileName = "";
	public int arestasPrisma = 0;
	public float alturaPrisma = 1f;

	// Start is called before the first frame update
	void Start() {
		string jsonDir = Application.dataPath + "/Shapes/" + fileName;

		if(fileName == "") {
			Debug.Log("Error: fileName is empty of invalid.");
		}

		string jsonStr = File.ReadAllText(jsonDir);
		CustomShape	shape;
		if(arestasPrisma == 0) {
			shape = JsonUtility.FromJson<CustomShape>(jsonStr);
		} else {
			shape = gerarPrisma();
		}

		// Criando lista com os nomes de cada vértice
		List<string> labels = new List<string>();
		// Criando lista com os nomes de cada reta
		List <string> connectionLabels = new List<string>();

		// Lista que contém se um dado vértice já foi desenhado ou não, inicialmente false
		List<bool> verticesDesenhados = new List<bool>();
		for(int i = 0; i < shape.vertices.Count; i++) {
			verticesDesenhados.Add(false);
		}


		// Definindo as letras automaticamente
		for(int i = 0; i < shape.connections.Count; i++) {
			// Se ainda não tiver estourado o alfabeto
			if(i < 26) {
				labels.Add(((char) (65 + i)).ToString());
			} else { // Caso contrário, defina a partir de A1..A25, B1..25, etc.
				int mod = i % 26;
				int div = (i-mod)/26;
				labels.Add(System.Convert.ToChar(65 + div).ToString() + mod);
			}
		}
		
		// Efetuando as ligações
		List<GameObject> connObjs = new List<GameObject>();
		for(int i = 0; i < shape.connections.Count; i+=2) {

			// Definindo os índices do primeiro e do segundo ponto a ser conectado a uma reta
			int a = shape.connections[i];
			int b = shape.connections[i+1];

			// Concatenando as letras dos vértices para obter o da Reta (ex. A + B vai ser a reta AB)
			connectionLabels.Add(labels[a] + labels[b]);

			// Criando uma linha que servirá de conexão de um vértice a outro, representando a reta
			// Sua posição é um ponto médio entre os dois pontos.
			GameObject conn = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			conn.name = "Connection_" + labels[a] + labels[b];
			conn.transform.parent = transform; // Definindo parentesco
			conn.transform.localScale = new Vector3(connectionRadius, (shape.vertices[a] - shape.vertices[b]).magnitude/2, connectionRadius); // Definindo comprimento e raio
			conn.transform.position = transform.position + (shape.vertices[a] + shape.vertices[b])/2; // Colocando na posição correta
			
			// Fazer com que esta linha mire para um dos pontos (com o eixo Y)
			conn.transform.LookAt(transform.position + shape.vertices[b]);
			conn.transform.Rotate(90, 0, 0);

			// Definindo o material como o Não Selecionado
			conn.GetComponent<Renderer>().material = unselectedMaterial;

			// Também desenhando os vértices no espaço como esferas, mas verificando se eles ainda não foram desenhados
			if(!verticesDesenhados[a]) {
				GameObject vertexA = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				vertexA.name = "Vertex_" + labels[a];
				vertexA.transform.parent = transform;
				vertexA.transform.localScale = new Vector3(connectionRadius, connectionRadius, connectionRadius);
				vertexA.transform.position = transform.position + shape.vertices[a];
				vertexA.GetComponent<Renderer>().material = unselectedMaterial;
				connObjs.Add(vertexA);
				verticesDesenhados[a] = true;
			}
			if(!verticesDesenhados[b]) {
				GameObject vertexB = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				vertexB.name = "Vertex_" + labels[b];
				vertexB.transform.parent = transform;
				vertexB.transform.localScale = new Vector3(connectionRadius, connectionRadius, connectionRadius);
				vertexB.transform.position = transform.position + shape.vertices[b];
				vertexB.GetComponent<Renderer>().material = unselectedMaterial;
				verticesDesenhados[b] = true;
				connObjs.Add(vertexB);
			}

			connObjs.Add(conn);
		}
	}

	// Update is called once per frame
	// void Update() {
		
	// }

	CustomShape gerarPrisma() {
		CustomShape prisma = new CustomShape("Prisma de " + arestasPrisma + " lados");

		float cx = 0;
		float cz = 0;
		float r = 1;

		List<Vector3> vts = new List<Vector3>();
		
		for(int i = 0; i < arestasPrisma; i++) {
			float theta = 2.0f * Mathf.PI * (((float) i) / (float) arestasPrisma);

			float x = r * Mathf.Cos(theta);
			float z = r * Mathf.Sin(theta);

			Vector3 v = new Vector3(cx + x, -(alturaPrisma/2), cz + z);
			vts.Add(v);
			prisma.AddVertice(v);
						
			if(i < arestasPrisma-1) {
				prisma.AddConnection(i, i+1);
			} else {
				prisma.AddConnection(0, i);
			}
		}
		for(int i = 0; i < arestasPrisma; i++) {
			Vector3 v = vts[i];
			v.y += alturaPrisma;

			vts.Add(v);
			prisma.AddVertice(v);

			if(i < arestasPrisma-1) {
				prisma.AddConnection(i+arestasPrisma, i+arestasPrisma+1);
			} else {
				prisma.AddConnection(i+arestasPrisma, arestasPrisma);
			}
			prisma.AddConnection(i, i+arestasPrisma);
		}
		
		return prisma;
	}
}