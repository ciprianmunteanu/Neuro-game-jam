public class CampfireNodeController : MapNodeController
{
    protected override Rewards GetRewards()
    {
        return new Rewards() { Healing = 30 };
    }
}
