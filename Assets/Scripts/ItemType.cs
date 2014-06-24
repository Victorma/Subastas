using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemType {

	public string name;
	public float mujer;
	public float hombre;
	public float joven;
	public float adulto;
	public float anciano;
	public float prole;
	public float normal;
	public float burgues;

	public ItemType(string name, float mujer, float hombre, float joven, float adulto, float anciano, float prole, float normal, float burgues) {
		this.name = name;
		this.mujer = mujer;
		this.hombre = hombre;
		this.joven = joven;
		this.adulto = adulto;
		this.anciano = anciano;
		this.prole = prole;
		this.normal = normal;
		this.burgues = burgues;
	}
}
