using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NodeClass : MonoBehaviour {
	public Vector3 pointA;
	public Vector3 pointB;
	public Vector3 pointC;
	public Vector3 pointD;
	public Vector3 center;

	public int isABC = 0;
	public int isBCD = 0;
	public int isCDA = 0;
	public int isABD = 0;

	public bool draw;

	public List<NodeClass> fils = new List<NodeClass> ();

	void Start(){

	}

	void Update() {

	}

	public void CreerTetra(float scale){
		
		draw = false;
		float temp;
		temp = Random.Range (0.0F, 2.0F);
		if (temp >= 1)
			pointA.y = transform.position.y + (Random.Range (0.4F, 0.7F) * scale);
		else
			pointA.y = transform.position.y + (Random.Range (-0.4F, -0.7F) * scale);
		pointA.x = transform.position.x + (Random.Range (-0.2F, 0.2F) * scale);
		pointA.z = transform.position.z - (Random.Range (-0.2F, 0.2F) * scale);

		pointB.y = transform.position.y - (Random.Range (-0.1F, 0.1F) * scale);
		pointB.x = transform.position.x - (Random.Range (0.2F, 0.4F) * scale);
		pointB.z = transform.position.z - (Random.Range (-0.4F, -0.2F) * scale);

		pointC.y = transform.position.y - (Random.Range (-0.1F, 0.1F) * scale);
		pointC.x = transform.position.x - (Random.Range (-0.4F, -0.2F) * scale);
		pointC.z = transform.position.z - (Random.Range (0.2F, 0.4F) * scale);

		float difZ = pointC.z - pointB.z;
		float difX = pointC.x - pointB.x;

		pointD.y = pointB.y + (Random.Range (-0.1F, 0.1F) * scale);

		if (Mathf.Abs (difZ) > Mathf.Abs (difX)) {
			pointD.x = pointB.x + (Random.Range (-0.1F, 0.1F) * scale);
			pointD.z = pointB.z + (difZ * scale);
		} else {
			pointD.x = pointB.x + (difX * scale);
			pointD.z = pointB.z + (Random.Range (-0.1F, 0.1F) * scale);
		}

		/*
		GameObject s = new GameObject ();
		s = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		s.transform.position = pointA;
		s.transform.localScale = new Vector3(0.25F, 0.25F, 0.25F);

		GameObject s1 = new GameObject ();
		s1 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		s1.transform.position = pointB;
		s1.transform.localScale = new Vector3(0.25F, 0.25F, 0.25F);

		GameObject s2 = new GameObject ();
		s2 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		s2.transform.position = pointC;
		s2.transform.localScale = new Vector3(0.25F, 0.25F, 0.25F);


		GameObject s3 = new GameObject ();
		s3 = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		s3.transform.position = pointD;
		s3.transform.localScale = new Vector3(0.25F, 0.25F, 0.25F);*/


		/********************************************
		NE PAS COMMENTER !!!!!!!!!!!!!!!!!!!!!!!!!!!!
		*********************************************/
		center = transform.position;
		transform.position = new Vector3 (0F, 0F, 0F);
	}
}