﻿using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = nameof(ViewEntityBehaviors), menuName = Assets.TEMPLATES + "/" + nameof(ViewEntityBehaviors))]
public class ViewEntityBehaviors : ScriptableObject {
	private static readonly System.Random random = new();

	public void ViewNext(Entity entity) {
		++entity.ViewSelection;
	}

	public void ViewRandom(Entity entity) {
		int selection = random.Next(entity.ViewCount - 1);

		entity.ViewSelection = selection < entity.ViewSelection ? selection : selection + 1;
	}

	public void ViewByEpoch(Entity entity) {
		entity.ViewSelection = (int)(entity.ViewCount * ((float)GameManager.Epoch / GameManager.EpochCount));
	}
}