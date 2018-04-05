using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringMeshRenderer : MonoBehaviour {

    public Transform platform;
    public Transform load;
    public SpringJoint springJoint;

    public float scale = 0.1f;

    private Vector3 springPlatformAnchor;
    private Vector3 springLoadAnchor;

	// Use this for initialization
	void Start ()
    {
        this.RenderSpring();
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.RenderSpring();
    }

    void RenderSpring()
    {
        MeshFilter spring = GetComponent<MeshFilter>();

        Vector3 platformPoint = this.platform.position + Vector3.Scale(this.springJoint.anchor, this.platform.localScale);
        var platformAnchor = (this.platform.rotation * (platformPoint - this.platform.position)) + this.platform.position;
        this.springPlatformAnchor = transform.InverseTransformPoint(platformAnchor);

        Vector3 loadPoint = this.load.position + Vector3.Scale(this.springJoint.connectedAnchor, this.load.localScale);
        var loadAnchor = (this.load.rotation * (loadPoint - this.load.position)) + this.load.position;
        this.springLoadAnchor = transform.InverseTransformPoint(loadAnchor);
        
        Vector3[] vertices = new Vector3[]
        {
            //bottom face//
            this.springPlatformAnchor + new Vector3(  this.scale, -this.scale,  this.scale),  //left top front, 0
            this.springPlatformAnchor + new Vector3( -this.scale, -this.scale,  this.scale),  //right top front, 1
            this.springPlatformAnchor + new Vector3(  this.scale, -this.scale, -this.scale),  //left bottom front, 2
            this.springPlatformAnchor + new Vector3( -this.scale, -this.scale, -this.scale),  //right bottom front, 3

            this.springPlatformAnchor,   //top, 4

            this.springLoadAnchor + new Vector3(  this.scale,  this.scale,  this.scale),    //left top front, 5
            this.springLoadAnchor + new Vector3( -this.scale,  this.scale,  this.scale),    //right top front, 6
            this.springLoadAnchor + new Vector3(  this.scale,  this.scale, -this.scale),    //left bottom front, 7
            this.springLoadAnchor + new Vector3( -this.scale,  this.scale, -this.scale),    //right bottom front, 8

            this.springLoadAnchor,       //bottom, 9
        };

        int[] triangles = new int[]
        {
            //platform pyramid 
            4, 1, 0,
            4, 0, 2,
            4, 2, 3,
            4, 3, 1,

            //load pyramid
            9, 6, 5,
            9, 5, 7,
            9, 7, 8,
            9, 8, 6,

            //connector
            0, 1, 5,
            1, 6, 5,
            1, 3, 6,
            3, 8, 6,
            3, 2, 8,
            2, 7, 8,
            2, 0, 7,
            0, 5, 7,
        };

        spring.mesh.Clear();
        spring.mesh.vertices = vertices;
        spring.mesh.triangles = triangles;
        spring.mesh.RecalculateNormals();
    }
}
