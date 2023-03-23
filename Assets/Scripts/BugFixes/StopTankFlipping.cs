using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTankFlipping : MonoBehaviour
{
    private Transform tank;
    // Start is called before the first frame update
    void Start()
    {
        tank = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //await Task.Delay(2000);
        //if (tank.rotation.y < 30)
        //{
        //    tank.rotation = new Quaternion(tank.rotation.x, 0, tank.rotation.z, 1);
        //}
        //if (tank.rotation >= new Vector3(30.0f, 30.0f, 30.0f))
        //{
        //    Debug.Log("aaaaa");
        //}
    }
}
