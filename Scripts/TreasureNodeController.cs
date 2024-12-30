using System.Collections.Generic;
using System.Linq;

public class TreasureNodeController : MapNodeController
{
    protected override Rewards GetRewards()
    {
        List<Item> itemRewards = new()
        {
            GetRandomItemReward(),
            GetRandomItemReward()
        };

        var rew = new Rewards() { Items = itemRewards };

        return rew;
    }
}
