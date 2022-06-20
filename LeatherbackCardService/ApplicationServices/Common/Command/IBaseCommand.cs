namespace ApplicationServices.Common.Command
{
    public interface IBaseCommand
    {
        public string Channel { get; set; }
        public string Currency { get; set; } 
    }
}