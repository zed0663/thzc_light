using System.Collections.Generic;
using SimpleJSON;

public class RedBagData
{
    public int invitationCount;
    public int adsCount;
    public int signInDays;
    public int redBagCoins;
    public List<WithdrawalData> withdrawalDatas;

    public RedBagData(JSONNode data)
    {
        invitationCount = data["invitationCount"].AsInt;
        adsCount = data["adsCount"].AsInt;
        signInDays = data["signInDays"].AsInt;
        redBagCoins = data["redBagCoins"].AsInt;
        var withdrawal = data["withdrawal"].AsArray;
        withdrawalDatas = new List<WithdrawalData>();
        foreach (JSONNode w in withdrawal)
        {
            WithdrawalData withdrawalData = new WithdrawalData(w);
            withdrawalDatas.Add(withdrawalData);
        }
    }
}
