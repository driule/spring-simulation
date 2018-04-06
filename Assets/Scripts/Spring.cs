using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public GameObject platform;
    public GameObject load;

    public Vector3 platformAnchorOffset;
    public Vector3 loadAnchorOffset;

    [Range(0.01f, 1.00f)]
    public float scale;
    [Range(0.0f, 100.0f)]
    public float constant;
    [Range(0.0f, 10.0f)]
    public float restLength;
    [Range(0.0f, 1.0f)]
    public float dampeningCoefficient;
    [Range(0.0f, 25.0f)]
    public float gravity = 1.0f;

    private Vector3 platformAnchor;
    private Vector3 loadAnchor;

    // Use this for initialization
    void Start()
    {
        this.CalculatePoints();
        this.CalculateForce();
        this.Render();
    }

    // Update is called once per frame
    void Update()
    {
        this.CalculatePoints();
        this.CalculateForce();
        this.Render();
    }

    void CalculatePoints()
    {
        // Convert platform local space into global space
        Vector3 platformPoint = this.platform.transform.position + Vector3.Scale(this.platformAnchorOffset, this.platform.transform.localScale);
        var localPlatformAnchor = (this.platform.transform.rotation * (platformPoint - this.platform.transform.position)) + this.platform.transform.position;
        this.platformAnchor = transform.InverseTransformPoint(localPlatformAnchor);

        // Convert load local space into global space
        Vector3 loadPoint = this.load.transform.position + Vector3.Scale(this.loadAnchorOffset, this.load.transform.localScale);
        var localLoadAnchor = (this.load.transform.rotation * (loadPoint - this.load.transform.position)) + this.load.transform.position;
        this.loadAnchor = transform.InverseTransformPoint(localLoadAnchor);
    }

    void CalculateForce()
    {
        // Vector pointing from platform anchor to load anchor
        Vector3 forceDirection = this.loadAnchor - this.platformAnchor;
        // Calculate the spring's current length
        float currentLength = forceDirection.magnitude;
        // x is the difference between current length and rest length
        float stretchLength = currentLength - this.restLength;

        // Calculate force accoring to Hooke's Law
        // F = -k * x
        forceDirection.Normalize();
        Vector3 force = forceDirection * (-constant * stretchLength) - dampeningCoefficient * load.GetComponent<Rigidbody>().velocity;

        // Apply force on the rigidbody based on the inverse masses
        if (this.platform.GetComponent<FixedJoint>())
        {
            this.load.GetComponent<Rigidbody>().AddForce(force);
        }
        else if (this.load.GetComponent<FixedJoint>())
        {
            this.platform.GetComponent<Rigidbody>().AddForce(-force);
        }
        else
        {
            float totalMass = this.load.GetComponent<Rigidbody>().mass + this.platform.GetComponent<Rigidbody>().mass;
            float loadInverseMass = 1 - (this.load.GetComponent<Rigidbody>().mass / totalMass);
            float platformInverseMass = 1 - (this.platform.GetComponent<Rigidbody>().mass / totalMass);

            this.load.GetComponent<Rigidbody>().AddForce(force * loadInverseMass);
            this.platform.GetComponent<Rigidbody>().AddForce(-force * platformInverseMass);
        }
        this.load.GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity, 0));
    }

    void Render()
    {
        MeshFilter spring = GetComponent<MeshFilter>();
        MeshCollider springCollider = GetComponent<MeshCollider>();

        Vector3[] vertices = new Vector3[]
        {
            this.platformAnchor + new Vector3(  this.scale, -this.scale,  this.scale),  //left top front, 0
            this.platformAnchor + new Vector3( -this.scale, -this.scale,  this.scale),  //right top front, 1
            this.platformAnchor + new Vector3(  this.scale, -this.scale, -this.scale),  //left bottom front, 2
            this.platformAnchor + new Vector3( -this.scale, -this.scale, -this.scale),  //right bottom front, 3
            this.platformAnchor,    //top, 4

            this.loadAnchor + new Vector3(  this.scale,  this.scale,  this.scale),  //left top front, 5
            this.loadAnchor + new Vector3( -this.scale,  this.scale,  this.scale),  //right top front, 6
            this.loadAnchor + new Vector3(  this.scale,  this.scale, -this.scale),  //left bottom front, 7
            this.loadAnchor + new Vector3( -this.scale,  this.scale, -this.scale),  //right bottom front, 8
            this.loadAnchor,        //bottom, 9
        };

        int[] triangles = new int[]
        {
            // Platform pyramid
            4, 1, 0,
            4, 0, 2,
            4, 2, 3,
            4, 3, 1,

            // Load pyramid
            9, 5, 6,
            9, 7, 5,
            9, 8, 7,
            9, 6, 8,

            // Connector
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
        spring.mesh.RecalculateBounds();

        springCollider.sharedMesh = spring.mesh;
    }
}
