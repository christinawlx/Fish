using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeletFish : MonoBehaviour
{
    public int fish_num;
    public GameObject fish_pref;
    public List<FishSwim> all_group=new List<FishSwim>();
    public int[] Die_ID;
    public Transform background;
    public Transform[] all_position;
    public int cur_index=0;
    public Transform out_screen;
    Rigidbody testRigid;
	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < fish_num; i++)
        {
            GameObject temp=Instantiate(fish_pref,transform);
            temp.GetComponent<FishSwim>().MyInit(i,fish_num-1);
            all_group.Add(temp.GetComponent<FishSwim>());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            for(int i = 0; i < all_group.Count; i++)
            {
                if (!all_group[i].die&&!all_group[i].is_the_last)
                {
                    all_group[i].Die();
                    if (i + 1 < all_group.Count && !all_group[i + 1].is_the_last)
                    {
                        all_group[i + 1].Die();
                    }
                    break;
                }
            }
            CheckMoveBack();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GameObject temp = Instantiate(fish_pref, transform);
            //temp.GetComponent<FishSwim>().MyInit(all_group.Count, fish_num - 1);
            //temp.transform.position = out_screen.position;
            for (int i = 0; i < all_group.Count; i++)
            {
                if (all_group[i].die)
                {
                    all_group[i].Revive();
                    all_group[i].transform.position = out_screen.position;
                    break;
                }
            }
        }
	}

    void CheckMoveBack()
    {
        int die_count=0;
        for(int i = 0; i < all_group.Count; i++)
        {
            if (all_group[i].die)
            {
                die_count++;
            }
        }

        int test_index = 0;
        for(int i = 0; i < Die_ID.Length; i++)
        {
            if (die_count >= Die_ID[i])
            {
                test_index++;
            }
        }

        cur_index = Mathf.Max(cur_index,test_index);

        if (cur_index < all_position.Length)
        {
            background.DOMove(all_position[cur_index].position,3.0f);
        }
    }

}
