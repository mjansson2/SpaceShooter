using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerMoveSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        new PlayerMoveJob
        {
            DeltaTime = deltaTime
        }.Schedule();
    }
}

[BurstCompile]
public partial struct PlayerMoveJob : IJobEntity
{
    public float DeltaTime;
    private void Execute(ref LocalTransform transform, in PlayerMoveInput input, PlayerMoveSpeed speed)
    {
        transform.Position.xy += input.Value * speed.Value * DeltaTime;
    }
}
