using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FishSwim : MonoBehaviour
{
    public float realSpeed;//实际速度
    public bool die=false;
    public bool is_the_last = false;

    public Transform show_parent;
    public Transform target_parent;
    public Transform[] wander_points;
    public GameObject[] models;
    public int index = 0;

    public AudioSource bubble;
    public AudioSource fall;

    int wander_index = 1;

    Vector3 cur_tar_pos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckChangePoint();
        MyMove();
        MyRotate();
    }

    void CheckChangePoint()
    {
        if ((transform.position - cur_tar_pos).magnitude <= realSpeed)
        {
            float nx = Random.RandomRange(wander_points[0].position.x,wander_points[1].position.x);
            float ny = Random.RandomRange(wander_points[0].position.y, wander_points[1].position.y);
            float nz = Random.RandomRange(wander_points[0].position.z, wander_points[1].position.z);
            cur_tar_pos = new Vector3(nx,ny,nz);
        }
    }

    void MyMove()
    {
        if (die) return;
        transform.position += (cur_tar_pos - transform.position).normalized * realSpeed;
    }

    void MyRotate()
    {
        if (die) return;
        transform.LookAt(cur_tar_pos);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
    }

    public void Die()
    {
        if (die) return;
        die = true;
        StartCoroutine(Fall());
    }

    public void MyInit(int id,int endid)
    {
        models = new GameObject[show_parent.transform.childCount];
        for (int i = 0; i < show_parent.transform.childCount; i++)
        {
            models[i] = show_parent.transform.GetChild(i).gameObject;
        }

        if (id != endid)
        {
            int rand = Random.Range(0, show_parent.transform.childCount - 1);
            models[rand].SetActive(true);
            is_the_last = false;
        }
        else
        {
            for (int i = 0; i < models.Length; i++)
            {
                if (models[i].name == "Shark")
                {
                    models[i].SetActive(true);
                    is_the_last = true;
                    break;
                }
            }
        }

        wander_points = FindObjectOfType<AllPoints>().all_Points;
        float nx = Random.Range(wander_points[0].position.x, wander_points[1].position.x);
        float ny = Random.Range(wander_points[0].position.y, wander_points[1].position.y);
        float nz = Random.Range(wander_points[0].position.z, wander_points[1].position.z);
        cur_tar_pos = new Vector3(nx, ny, nz);
        nx = Random.Range(wander_points[0].position.x, wander_points[1].position.x);
        ny = Random.Range(wander_points[0].position.y, wander_points[1].position.y);
        nz = Random.Range(wander_points[0].position.z, wander_points[1].position.z);
        transform.position = new Vector3(nx, ny, nz);
    }

    IEnumerator Fall()
    {
        float rad = Random.Range(0.0f,2.5f);
        yield return new WaitForSeconds(rad);
        transform.DORotate(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180), 1.0f);
        fall.Play();
        yield return new WaitForSeconds(1.0f);
        Rigidbody rigid=gameObject.AddComponent<Rigidbody>();
        yield return 0;
    }

    public void Revive()
    {
        bubble.Play();
        Destroy(GetComponent<Rigidbody>());
        die = false;
        transform.eulerAngles = Vector3.zero;
    }


}
