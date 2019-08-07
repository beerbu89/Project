using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestReward
{
    public int ReturnGem(int stage)
    {
        int gem = 0;

        var data = ExtensionMethod.GetQuestRewardData(stage);

        if (data == null) { gem = -1; }

        gem = data.nRewardGem;

        return gem;
    }
}
