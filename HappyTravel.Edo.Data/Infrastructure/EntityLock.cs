namespace HappyTravel.Edo.Data.Infrastructure
{
    public class EntityLock
    {
        public string EntityDescriptor { get; set; }
        public string LockerInfo { get; set; }
        public string Token { get; set; }
    }
}