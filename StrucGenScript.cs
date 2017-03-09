using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class StrucGenScript : MonoBehaviour {

	public int nbpts;
	public int radius;
	[Range(0.05f, 1.7f)]
	public float scale;
	public int mindist1;
	public int maxdist1;
	public int mindist2;
	public int maxdist2;
	public int nbLiensParFace = 1;

	public GameObject prefab; 


	private Vector3[] vertices;
	private GameObject[] structure;
	private int tempFace;
	private Vector3[] faces;

	public GameObject[] getStructure(){
		return structure;
	}


	public void Start () {
		structure = new GameObject[nbpts];
		StartCoroutine(GeneratePoint());
		Mesh m = transform.GetComponent<MeshFilter> ().mesh;
	//	m.RecalculateNormals ();
	//	m.RecalculateBounds (); 

	}

	private IEnumerator GeneratePoint () {
		//Generation des coordonées des noeuds
		WaitForSeconds wait = new WaitForSeconds(0.01f);
		vertices = new Vector3[nbpts];
		for (int i = 0; i < nbpts; i++){	

			float angleA = Random.Range (0, 2 * Mathf.PI);
			float angleB = Random.Range (0, 2 * Mathf.PI);
			float myX = (radius * Mathf.Sin (angleA) * Mathf.Cos (angleB)) + transform.localPosition.x;
			float myY = (radius * Mathf.Sin (angleA) * Mathf.Sin (angleB)) + transform.localPosition.y;
			float myZ = (radius * Mathf.Cos (angleA))+ transform.localPosition.z;
			Vector3 vect1 = new Vector3(myX,myY,myZ);
			vect1 = Vector3.MoveTowards (vect1, transform.localPosition, Random.Range(-radius/10,radius/10));
			vertices [i] = vect1;
			yield return wait;
		}
		GenerateStructure ();


	}

	//Fonction Main de la generation de structure
	public void GenerateStructure () {
		if (vertices == null) {
			return;
		}
		for (int i = 0; i < vertices.Length; i++) {

			structure[i] = (GameObject)Instantiate (prefab, vertices [i], Quaternion.identity);	
			structure [i].GetComponent<NodeClass> ().CreerTetra (scale);
			structure [i].tag = "tetra";
			structure [i].transform.parent = transform;
		}


		ComparerDistance ();

		//On supprime tous les morceaux de mesh une fois conbinés
		RemoveMesh ();


		GameObject[] lumObj = GameObject.FindGameObjectsWithTag ("lumieres");

		lumObj[0].GetComponent<LumiereMonitor>().enabled = true;
	}

	public void ComparerDistance(){

		for (int i = 0; i < nbpts; i++) {
			Vector3 v1 = vertices [i]; 
			for (int j = 0; j < nbpts; j++) {
				Vector3 v2 = vertices [j];

				float dist = Vector3.Distance (v1, v2);
				if ((dist < maxdist2) && (dist > mindist2) || (dist < maxdist1) && (dist > mindist1)) {
					faces = ComparerFace (structure [i].GetComponent<NodeClass> (), structure [j].GetComponent<NodeClass> ());
					if (faces != null) {
						if (!structure [i].GetComponent<NodeClass> ().draw) { 
							AddTetra (structure [i]);
						}
						if (!structure [j].GetComponent<NodeClass> ().draw) {
							AddTetra (structure [j]);
						}
						AddEdge (faces, i);
						structure [i].GetComponent<NodeClass> ().fils.Add (structure [j].GetComponent<NodeClass> ());
						structure [j].GetComponent<NodeClass> ().fils.Add (structure [i].GetComponent<NodeClass> ());
					}
				}
			}
		}
	}

	public Vector3[] ComparerFace(NodeClass g1, NodeClass g2){
		//On reinitialise TempFace
		tempFace = -1;

		//Tableau de sortie avec les points reliées 0-3 1-4 2-5
		Vector3[] triangle = new Vector3[6];

		//Stocker les 3 points de chaque gameobject
		Vector3[] pointG1Nontrier = TetraToTri(g1, g2);
		if (pointG1Nontrier == null)
			return null;
		Vector3[] pointG2Nontrier = TetraToTri(g2, g1);
		if (pointG2Nontrier == null)
			return null;


		int point = 0, resteATrier = 3;
		float distance1 = 0 ,distance2;
		for(int j = 0 ; j < 3 ; j++){ 
			for (int i = 0; i < resteATrier; i++) {
				if (i == 0) {
					distance1 = Vector3.Distance (pointG1Nontrier [i], pointG2Nontrier [j]);
					point = i;
				} else {
					distance2 = Vector3.Distance (pointG1Nontrier [i], pointG2Nontrier [j]);
					if (distance1 < distance2) {
						Vector3 temp = pointG1Nontrier [i];
						pointG1Nontrier [i] = pointG1Nontrier [i - 1];
						pointG1Nontrier [i - 1] = temp;
					} else {
						distance1 = distance2;
						point = i;
					}
				}
			}
			triangle [j] = pointG1Nontrier [point];
			triangle [j + 3] = pointG2Nontrier [j];

			resteATrier--;
			if (resteATrier > 1) {
				Vector3[] temp = new Vector3[resteATrier];
				int t = 0;
				for (int u = 0; u < pointG1Nontrier.Length; u++) {
					if (u != point) {
						temp [t] = pointG1Nontrier [u];
						t++;
					}
				}
				pointG1Nontrier = temp;
			}
		}

		return triangle;
	}

	public Vector3[] TetraToTri(NodeClass g1, NodeClass g2){
		//Stocker tous les points dans un tableau
		Vector3[] points = new Vector3[4];
		points [0] = g1.pointA;
		points [1] = g1.pointB;
		points [2] = g1.pointC;
		points [3] = g1.pointD;

		//On Stocke le nom de la face pour ne pas mettre 2 liens par face
		int[] pointString = new int[4];
		pointString [0] = 1000;
		pointString [1] = 100;
		pointString [2] = 10;
		pointString [3] = 1;

		//Stocker les 3 points de chaque gameobject
		Vector3[] pointNontrier = new Vector3[3];
		int[] nomPoint = new int[3];

		//Pour comparer et stocker les valeurs de distance entre les 2 gameobjects
		float[] d = new float[4];

		/**
		 * Premier GameObject
		 **/
		d[0] = Vector3.Distance (g1.pointA, g2.pointA);
		d[1] = Vector3.Distance (g1.pointB, g2.pointA);
		d[2] = Vector3.Distance (g1.pointC, g2.pointA);
		d[3] = Vector3.Distance (g1.pointD, g2.pointA);

		for (int i = 0; i < 3; i++) {
			if (d [i] < d [i + 1]) {
				//Ajout du plus pret aux listes resultats
				pointNontrier [i] = points [i];
				nomPoint[i] = pointString [i];
			} else {
				//Ajout du plus pret aux listes resultats
				pointNontrier [i] = points [i+1];
				nomPoint[i] = pointString [i+1];

				//Transmission du sommet non choisi pour le comparer aux points restants
				pointString [i + 1] = pointString [i];
				points [i + 1] = points [i];
				d [i + 1] = d [i];
			}
		}

		int nomFace = nomPoint [0] + nomPoint [1] + nomPoint [2];

		switch (nomFace) {
		case 1110:
			if (g1.isABC >= nbLiensParFace) {
				if (tempFace != -1) {
					switch (tempFace) {
					case 1110:
						g2.isABC--;
						break;
					case 0111:
						g2.isBCD--;
						break;
					case 1011:
						g2.isCDA--;
						break;
					case 1101:
						g2.isABD--;
						break;
					}
				}
				pointNontrier = null;
			} else {
				if (tempFace == -1)
					tempFace = 1110;
				g1.isABC++;
			}
			break;
		case 0111:
			if (g1.isBCD >= nbLiensParFace) {
				if (tempFace != -1) {
					switch (tempFace) {
					case 1110:
						g2.isABC--;
						break;
					case 0111:
						g2.isBCD--;
						break;
					case 1011:
						g2.isCDA--;
						break;
					case 1101:
						g2.isABD--;
						break;
					}
				}
				pointNontrier = null;
			} else {
				if (tempFace == -1)
					tempFace = 0111;
				g1.isBCD++;
			}
			break;
		case 1011:
			if (g1.isCDA >= nbLiensParFace) {
				if (tempFace != -1) {
					switch (tempFace) {
					case 1110:
						g2.isABC--;
						break;
					case 0111:
						g2.isBCD--;
						break;
					case 1011:
						g2.isCDA--;
						break;
					case 1101:
						g2.isABD--;
						break;
					}
				}
				pointNontrier = null;
			} else {
				if (tempFace == -1)
					tempFace = 1011;
				g1.isCDA++;
			}
			break;
		case 1101:
			if (g1.isABD >= nbLiensParFace) {
				if (tempFace != -1) {
					switch (tempFace) {
					case 1110:
						g2.isABC--;
						break;
					case 0111:
						g2.isBCD--;
						break;
					case 1011:
						g2.isCDA--;
						break;
					case 1101:
						g2.isABD--;
						break;
					}
				}
				pointNontrier = null;
			} else {
				if (tempFace == -1)
					tempFace = 1101;
				g1.isABD++;
			}
			break;
		}

		return pointNontrier;
	}

	public void AddTetra(GameObject node){
		
		GameObject child = new GameObject ();
		child.transform.parent = node.transform;
		child.AddComponent<MeshFilter> ();

		NodeClass cs = node.GetComponent<NodeClass> ();
		Mesh mesh = new Mesh();
		mesh.Clear();
		Vector3[] points = new Vector3[4];
		points [0] = cs.pointA;
		points [1] = cs.pointB;
		points [2] = cs.pointC;
		points [3] = cs.pointD;
		// make changes to the Mesh by creating arrays which contain the new values
		mesh.vertices = points;
		mesh.triangles = new int[]{
			0,1,2,
			0,2,3,
			2,1,3,
			0,3,1
		};	
		node.GetComponent<NodeClass>().draw = true;
		child.GetComponent<MeshFilter> ().mesh = mesh;
		Fusion ();
	}

	public void AddEdge(Vector3[] v, int numero){
		
		Mesh mesh =	structure [numero].GetComponent<MeshFilter> ().mesh;
		mesh.Clear();

		// make changes to the Mesh by creating arrays which contain the new values
		mesh.vertices = v;
		mesh.triangles =  new int[]{0,1,2,
			5,4,3,
			2,4,5,
			1,4,2,
			0,3,1,
			1,3,4,
			0,2,5,
			5,3,0
		};
		structure [numero].transform.gameObject.SetActive (true);
		Fusion ();
	}

	public void Fusion(){
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter> ();
		MeshRenderer[] meshRend = GetComponentsInChildren<MeshRenderer> ();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine [i].mesh = meshFilters [i].sharedMesh;
			combine [i].transform = meshFilters [i].transform.localToWorldMatrix;
			i++;
		}
		transform.GetComponent<MeshFilter> ().mesh = new Mesh ();
		transform.GetComponent<MeshFilter> ().mesh.CombineMeshes (combine);
		transform.gameObject.active = true;

	}

	public void RemoveMesh(){

		foreach (GameObject g in GameObject.FindGameObjectsWithTag ("tetra")) {
			Destroy (g.GetComponent<MeshRenderer> ());
			Destroy (g.GetComponent<MeshFilter> ());
		}
		
	}

}

