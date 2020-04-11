﻿using System.IO;
using NUnit.Framework;
using UnityEngine;
using ShootAR;
using ShootAR.TestTools;
using static ShootAR.Spawner;

public class PatternsFileTests : TestBase
{
	private const string PATTERN_FILE = "patternstestfile.xml";

	[TearDown]
	public void DeletePatternFile() {
		if (File.Exists(PATTERN_FILE))
			File.Delete(PATTERN_FILE);

		Assert.That(
			!File.Exists(PATTERN_FILE),
			"The file should be deleted when the test ends."
		);
	}

	[Test]
	public void CopyFileToPermDataPath() {
		const string patternFileBasename = "spawnpatterns";
		const string patternFile = patternFileBasename + "-test.xml";
		string targetFile = Path.Combine(Application.persistentDataPath, patternFile);

		LocalFiles.CopyResourceToPersistentData(patternFileBasename, patternFile);

		Assert.That(File.Exists(targetFile));
		File.Delete(patternFile);
	}

	[Test]
	public void ExtractPattern() {
		var patterns = Spawner.ParseSpawnPattern("Assets/Resources/spawnpatterns.xml");

		Assert.IsNotEmpty(patterns, "No patterns extracted.");
	}

	//TODO: Write tests catching failures when patterns' file contains errors.

	//TODO: Test case of XML file containing duplicate nodes of a spawnable type.

	//TODO: Test case of XML file containing the "repeat" attribute.

	/// <summary>
	/// Test case of XML file containing a spawnable type that is not valid.
	/// </summary>
	[Test]
	public void InvalidSpawnable() {
		string invalidSpawnable = "Boogeroo";

		string[] data = new string[] {
			"<spawnerconfiguration>",
			"\t<level>",
			$"\t\t<spawnable type=\"{invalidSpawnable}\">",
			"\t\t\t<pattern>",
			"\t\t\t\t<limit>1</limit>",
			"\t\t\t\t<rate>0</rate>",
			"\t\t\t\t<delay>0</delay>",
			"\t\t\t\t<maxDistance>0</maxDistance>",
			"\t\t\t\t<minDistance>0</minDistance>",
			"\t\t\t</pattern>",
			"\t\t</spawnable>",
			"\t</level>",
			"</spawnerconfiguration>"
		};

		File.WriteAllLines(PATTERN_FILE, data);

		var error = $"Error in {PATTERN_FILE}:\n" +
					$"{invalidSpawnable} is not a valid type of spawnable.";

		UnityException ex = Assert.Throws<UnityException>(() =>
			ParseSpawnPattern(PATTERN_FILE)
		);

		Assert.That(ex.Message, Is.EqualTo(error));
	}

	// var spawners = new Dictionary<Type, List<Spawner>>();
	// var stashedSpawners = new Stack<Spawner>();

	// SpawnerFactory(
	// 	new Stack<SpawnConfig>[] {
	// 		new Stack<SpawnConfig>(
	// 			new SpawnConfig[1] {
	// 				new SpawnConfig(typeof(Capsule), 1, 0f, 0f, 0f, 0f)
	// 			}
	// 		)
	// 	},
	// 	0,
	// 	ref spawners,
	// 	ref stashedSpawners
	// );

	//TODO: Test if stashing remaining spawners works correctly.

	//TODO: Test if SpawnerFactory sets up spawners correctly.
}
