using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupController : MonoBehaviour
{
    public Material rimMaterial;

    private Material standardMaterial;
    private Material isHitMaterial;

    public GameObject Rack { get; set; }

    bool isHit;

    //Reracking stuff
    public Vector3 newPosition { get; set; }
    


    private void Start()
    {
        isHit = false;
        newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void Update()
    {
      
            float step = 2.5f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
        
    }

    public void SetMaterials(Material standard, Material hit)
    {
        isHitMaterial = hit;
        standardMaterial = standard;
        SetMainMaterial(standardMaterial);
    }

    private void SetMainMaterial(Material material)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        var m = new Material[2];
        m[0] = rimMaterial;
        m[1] = material;
        renderer.materials = m;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ballen traff koppen!!
        if(other.tag == "Ball")
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<BallController>().BallOutOfBounds(1f);
            RegisterHit(other.gameObject);
            UpdateToHitMaterial();
        }
    }

    public void RegisterHit(GameObject ball)
    {
        GameObject owner = ball.GetComponent<BallController>().owner;
        owner.GetComponent<PlayerRoundHandler>().HitCups.Add(gameObject);
    }

    private void UpdateToHitMaterial()
    {
        if(isHitMaterial != null)
        {
            SetMainMaterial(isHitMaterial);
        }
    }
    /*
    //Kan denne lages bedre?
    public List<GameObject> GetNeighbours()
    {
        Physics.
    }
    */
}
