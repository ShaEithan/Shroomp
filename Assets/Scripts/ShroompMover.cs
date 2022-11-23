using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ShroompMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Invoke("getShroomp", 2f);
    }
    void getShroomp()
    {
        FindObjectOfType<ShroompController>().transform.position = transform.position;
        FindObjectOfType<ParallaxCamera>().updateOld();
        Destroy(gameObject);
    }
    void OnEnable()
    {
        
    }
    private void Awake()
    {
        getShroomp();
    }
}
