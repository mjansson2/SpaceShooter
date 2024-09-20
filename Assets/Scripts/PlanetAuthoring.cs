using UnityEngine;
using Unity.Entities;
using System.Linq.Expressions;

public class PlanetAuthoring : MonoBehaviour
{
    class PlanetAuthoringBaker : Baker<PlanetAuthoring>
        {
        public override void Bake(PlanetAuthoring authoring)
        {
            Entity planetEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<PlanetTag>(planetEntity);

        }
    }
}

public struct PlanetTag : IComponentData { }


