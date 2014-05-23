using UnityEngine;
using System.Collections.Generic;

public class PeopleScript : MonoBehaviour {

	public enum Genre{Male, Female}
	public enum Age{Young, Adult, Old}
	public enum SocialClass{Low, Middle, Gentry}

	public Genre genre;
	public Age age;
	public SocialClass socialClass;

	public float speed;

	private bool isMoving = false;
	public bool Moving {
		get { return isMoving; }
	}
	private Vector3 movingTo;

	public void moveTo(Vector3 point){
		isMoving = true;
		movingTo = point;
	}

	// Use this for initialization
	void Start () {
		
	}

	private class RendererComparer : IComparer<SpriteRenderer> {
		public int Compare(SpriteRenderer s1, SpriteRenderer s2){
			float ys1 = s1.transform.position.y;
			float ys2 = s2.transform.position.y;

			if(ys1 < ys2)		return 1;
			else if(ys1 > ys2)	return -1;
			else 				return 0;
		}
	}

	// Update is called once per frame
	void Update () {
		if(this.enabled){
			if(this.isMoving){
				Vector3 vectorLeft = movingTo - transform.position;
				Vector3 movement = vectorLeft.normalized * speed * Time.deltaTime;
				if(movement.magnitude > vectorLeft.magnitude)
					movement = vectorLeft;

				transform.position += movement;

				if((transform.position - movingTo).magnitude < 0.001f)
					this.isMoving = false;

				SpriteRenderer myS = GetComponent<SpriteRenderer>();

				SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>();
				List<SpriteRenderer> myLayer = new List<SpriteRenderer>();
				foreach(SpriteRenderer s in renderers)
					if(s.sortingLayerName == myS.sortingLayerName)
						myLayer.Add(s);

				myLayer.Sort(new RendererComparer());

				for(int i = 0; i< myLayer.Count; i++)
					myLayer[i].sortingOrder = i;
			

			}
		}
		
	}
}
