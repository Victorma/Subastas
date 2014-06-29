using UnityEngine;
using System.Collections.Generic;

public class PoliceScript : MonoBehaviour {

	public bool Moving {
		get { return agent.hasPath; }
	}
	
	private bool isFollowing = false;
	public bool Following {
		get { return isFollowing; }
	}
	
	private NavMeshAgent agent;
	private SpriteRenderer sr;
	
	private Vector3 movingTo;
	private Transform followed;
	private float distance;
	private Vector3 stuckPos;
	private float stuckTime = 0;
	
	public void moveTo(Vector3 point){
		if (this.enabled) {
			if(!started) Start ();
			agent.SetDestination(point);
			movingTo = point;
		}
	}
	
	public void follow(Transform transform, float distance){
		this.isFollowing = true;
		this.distance = distance;
		this.followed = transform;
	}
	
	public void unfollow(){
		this.isFollowing = false;
		this.followed = null;
	}
	
	private bool started = false;
	void Start (){
		sr = GetComponentInChildren<SpriteRenderer> ();
		agent = GetComponent<NavMeshAgent> ();
		started = true;
	}
	
	void Update () {
		if(this.enabled){
			if(!started) Start ();
			
			if(isFollowing){
				moveTo(followed.position + (this.transform.position - followed.position).normalized * distance);
			}
			
			if(this.Moving){
				
				SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>();
				List<SpriteRenderer> myLayer = new List<SpriteRenderer>();
				foreach(SpriteRenderer s in renderers)
					if(s.sortingLayerName == sr.sortingLayerName)
						myLayer.Add(s);
				
				myLayer.Sort(RendererComparer.Instance);
				for(int i = 0; i< myLayer.Count; i++)
					myLayer[i].sortingOrder = i;
				
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
				
				
				if((movingTo - transform.position).magnitude <= 0.02f)
					GetComponent<NavMeshAgent>().Stop();
			}else{
				GetComponentInChildren<Animator>().Play("south");
			}
		}
	}
	
	private class RendererComparer : IComparer<SpriteRenderer> {
		
		private static RendererComparer instance;
		public static RendererComparer Instance {get {return (instance == null)? instance = new RendererComparer(): instance;}}
		
		public int Compare(SpriteRenderer s1, SpriteRenderer s2){
			float ys1 = s1.transform.position.z;
			float ys2 = s2.transform.position.z;
			
			if(ys1 < ys2)		return 1;
			else if(ys1 > ys2)	return -1;
			else 				return 0;
		}
	}
}
