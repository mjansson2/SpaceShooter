using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour
{
    public GameObject[] PlanetPrefabs;
    public float SpawnRate;
    public float PlanetLifeTime;
    public int MaxPrefabCount;

    class SpawnerBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Spawner
            {
                SpawnPosition = new float3(-200f, 0f, 0f),
                NextSpawnTime = 0,
                SpawnRate = authoring.SpawnRate,
                PlanetLifeTime = authoring.PlanetLifeTime,
                MaxPrefabCount = authoring.MaxPrefabCount
            });

            DynamicBuffer<SpawnerPrefabBuffer> prefabBuffer = AddBuffer<SpawnerPrefabBuffer>(entity);

            foreach(var prefab in authoring.PlanetPrefabs)
            {
                prefabBuffer.Add(new SpawnerPrefabBuffer
                {
                    PrefabEntity = GetEntity(prefab, TransformUsageFlags.Dynamic)
                });
            }

        }
    }
}

public struct Spawner : IComponentData
{
    public float3 SpawnPosition;
    public float NextSpawnTime;
    public float SpawnRate;
    public int CurrentPrefabIndex;
    public float PlanetLifeTime;
    public int PrefabCount;
    public int MaxPrefabCount;
}

public struct SpawnerPrefabBuffer : IBufferElementData
{
    public Entity PrefabEntity;
}


