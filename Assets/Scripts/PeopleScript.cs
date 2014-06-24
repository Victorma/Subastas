using UnityEngine;
using System.Collections.Generic;

public class PeopleScript : MonoBehaviour {

	public enum Genre{Male, Female}
	public enum Age{Young, Adult, Old}
	public enum SocialClass{Low, Middle, Gentry}

	public Genre genre;
	public Age age;
	public SocialClass socialClass;

	public Item item;

	private bool isMoving = false;
	public bool Moving {
		get { return isMoving; }
	}

	private bool isFollowing = false;
	public bool Following {
		get { return isFollowing; }
	}

	private Vector3 movingTo;

	private Transform followed;
	private float distance;

	public void moveTo(Vector3 point){
		isMoving = true;
		GetComponent<NavMeshAgent>().SetDestination(point);
		movingTo = point;
	}

	public void follow(Transform transform, float distance){
		int followdistance = 10;
		this.isFollowing = true;
		this.distance = distance;
		this.followed = transform;
	}

	public void unfollow(){
		this.isFollowing = false;
		this.followed = null;
	}

	private class RendererComparer : IComparer<SpriteRenderer> {
		public int Compare(SpriteRenderer s1, SpriteRenderer s2){
			float ys1 = s1.transform.position.z;
			float ys2 = s2.transform.position.z;

			if(ys1 < ys2)		return 1;
			else if(ys1 > ys2)	return -1;
			else 				return 0;
		}
	}

	private Vector3 stuckPos;
	private float stuckTime = 0;

	// Update is called once per frame
	void Update () {
		if(this.enabled){
			if(isFollowing){
				moveTo(followed.position + (this.transform.position - followed.position).normalized * distance);
			}
			if(this.isMoving){
				/*Vector3 vectorLeft = movingTo - transform.position;
				Vector3 movement = vectorLeft.normalized * speed * Time.deltaTime;
				if(movement.magnitude > vectorLeft.magnitude)
					movement = vectorLeft;

				transform.position += movement;

				if((transform.position - movingTo).magnitude < 0.001f)
					this.isMoving = false;
*/
				SpriteRenderer myS = GetComponentInChildren<SpriteRenderer>();
				if(myS == null)
					myS = GetComponent<SpriteRenderer>();

				SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>();
				List<SpriteRenderer> myLayer = new List<SpriteRenderer>();
				foreach(SpriteRenderer s in renderers)
					if(s.sortingLayerName == myS.sortingLayerName)
						myLayer.Add(s);

				myLayer.Sort(new RendererComparer());

				if((stuckPos - transform.position).magnitude < 0.01f){
					stuckTime += Time.deltaTime;
					if(stuckTime > 3){
						if(stuckTime > 5){
							transform.position = movingTo;
							stuckTime = 0;
						}else{
							GetComponent<NavMeshAgent>().ResetPath();
							GetComponent<NavMeshAgent>().SetDestination(movingTo);
						}
					}
				}else
					stuckTime = 0;
				stuckPos = transform.position;

				//GetComponent<NavMeshAgent>().ResetPath();

				for(int i = 0; i< myLayer.Count; i++)
					myLayer[i].sortingOrder = i;
				if((movingTo - transform.position).magnitude <= 0.002f){
					isMoving = false;
					GetComponent<NavMeshAgent>().Stop();
				}

			}

		}
		
	}
}
