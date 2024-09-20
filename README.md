# SpaceShooter
 Assignment:  Create a small space shooter game with the following features: Simple movement, Shooting, Waves of enemies. Using Unity's DOTS system

Object-oriented vs Data-oriented Programming:
In the project there is a planet movement system that run on entities that contain the PlanetTag component. All the logic for moving the entities that are spawned is contained within that ISystem. The functions within planet movement system have the attribute BurstCompile to make sure that the logic is transformed into low level machine code for faster execution. To achieve a better performance when potentially scaling up the amount of planet entities that are being spawned, DOP allows for having the movement system operate in job threads instead of the main thread. OOP would have each move function that is contained within each GameObject's MonoBehvaiour script executed on the main thread, which leads to the main thread struggling to keep up as it treats each object one by one resulting in frame drops if the amount of objects to process is particularly high.

Spawning logic is treated in the background, and not run on the main thread. Meaning the program doesn't have to wait for whenever a large amount of entities are being spawned to carry on running the main logic of the program, such as transforms and rendering. It is also used with an EntityCommandBuffer for optimization. With the entity command buffer a command will not happen everytime something changes, rather when ecb.Playback() is called to minimize syncing points that are costly for the CPU.

DynamicBuffer makes sure that data is stored in chunk memory and allows for resizing that is less costly than, for example, finding a new space and copying identical data for each object, causing more frequent memory reallocations. This is used with the SpawnerAuthoring script to be able to store different prefabs for a variation of "enemy-planets". And are then spawned based on the logic within SpawnerSystem. 

In LifeTimeManagement system the actual calculations are scheduled through an IJobEntity, meaning not executed on the main thread. The Update function does not run unless there are entities with a LifeTime component present in the SubScene. 

To have entities be destroyed, a lifetime component is added to the entity that is being spawned. The prefab then has it's own lifetime value that can be modified through the prefab's MonoBehaviour script, the authoring script. That value is then taken into that newly added lifetime component to process based on the conditions set in LifeTimeManagementSystem. That system tags the entity when that lifetime value is less than 0. With the tag it is then processed by the IsDestroyManagement system, which only job is to destroy any entity that contains the tag IsDestroying.

ISystem:

- FireProjectileSystem
- IsDestroyManagementSystem
- LifeTimeManagementSystem
- PlanetMoveSystem
- PlayerMoveSystem
- ResetInputSystem
- SpawnerSystem

SystemBase:
- PlayerInputSystem

IComponentData:
- PlanetAuthoring
	PlanetTag
- PlayerAuthoring
	PlayerMoveInput
	PlayerMoveSpeed
	PlayerTag
	ProjectilePrefab
	ProjectileMoveSpeed
	FireProjectileTag
	ProjectileLifeTime
	LifeTime
	IsDestroying
- ProjectileAuthoring
- SpawnerAuthoring
	Spawner
	SpawnerPrefabBuffer

Instantiate entities in:
- SpawnerSystem (PrefabEntity from prefabBuffer)
- FireProjectileSystem (projectilePrefab)

Jobs in:
- PlanetMoveSystem
- LifeTimeManagementSystem (ecb)
- PlayerMoveSystem
- SpawnerSystem (ecb)








