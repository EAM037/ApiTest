using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TractorController : MonoBehaviour
{
    public int tractorId;
    private int step = 0;
    public int speed = 1;
    public int rotationSpeed = 1;
    public int currentLoad = 0;
    public int maxLoad = 50;
    private API_Connect api;
    [SerializeField] List<(int, int)> pathValues;
    bool startFlag = false;
    bool endFlag = false;
    public Transform endPosition;
    public ContainerController ContainerController;
    public GameObject container;


    // Start is called before the first frame update
    void Start()
    {
        api = GetComponent<API_Connect>();
        api.DoRequest(tractorId);
        container = GameObject.Find("Container"+tractorId.ToString());
        ContainerController = container.GetComponent<ContainerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (endFlag)
        {
            startFlag = false;
            ContainerController.siloFlag = true;
            Vector3 endTarget = new Vector3(endPosition.position.x, transform.position.y, endPosition.position.z);
            Vector3 direction = (endTarget - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, endTarget) < 0.5f)
            {
                speed = 0;
            }
        }
        if (currentLoad >= maxLoad - 10)
        {
            Transform containerTransform = container.transform;
            ContainerController.followDistance = 1;
            ContainerController.speed = 20;
            if (Vector3.Distance(transform.position, containerTransform.position) < 2f)
            {
                ContainerController.siloFlag = true;
                currentLoad = 0;
                ContainerController.followDistance = 6;
            }
        }
        if (api.values.Count > 0 && !startFlag && !endFlag)
        {
            startFlag = true;
            Vector3 startTarget = new Vector3(api.values[step].Item1, transform.position.y, api.values[step].Item2);
            Vector3 direction = (startTarget - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, startTarget) < 0.5f)
            {
                step++;
                if (step >= api.values.Count)
                {
                    speed = 0;
                    rotationSpeed = 0;
                }
            }
            step++;
            StartCoroutine(UpdatePathConstantly());
            
        }
        if (api.values.Count > 0 && startFlag && !endFlag)
        {
            Vector3 target = new Vector3(api.values[step].Item1, transform.position.y, api.values[step].Item2);
            Vector3 direction = (target - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                step++;
                if (step >= api.values.Count)
                {
                    endFlag = true;
                }
            }

        }
    }

    IEnumerator UpdatePathConstantly()
    {
        while (api.connectionFlag)
        {
            api.DoRequest(tractorId);
            yield return new WaitForSeconds(1f);
        }
    }

}
