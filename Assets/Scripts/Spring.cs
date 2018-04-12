using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{

    public GameObject platform;
    public GameObject load;

    public Vector3 platformAnchorOffset;
    public Vector3 platformAnchorRotation;
    public Vector3 loadAnchorOffset;
    public Vector3 loadAnchorRotation;

    [Range(0.01f, 1.00f)]
    public float scale;
    [Range(0.0f, 500.0f)]
    public float constant;
    [Range(0.0f, 10.0f)]
    public float restLength;
    [Range(0.0f, 1.0f)]
    public float dampeningCoefficient;
    [Range(0.0f, 25.0f)]
    public float gravity = 1.0f;

    public bool isRigid;

    private Vector3 platformAnchor;
    private Vector3 platformAnchor1;
    private Vector3 platformAnchor2;
    private Vector3 platformAnchor3;
    private Vector3 platformAnchor4;

    private Vector3 loadAnchor;
    private Vector3 loadAnchor1;
    private Vector3 loadAnchor2;
    private Vector3 loadAnchor3;
    private Vector3 loadAnchor4;

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
        Vector3 platformPoint1 = this.platform.transform.position + Vector3.Scale(this.platformAnchorOffset, this.platform.transform.localScale) + Quaternion.Euler(this.platformAnchorRotation) * new Vector3( scale, -scale,  scale);
        Vector3 platformPoint2 = this.platform.transform.position + Vector3.Scale(this.platformAnchorOffset, this.platform.transform.localScale) + Quaternion.Euler(this.platformAnchorRotation) * new Vector3(-scale, -scale,  scale);
        Vector3 platformPoint3 = this.platform.transform.position + Vector3.Scale(this.platformAnchorOffset, this.platform.transform.localScale) + Quaternion.Euler(this.platformAnchorRotation) * new Vector3( scale, -scale, -scale);
        Vector3 platformPoint4 = this.platform.transform.position + Vector3.Scale(this.platformAnchorOffset, this.platform.transform.localScale) + Quaternion.Euler(this.platformAnchorRotation) * new Vector3(-scale, -scale, -scale);

        var localPlatformAnchor = (this.platform.transform.rotation * (platformPoint - this.platform.transform.position)) + this.platform.transform.position;
        var localPlatformAnchor1 = (this.platform.transform.rotation * (platformPoint1 - this.platform.transform.position)) + this.platform.transform.position;
        var localPlatformAnchor2 = (this.platform.transform.rotation * (platformPoint2 - this.platform.transform.position)) + this.platform.transform.position;
        var localPlatformAnchor3 = (this.platform.transform.rotation * (platformPoint3 - this.platform.transform.position)) + this.platform.transform.position;
        var localPlatformAnchor4 = (this.platform.transform.rotation * (platformPoint4 - this.platform.transform.position)) + this.platform.transform.position;

        this.platformAnchor = transform.InverseTransformPoint(localPlatformAnchor);
        this.platformAnchor1 = transform.InverseTransformPoint(localPlatformAnchor1);
        this.platformAnchor2 = transform.InverseTransformPoint(localPlatformAnchor2);
        this.platformAnchor3 = transform.InverseTransformPoint(localPlatformAnchor3);
        this.platformAnchor4 = transform.InverseTransformPoint(localPlatformAnchor4);

        // Convert load local space into global space
        Vector3 loadPoint = this.load.transform.position + Vector3.Scale(this.loadAnchorOffset, this.load.transform.localScale);
        Vector3 loadPoint1 = this.load.transform.position + Vector3.Scale(this.loadAnchorOffset, this.load.transform.localScale) + Quaternion.Euler(this.loadAnchorRotation) * new Vector3( scale,  scale,  scale);
        Vector3 loadPoint2 = this.load.transform.position + Vector3.Scale(this.loadAnchorOffset, this.load.transform.localScale) + Quaternion.Euler(this.loadAnchorRotation) * new Vector3(-scale,  scale,  scale);
        Vector3 loadPoint3 = this.load.transform.position + Vector3.Scale(this.loadAnchorOffset, this.load.transform.localScale) + Quaternion.Euler(this.loadAnchorRotation) * new Vector3( scale,  scale, -scale);
        Vector3 loadPoint4 = this.load.transform.position + Vector3.Scale(this.loadAnchorOffset, this.load.transform.localScale) + Quaternion.Euler(this.loadAnchorRotation) * new Vector3(-scale,  scale, -scale);

        var localLoadAnchor = (this.load.transform.rotation * (loadPoint - this.load.transform.position)) + this.load.transform.position;
        var localLoadAnchor1 = (this.load.transform.rotation * (loadPoint1 - this.load.transform.position)) + this.load.transform.position;
        var localLoadAnchor2 = (this.load.transform.rotation * (loadPoint2 - this.load.transform.position)) + this.load.transform.position;
        var localLoadAnchor3 = (this.load.transform.rotation * (loadPoint3 - this.load.transform.position)) + this.load.transform.position;
        var localLoadAnchor4 = (this.load.transform.rotation * (loadPoint4 - this.load.transform.position)) + this.load.transform.position;

        this.loadAnchor = transform.InverseTransformPoint(localLoadAnchor);
        this.loadAnchor1 = transform.InverseTransformPoint(localLoadAnchor1);
        this.loadAnchor2 = transform.InverseTransformPoint(localLoadAnchor2);
        this.loadAnchor3 = transform.InverseTransformPoint(localLoadAnchor3);
        this.loadAnchor4 = transform.InverseTransformPoint(localLoadAnchor4);

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

        // Check if force is only for the load or the platform
        if (this.platform.GetComponent<FixedJoint>())
        {
            this.load.GetComponent<Rigidbody>().AddForce(force);
        }
        else if (this.load.GetComponent<FixedJoint>())
        {
            this.platform.GetComponent<Rigidbody>().AddForce(-force);
        }
		// Apply force on the rigidbody based on the inverse masses
        else
        {
            float totalMass = this.load.GetComponent<Rigidbody>().mass + this.platform.GetComponent<Rigidbody>().mass;
            float loadInverseMass = 1 - (this.load.GetComponent<Rigidbody>().mass / totalMass);
            float platformInverseMass = 1 - (this.platform.GetComponent<Rigidbody>().mass / totalMass);

            this.load.GetComponent<Rigidbody>().AddForce(force * loadInverseMass);
            this.platform.GetComponent<Rigidbody>().AddForce(-force * platformInverseMass);
        }
		
		// Apply gravity to dampen spring (be sure to uncheck gravity from unity's rigidbody)
        this.load.GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity, 0));
    }

    void Render()
    {
        MeshFilter spring = GetComponent<MeshFilter>();
        MeshCollider springCollider = GetComponent<MeshCollider>();

		// Set points to be used as the mesh
        Vector3[] vertices = new Vector3[]
        {
            this.platformAnchor1,   //left top front, 0
            this.platformAnchor2,   //right top front, 1
            this.platformAnchor3,   //left bottom front, 2
            this.platformAnchor4,   //right bottom front, 3
            this.platformAnchor,    //top, 4
 
            this.loadAnchor1,       //left top front, 5
            this.loadAnchor2,       //right top front, 6
            this.loadAnchor3,       //left bottom front, 7
            this.loadAnchor4,       //right bottom front, 8
            this.loadAnchor,        //bottom, 9
        };

		// Create triangles to form polygons
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

		// Recalculate all the meshes from points
        spring.mesh.Clear();
        spring.mesh.vertices = vertices;
        spring.mesh.triangles = triangles;
        spring.mesh.RecalculateNormals();
        spring.mesh.RecalculateBounds();

		// To enable spring mesh collision
        if (isRigid)
        {
            springCollider.sharedMesh = spring.mesh;
        }
    }
}
