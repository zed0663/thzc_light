using SimpleJSON;

public class WithdrawalData
{
    //id
    public int id;
    //提现时间
    public int timeStamp;
    //提现金额
    public int currentRecord;
    //是否提现
    public bool isGet;
    //金额
    public int money;

    public WithdrawalData(JSONNode json)
    {
        id = json["id"].AsInt;
        timeStamp = json["time"].AsInt;
        money = json["money"].AsInt;
        currentRecord = json["currentRecord"].AsInt;
        isGet = json["isGet"].AsBool;
    }
}
