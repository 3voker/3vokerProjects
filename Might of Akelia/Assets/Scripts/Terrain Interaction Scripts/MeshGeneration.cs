using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGeneration : MonoBehaviour

{
    /// <summary>
    /// Mesh of the prefab mesh
    /// </summary>
    Mesh cubePrefabMesh;

    /// <summary>
    /// Mesh of the gameObject that contains this script
    /// </summary>
    Mesh gameObjectMesh;


    Vector3[] vertices;

    int[] triangles;

    public Material mat;

    float width = 1;

    float height = 1;

    [SerializeField]
    private GameObject cubePrefab;

    private GameObject cubeMesh;

    private Collider gameObjectCollider;

    private Renderer cubeMeshRenderer;

    [SerializeField]
    private GameObject parentGameObject;

    public bool canBeTriggered = false;
    public bool grindHasEnded = false;

    

    private void Awake()
    {
        gameObjectMesh = this.GetComponent<MeshFilter>().mesh;
        gameObjectCollider = this.GetComponent<Collider>();
    }
    void Start() {
       // MakeMeshData();
        //Createmesh();

        //Mesh mesh = new Mesh();

        //Vector3[] vertices = new Vector3[4];

        //vertices[0] = new Vector3(-width, -height);
        //vertices[1] = new Vector3(-width, height);
        //vertices[2] = new Vector3(width, height);
        //vertices[3] = new Vector3(width, -height);

        //mesh.vertices = vertices;

        //mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        //GetComponent<MeshRenderer>().material = mat;

        //GetComponent<MeshFilter>().mesh = mesh;

        cubeMesh = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            grindHasEnded = true;
            canBeTriggered = false;
        }      
    }

    //Create Mesh
    public void Createmesh(Vector3 meshPosition)
    {

        if (cubeMesh == null)
        {
            //Instantiate cubemesh at the location of the interactable gameObject(This might be working right now...)
            cubeMesh = Instantiate(cubePrefab, parentGameObject.transform.position, parentGameObject.transform.rotation) as GameObject;
            cubePrefabMesh = cubeMesh.GetComponent<MeshFilter>().mesh;
        }

        cubeMesh.SetActive(true);
        vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1) };
        ////Clear first
        //cubePrefabMesh.Clear();
        //cubePrefabMesh.vertices = vertices;

        //cubePrefabMesh.triangles = triangles;

        //cubePrefabMesh.RecalculateNormals();

        //Assign vertices to the gameObject's mesh vertices
        vertices = gameObjectMesh.vertices;

        //Create gameObject UV's as the vertices length
        Vector2[] gameObjectUVs = new Vector2[vertices.Length];


        //Create gameObject UV's as the vertices length
        Vector2[] cubePrefabUVs = new Vector2[vertices.Length];

        ScaleMesh();
    }

    private void ScaleMesh()
    {
        if (cubeMesh == null)
        {
            //Instantiate cubemesh at the location of the interactable gameObject(This might be working right now...)
            cubeMesh = Instantiate(cubePrefab, parentGameObject.transform.parent.position, parentGameObject.transform.parent.rotation) as GameObject;
            cubePrefabMesh = cubeMesh.GetComponent<MeshFilter>().mesh;
            cubeMeshRenderer = cubeMesh.GetComponent<Renderer>();

            cubeMesh.transform.parent = this.transform;
        }
        cubeMesh.SetActive(true);



        //Set the bounds as the gameObject's Mesh bounds
        Bounds bounds = gameObjectMesh.bounds;

        // Vector3 i = cubePrefabMesh.bounds.size;




        float prefabSizeX = cubePrefab.transform.localScale.x;
        float prefabSizeY = cubePrefab.transform.localScale.y;
        float prefabSizeZ = cubePrefab.transform.localScale.z;

        //Get the total of the gameObjects size
        float gameObjectSizeDifference = Vector3.Dot(cubePrefabMesh.bounds.size, bounds.size);

        int sizeDifferenceToInt = Mathf.RoundToInt(gameObjectSizeDifference);

        int i = 0;


        //While i is less than the gameObjectsUVs length, increase the cubePrefabMeshUV's
        while (i < sizeDifferenceToInt) //        while (i < gameObjectUVs.Length)
        {
            cubeMesh.transform.localScale += new Vector3(vertices[i].x / bounds.size.x, vertices[i].z / bounds.size.x);
            //cubePrefabUVs[i] = new Vector2(vertices[i].x / bounds.size.x, vertices[i].z / bounds.size.x);
            i++;
            if (cubeMesh.transform.localScale == transform.localScale)
                break;
        }
        //cubeMesh.transform.localScale = transform.localScale;           
    }

    public void IncreaseVolume()
    {
        if (cubeMesh == null)
        {
            //Instantiate cubemesh at the location of the interactable gameObject(This might be working right now...)
            cubeMesh = Instantiate(cubePrefab, parentGameObject.transform.localPosition, parentGameObject.transform.rotation) as GameObject;
            cubePrefabMesh = cubeMesh.GetComponent<MeshFilter>().mesh;
            cubeMeshRenderer = cubeMesh.GetComponent<Renderer>();
        }
        cubeMesh.transform.parent = this.transform;
        cubeMesh.transform.localScale = transform.localScale / 10;
        cubeMesh.SetActive(true);

        // get the volume from the bounds
        var addVolume = cubeMeshRenderer.bounds.size.x * cubeMeshRenderer.bounds.size.y * cubeMeshRenderer.bounds.size.z;

        // get the current radius
        var radius = transform.localScale.y;

        // now figure volume of the sphere
        var volume = (4 / 3 * Math.PI) * radius * radius * radius;

        // now add the mass of the cube
        volume += addVolume;

        // now reverse the calculation for the radius from the volume
        radius = Mathf.Sqrt((float)volume / (4 / 3 * Mathf.PI));


        cubeMesh.transform.localScale = Vector3.one * radius;

        if (cubeMesh.transform.localScale == transform.localScale)
        {
            cubeMesh.transform.localScale = transform.localScale;
        }     
    }

    public void DecreaseVolume()
    {
        if (cubeMesh == null)
        {
            //Instantiate cubemesh at the location of the interactable gameObject(This might be working right now...)
            cubeMesh = Instantiate(cubePrefab, this.transform.localPosition, this.transform.rotation) as GameObject;
            cubePrefabMesh = cubeMesh.GetComponent<MeshFilter>().mesh;
            cubeMeshRenderer = cubeMesh.GetComponent<Renderer>();
        }
        cubeMesh.transform.parent = this.transform;
        // get the volume from the bounds
        var subtractVolume = cubeMeshRenderer.bounds.size.x / cubeMeshRenderer.bounds.size.y / cubeMeshRenderer.bounds.size.z;

            // get the current radius
            var radius = transform.localScale.y;

            // now figure volume of the sphere
            var volume = (4 / 3 * Math.PI) * radius * radius * radius;

            // now add the mass of the cube
            volume -= subtractVolume;

            // now reverse the calculation for the radius from the volume
            radius = Mathf.Sqrt((float)volume / (4 / 3 * Mathf.PI));


            cubeMesh.transform.localScale = Vector3.one * radius;

            if (cubeMesh.transform.localScale == transform.localScale)
            {
                cubeMesh.transform.localScale = transform.localScale;
                cubeMesh.SetActive(false);
            }
        
      
    }

    public void RemoveMesh()
    {
        cubeMesh.SetActive(false);
    }

    //Make MeshData
    public void MakeMeshData()
    {
        //create an array of vertices
        vertices = new Vector3[] {new Vector3 (0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(1, 0, 1) };

        //Create an array of integers
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }
}
