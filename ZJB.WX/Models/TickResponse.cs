namespace ZJB.WX.Models
{
    public class TickResponse
    {
        public Action Action { get; set; }
        public object Data { get; set; }
    }

    public enum Action
    {
        Nothing = 1,
        NewHouseInfo = 2,
        UpdateApp = 3,
        LoadAssembly = 4,
        ForwardRequest = 5
    }
}