﻿using NUnit.Framework;
using ShootAR;
using UnityEngine.TestTools;

public class ObjectPoolTests : ShootAR.TestTools.TestBase
{
	[Test]
	public void NoAvailableObjectToFetch() {
		const int limit = 3;
		Spawnable.Pool<Capsule>.Populate(Capsule.Create(0, 0), limit);

		for (int i = 0; i < limit; i++)
			Spawnable.Pool<Capsule>.RequestObject();

		Assert.IsNull(Spawnable.Pool<Capsule>.RequestObject(),
				"A null reference should have been returned.");
	}

	[UnityTest]
	public System.Collections.IEnumerator CreateNewObjectWhenPoolEmpty() {
		const int limit = 3;
		Spawner spawner = Spawner.Create(
				Capsule.Create(0, 0), limit, 0, 0, 10, 0);
		Spawnable.Pool<Capsule>.Populate((Capsule)spawner.ObjectToSpawn);

		spawner.StartSpawning();
		yield return new UnityEngine.WaitWhile(() => spawner.IsSpawning);
		spawner.StartSpawning(1);
		yield return new UnityEngine.WaitWhile(() => spawner.IsSpawning);

		int	expectedValue = limit + 1,
			actualValue = UnityEngine.Object.FindObjectsOfType<Capsule>().Length - 1;
		Assert.AreEqual(expectedValue, actualValue,
				$"Expected {expectedValue} spawned objects but got {actualValue}.");
	}

	[Test]
	public void ObjectReturnsToPool() {
		Spawnable.Pool<Capsule>.Populate(Capsule.Create(0, 0), 1);
		Capsule testObject = Spawnable.Pool<Capsule>.RequestObject();

		testObject.ReturnToPool<Capsule>();

		Assert.AreEqual(1, Spawnable.Pool<Capsule>.Count,
				"No object available in the pool.");
		Assert.IsNotNull(Spawnable.Pool<Capsule>.RequestObject(),
				"No object available to return.");
	}
}