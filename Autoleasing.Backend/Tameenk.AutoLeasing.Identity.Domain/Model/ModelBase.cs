namespace Tameenk.AutoLeasing.Identity.Domain
{ 
    public abstract class ModelBase
    {
        public Channel Channel { get; set; } = Channel.Portal;
        public string Language { get; set; } = "ar";
    }

    public enum Channel
    {
        Portal = 1,
        Mobile = 2,
        Dashboard = 3
    }
}
