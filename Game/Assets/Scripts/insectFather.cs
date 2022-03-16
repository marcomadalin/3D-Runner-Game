using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class insectFather : MonoBehaviour
{
    public playerController pc;
    private int done, nInsects;
    public GameObject insect;
    public GameObject astronaut;
    public bossController bc;
    public GameObject EndWall;

    void Start()
    {
        done = 0;
    }
    void Update()
    {

        nInsects = pc.insectsNum;
        
        if (pc.insectsNum >= 24 && done == 0)
        {
            Debug.Log("Enter");
            done = 1;
            deleteAndIncrease(1);

        }
        else if (pc.insectsNum >= 47 && done == 1)
        {
            Debug.Log("Enter");
            done = 2;
            deleteAndIncrease(2);
        }
        else if (pc.insectsNum >= 69 && done == 2)
        {
            Debug.Log("Enter");
            done = 3;
            deleteAndIncrease(3);
        }
    }

    private void deleteAndIncrease(int it)
    {

        int n = transform.childCount;
        for (int j = 0; j < n; ++j)
        {
            GameObject child = transform.GetChild(j).gameObject;
            insectController ic = child.GetComponent<insectController>();

            if (ic.ini && !ic.dead)
            {
                if (it == 1)
                {
                    if (ic.id == 0)
                    {
                        child.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                        ic.positionMatrix[0] = new Vector3(0.0f, 0.0f, -2.5f);
                        ic.increaseEffect();
                    }
                    else Destroy(child);
                }
                else if (it == 2)
                {
                    if (ic.id == 1)
                    {
                        child.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                        ic.positionMatrix[1] = new Vector3(-2.5f, 0.0f, 0.0f);
                        ic.increaseEffect();
                    }
                    else if (ic.id != 0) Destroy(child);
                }
                else if (it == 3)
                {
                    if (ic.id == 2)
                    {
                        child.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                        ic.positionMatrix[2] = new Vector3(2.5f, 0.0f, 0.0f);
                        ic.increaseEffect();
                    }
                    else if (ic.id != 0 && ic.id != 1) Destroy(child);
                }
            }
        }

    }

    public int getFirstId()
    {
        bool[] usedId = new bool[] { false, false, false, false, false, false, false, false, false, false,
                                false, false, false, false, false, false, false, false, false, false,
                                false, false, false, false};
        int n = transform.childCount;
        for (int j = 0; j < n; ++j)
        {
            GameObject child = transform.GetChild(j).gameObject;
            insectController ic = child.GetComponent<insectController>();
            if (ic.ini && !ic.dead)
            {
                usedId[ic.id] = true;
            }
        }

        n = 24;
        for (int i = 0; i < n; ++i)
        {
            if (!usedId[i]) return i;
        }
        return 0;
    }

    public void setInsects(int news)
    {
        insectController control = insect.GetComponent<insectController>();
        playerController pc = astronaut.GetComponent<playerController>();
        
        
        int n = transform.childCount;

        for (int i = 0; i < n; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            insectController ic = child.GetComponent<insectController>();
            if (ic.ini && !ic.dead)
            {
                Destroy(child);
            }
        }
        pc.insectsNum = 0;
        if (news <= 24)
        {
            done = 0;
            n = news;
            for (int i = 0; i < n; ++i)
            {
                Vector3 pos = control.positionMatrix[i];
                GameObject ins = Instantiate(insect, astronaut.transform.position + pos, insect.transform.rotation);
                insectController ic = ins.GetComponent<insectController>();
                ic.ini = true;
                ic.id = i;
                ic.astronaut = astronaut;
                ic.bc = bc;
                ic.EndWall = EndWall;
                ins.transform.parent = transform;
                //ins.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
        else if (news <= 47)
        {
            done = 1;
            n = news - 23;

            GameObject ins = Instantiate(insect, astronaut.transform.position + new Vector3(0.0f, 0.0f, -2.5f), insect.transform.rotation);
            insectController ic = ins.GetComponent<insectController>();
            ic.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            ic.id = 0;
            ic.positionMatrix[0] = new Vector3(0.0f, 0.0f, -2.5f);
            ic.ini = true;
            ic.astronaut = astronaut;
            ic.bc = bc;
            ic.EndWall = EndWall;
            ins.transform.parent = transform;
            //ins.transform.Rotate(0.0f, 180.0f, 0.0f);

            for (int i = 1; i < n; ++i)
            {
                Vector3 pos = control.positionMatrix[i];
                GameObject inse = Instantiate(insect, astronaut.transform.position + pos, insect.transform.rotation);
                insectController ice = inse.GetComponent<insectController>();
                ice.ini = true;
                ice.id = i;
                ice.astronaut = astronaut;
                ice.bc = bc;
                ice.EndWall = EndWall;
                inse.transform.parent = transform;
                //inse.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
        else if (news <= 69)
        {
            done = 2;
            n = news - 45;

            GameObject ins = Instantiate(insect, astronaut.transform.position + control.positionMatrix[0], insect.transform.rotation);
            insectController ic = ins.GetComponent<insectController>();
            ic.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            ic.id = 0;
            //ic.positionMatrix[0] = new Vector3(2.5f, 0.0f, 0.0f);
            ic.ini = true;
            ic.astronaut = astronaut;
            ic.bc = bc;
            ic.EndWall = EndWall;
            ins.transform.parent = transform;
            //ins.transform.Rotate(0.0f, 180.0f, 0.0f);

            GameObject ins2 = Instantiate(insect, astronaut.transform.position + control.positionMatrix[1], insect.transform.rotation);
            insectController ic2 = ins2.GetComponent<insectController>();
            ic2.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            ic2.id = 1;
            ic2.positionMatrix[1] = new Vector3(-2.5f, 0.0f, 0.0f);
            ic2.ini = true;
            ic2.astronaut = astronaut;
            ic2.bc = bc;
            ic2.EndWall = EndWall;
            ins2.transform.parent = transform;
            //ins2.transform.Rotate(0.0f, 180.0f, 0.0f);

            for (int i = 2; i < n; ++i)
            {
                Vector3 pos4 = control.positionMatrix[i];
                GameObject ins4 = Instantiate(insect, astronaut.transform.position + pos4, insect.transform.rotation);
                insectController ic4 = ins4.GetComponent<insectController>();
                ic4.ini = true;
                ic4.id = i;
                ic4.astronaut = astronaut;
                ic4.bc = bc;
                ic4.EndWall = EndWall;
                ins4.transform.parent = transform;
                //ins4.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
        else
        {
            done = 3;
            n = news - 66;

            GameObject ins = Instantiate(insect, astronaut.transform.position + control.positionMatrix[0], insect.transform.rotation);
            insectController ic = ins.GetComponent<insectController>();
            ic.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            ic.id = 0;
            ic.positionMatrix[0] = new Vector3(2.5f, 0.0f, 0.0f);
            ic.ini = true;
            ic.astronaut = astronaut;
            ic.bc = bc;
            ic.EndWall = EndWall;
            ins.transform.parent = transform;
            //ins.transform.Rotate(0.0f, 180.0f, 0.0f);

            GameObject ins2 = Instantiate(insect, astronaut.transform.position + control.positionMatrix[1], insect.transform.rotation);
            insectController ic2 = ins2.GetComponent<insectController>();
            ic2.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            ic2.id = 1;
            //ic2.positionMatrix[1] = new Vector3(-2.5f, 0.0f, 0.0f);
            ic2.ini = true;
            ic2.astronaut = astronaut;
            ic2.bc = bc;
            ic2.EndWall = EndWall;
            ins2.transform.parent = transform;
            //ins2.transform.Rotate(0.0f, 180.0f, 0.0f);

            GameObject ins3 = Instantiate(insect, astronaut.transform.position + control.positionMatrix[2], insect.transform.rotation);
            insectController ic3 = ins3.GetComponent<insectController>();
            ic3.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            ic3.id = 2;
            //ic3.positionMatrix[2] = new Vector3(2.5f, 0.0f, 0.0f);
            ic3.ini = true;
            ic3.astronaut = astronaut;
            ic3.bc = bc;
            ic3.EndWall = EndWall;
            ins3.transform.parent = transform;
            //ins3.transform.Rotate(0.0f, 180.0f, 0.0f);

            for (int i = 3; i < n; ++i)
            {
                Vector3 pos = control.positionMatrix[i];
                GameObject ins5 = Instantiate(insect, astronaut.transform.position + pos, insect.transform.rotation);
                insectController ic5 = ins5.GetComponent<insectController>();
                ic5.ini = true;
                ic5.id = i;
                ic5.astronaut = astronaut;
                ic5.bc = bc;
                ic5.EndWall = EndWall;
                ins5.transform.parent = transform;
                //ins5.transform.Rotate(0.0f, 180.0f, 0.0f);
            }
        }
        int ch = 0;
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            insectController ic = child.GetComponent<insectController>();
            if (ic.ini && !ic.dead)
            {
                ++ch;
            }
        }
        pc.insectsNum = news - ch;
        if (pc.insectsNum < 0) pc.insectsNum = 0;
    }
}
