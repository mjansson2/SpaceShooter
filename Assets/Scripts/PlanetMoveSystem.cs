using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using UnityEngine;
using Unity.Mathematics;

[UpdateBefore(typeof(TransformSystemGroup))]

public partial struct PlanetMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        new PlanetMoveJob
        {
            DeltaTime = deltaTime,
            SourcePosition = new float3(-200f, 0f, 0f),
            TargetPosition = new float3(200f, 0f, 0f),
            Speed = 25f

        }.Schedule();
    }
}

[BurstCompile]
public partial struct PlanetMoveJob : IJobEntity
{
    public float DeltaTime;
    public float3 SourcePosition;
    public float3 TargetPosition;
    public float Speed;


    private void Execute(ref LocalTransform transform, in PlanetTag planetTag)
    { 
        float3 direction = TargetPosition - transform.Position;
        float distance = math.length(direction);

        float step = Speed * DeltaTime;

        if (distance > step)
        {
            transform.Position += math.normalize(direction) * step;
        }
        else
        {
            transform.Position = TargetPosition;
        }
     
    }
}
