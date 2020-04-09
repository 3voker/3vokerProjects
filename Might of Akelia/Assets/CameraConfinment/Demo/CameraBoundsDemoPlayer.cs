using UnityEngine;
using System.Collections;

namespace FreddieBabord
{
	public class CameraBoundsDemoPlayer : MonoBehaviour {

		public float speed = 15.0f;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(Input.GetKey(KeyCode.D))
				transform.Translate(Vector3.right*speed*Time.deltaTime);
			if(Input.GetKey(KeyCode.A))
				transform.Translate(Vector3.left*speed*Time.deltaTime);
			if(Input.GetKey(KeyCode.S))
				transform.Translate(Vector3.down*speed*Time.deltaTime);
			if(Input.GetKey(KeyCode.W))
				transform.Translate(Vector3.up*speed*Time.deltaTime);
			
			if(Input.GetKey(KeyCode.Q))
			{
				transform.Translate(Vector3.forward*speed*Time.deltaTime);
				SendMessage("UpdateScreenBounds", SendMessageOptions.DontRequireReceiver);
			}

			if(Input.GetKey(KeyCode.E))
			{
				transform.Translate(Vector3.back*speed*Time.deltaTime);
				SendMessage("UpdateScreenBounds", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
