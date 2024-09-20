using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]

[BurstCompile]
public partial struct FireProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        float3 playerOffset = new float3(0f, 20f, 0f);

        foreach (var (projectilePrefab, transform, lifetime) in SystemAPI.Query<ProjectilePrefab, LocalTransform, ProjectileLifeTime>().WithAll<FireProjectileTag>())
        {
            var newProjectile = ecb.Instantiate(projectilePrefab.Value);
            float3 spawnPosition = transform.Position + playerOffset;

            var projectileTransform = LocalTransform.FromPositionRotation(spawnPosition, transform.Rotation);

            ecb.SetComponent(newProjectile, projectileTransform);
            ecb.AddComponent(newProjectile, new LifeTime
            {
                Value = lifetime.Value
            });
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
