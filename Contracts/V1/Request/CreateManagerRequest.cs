namespace ApiWorld.Contracts.V1.Request
{
    public class CreateManagerRequest
    {
        public string Name { get; set; }
        public bool IsManager { get; set; }
        public string Event { get; set; }
    }
}
