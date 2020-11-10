using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using TMPro;
using System.IO;

public class GeneratedMeshFromJSON : MonoBehaviour {

	// Material para quando o sólido estiver selecionado e não-selecionado. Padrão não-selecionado
	public Material unselectedMaterial;
	public Material selectedMaterial;

	public float connectionRadius = 0.1f;
	public string fileName = "";

	public bool ativarPrisma = false;
	public int facesPrisma = 3;
	public float alturaPrisma = 2f;
	public float raioPrisma = 1.5f;

	public bool rotacaoAutomatica = false;
	public float velocidadeRotacaoAutomatica = 12f;

	public bool inicializarAutomaticamente = false;

	// Elementos de UI que serão editados
	public TextMeshProUGUI textoTituloInformacoes;
	public TextMeshProUGUI textoNomeShape;
	public TextMeshProUGUI textoVertices;
	public TextMeshProUGUI textoArestas;
	public TextMeshProUGUI textoFaces;

	private List<string> labels; // Lista com os nomes de cada vértice
	private List <string> connectionLabels; // Lista com os nomes de cada aresta
	private List<bool> verticesDesenhados; // Lista que contém se um dado vértice já foi desenhado ou não, inicialmente false
	private List<GameObject> connObjs;

	private CustomShape	shape;

	// Start is called before the first frame update
	void Start() {
		labels = new List<string>();
		connectionLabels = new List<string>();
		connObjs = new List<GameObject>();
		verticesDesenhados = new List<bool>();
		textoTituloInformacoes.text = "Informações:";

		if(inicializarAutomaticamente) {
			inicializarObjeto();
		}
	}

	// Update is called once per frame
	void Update() {
		if(rotacaoAutomatica) {
			transform.Rotate(0, velocidadeRotacaoAutomatica * Time.deltaTime, 0);
		}
	}

	public void inicializarObjeto() {
		// Deletando os objetos anteriores
		for(int i = 0; i < connObjs.Count; i++) {
			GameObject.Destroy(connObjs[i]);
		}
		connObjs.Clear();
		labels.Clear();
		connectionLabels.Clear();
		verticesDesenhados.Clear();

		// Debug.Log("Inicializando objeto Shape");

		// Abrindo o arquivo JSON
		string jsonDir = Application.dataPath + "/Shapes/" + fileName;
		string jsonStr = "";
		if(fileName == "") {
			Debug.Log("Alerta: fileName é um valor vazio.");
		} else {
			jsonStr = File.ReadAllText(jsonDir);
		}

		// Debug.Log("ativar Prisma = " + ativarPrisma);
		if(ativarPrisma) {
			if(facesPrisma >= 3) {
				// Debug.Log("Gerando shape do prisma.");
				shape = gerarPrisma();
			} else {
				// Debug.Log("shape = null");
				shape = null;
			}
		} else {
			shape = null;
			if(fileName != "") {
				// Debug.Log("Criando JSON do arquivo " + fileName);
				shape = JsonUtility.FromJson<CustomShape>(jsonStr);
			}
		}

		gerarFormaGeometrica();
	}

	public void gerarFormaGeometrica() {
		// Debug.Log("Gerando forma geométrica. shape == null? " + shape == null);
		if(shape != null) {
			// Debug.Log("Ativar prisma? " + ativarPrisma);
			// Debug.Log("Vertice Count = " + shape.vertices.Count);
			// Debug.Log("Connections Count = " + shape.connections.Count);

			for(int i = 0; i < shape.vertices.Count; i++) {
				verticesDesenhados.Add(false);

				if(i < 26) {
					labels.Add(((char) (65 + i)).ToString());
				} else { // Caso contrário, defina a partir de A1..A25, B1..25, etc.
					int mod = i % 26;
					int div = (i-mod)/26;
					labels.Add(System.Convert.ToChar(65 + div).ToString() + mod);
				}
			}
			
			// Efetuando as ligações
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

		// Atualizando as informações do sólido
		if(shape != null) {
			textoNomeShape.text = shape.name;
			textoVertices.text = "Vértices = " + shape.numVertices;
			textoArestas.text = "Arestas = " + shape.numArestas;
			textoFaces.text = "Faces = " + shape.numFaces;
		} else {
			textoNomeShape.text = "";
			textoVertices.text = "Vértices = 0";
			textoArestas.text = "Arestas = 0";
			textoFaces.text = "Faces = 0";
		}
	}

	// Função que gera um prisma automaticamente dado um número de lados
	CustomShape gerarPrisma() {
		CustomShape prisma = new CustomShape("Prisma de " + facesPrisma + " lados");
		List<Vector3> vts = new List<Vector3>();

		float cx = 0;
		float cz = 0;
		// Criando a parte debaixo do sólido
		for(int i = 0; i < facesPrisma; i++) {
			float theta = 2.0f * Mathf.PI * (((float) i) / (float) facesPrisma);

			float x = raioPrisma * Mathf.Cos(theta);
			float z = raioPrisma * Mathf.Sin(theta);
			
			// Criando o vértice e o adicionando na lista
			Vector3 v = new Vector3(cx + x, -(alturaPrisma/2), cz + z);
			vts.Add(v);
			prisma.AddVertice(v);
						
			// Atualizando as ligações. Se for a última, ligar à primeira
			if(i < facesPrisma-1) {
				prisma.AddConnection(i, i+1);
			} else {
				prisma.AddConnection(0, i);
			}
		}

		// Criando a parte de cima do prisma, com base na parte debaixo
		for(int i = 0; i < facesPrisma; i++) {
			Vector3 v = vts[i];
			v.y += alturaPrisma; // Atualizando altura

			vts.Add(v);
			prisma.AddVertice(v);

			// Atualizando as ligações
			if(i < facesPrisma-1) {
				prisma.AddConnection(i+facesPrisma, i+facesPrisma+1);
			} else {
				prisma.AddConnection(i+facesPrisma, facesPrisma);
			}
			prisma.AddConnection(i, i+facesPrisma);
		}

		// Definindo atributos do objeto
		prisma.numVertices = prisma.vertices.Count;
		prisma.numArestas = prisma.connections.Count/2;
		prisma.numFaces = facesPrisma;
		
		return prisma;
	}

	public void toggleRotacaoAutomatica() {
		rotacaoAutomatica = !rotacaoAutomatica;
	}
}