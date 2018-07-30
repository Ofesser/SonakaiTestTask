using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Информация об объекте.
//
// В более больших проектах было бы целесообразно сделать этот класс родительским, в котором
//    будет описываться общая информация, и в наследниках для каждого типа объекта описывать уникальную логику.
public class BuildingComponent : MonoBehaviour {

    public BuildingTypes buildingType;
    private float meshHeight;

    protected virtual void Start()
	{
        meshHeight = GetComponent<MeshRenderer>().bounds.size.y;
	}

	public float MeshHeight
    { 
        get 
        {
            return meshHeight;
        }
    }

}
