using System.ComponentModel.DataAnnotations;

namespace ApiWorld.Domain
{
    public class Manager
    {
        [Key]
        public string ManagerId { get; set; }
        public string IsCurrentManager { get; set; }
        public string Event { get; set; }
    }
}
