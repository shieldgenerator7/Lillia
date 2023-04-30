using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

//2022-08-11: followed this tutorial: https://www.raywenderlich.com/7880445-unity-job-system-and-burst-compiler-getting-started
public class FireflyManager : MonoBehaviour
{
    public float moveSpeed;

    public List<Transform> fireFlies;

    private TransformAccessArray transformAccessArray;
    private UpdateFireflyJob updateFireflyJob;
    private JobHandle updateFireflyJobHandle;

    // Start is called before the first frame update
    void Start()
    {
        transformAccessArray = new TransformAccessArray(fireFlies.Count);
        fireFlies.ForEach(t => transformAccessArray.Add(t));
    }

    private void OnDestroy()
    {
        transformAccessArray.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        updateFireflyJob = new UpdateFireflyJob()
        {
            moveSpeed = this.moveSpeed,
            jobDeltaTime = Time.deltaTime,
            time = Time.time,
            seed = System.DateTimeOffset.Now.Millisecond,
        };
        updateFireflyJobHandle = updateFireflyJob.Schedule(transformAccessArray);
    }

    private void LateUpdate()
    {
        updateFireflyJobHandle.Complete();
    }

    [BurstCompile]
    private struct UpdateFireflyJob : IJobParallelForTransform
    {
        public float moveSpeed;

        public float jobDeltaTime;
        public float time;
        public float seed;

        public void Execute(int index, TransformAccess transform)
        {
            Unity.Mathematics.Random randomGen = new Unity.Mathematics.Random(
                (uint)(index * time + 1 + seed)
                );
            float primerFloat = randomGen.NextFloat(-1.0f, 1.0f);//call it once first just to prime it
            Vector2 moveDir = new Vector2(
                randomGen.NextFloat(-1.0f, 1.0f),
                randomGen.NextFloat(-1.0f, 1.0f)
                );
            transform.position += (Vector3)moveDir.normalized * moveSpeed * jobDeltaTime;
        }
    }
}
