namespace ETICARET.WebUI.Models
{
    public record NotifyMessage(string Text, string Type = "success")
    {
        public const string Key = "Notify";
    }
   
}
