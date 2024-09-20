using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;


[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Spawner>();
    }
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

        new SpawnerJob
        {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            ecb = ecb
        }.Schedule();

        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

    }

    [BurstCompile]
    public partial struct SpawnerJob : IJobEntity
    {
        public EntityCommandBuffer ecb;
        public double ElapsedTime;

        [BurstCompile]
        public void Execute(Entity entity, ref Spawner spawner, DynamicBuffer<SpawnerPrefabBuffer> prefabBuffer)
        {

            if (spawner.NextSpawnTime < ElapsedTime)
            {
                if (spawner.PrefabCount >= spawner.MaxPrefabCount)
                {
                    spawner.PrefabCount = 0;
                    spawner.CurrentPrefabIndex++;
                    spawner.MaxPrefabCount++;

                    if (spawner.CurrentPrefabIndex >= prefabBuffer.Length)
                    {
                        spawner.CurrentPrefabIndex = 0;
                    }
                }

                Entity prefabToSpawn = prefabBuffer[spawner.CurrentPrefabIndex].PrefabEntity;
                Entity newEntity = ecb.Instantiate(prefabToSpawn);
                ecb.SetComponent(newEntity, LocalTransform.FromPosition(spawner.SpawnPosition));

                ecb.AddComponent(newEntity, new LifeTime
                {
                    Value = spawner.PlanetLifeTime
                });

                spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
                spawner.PrefabCount++;

                ecb.SetComponent(entity, spawner);
            }
        }
    }
}