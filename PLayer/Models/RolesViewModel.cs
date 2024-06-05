namespace PLayer.Models
{
    public class RolesViewModel
    {
        public RolesViewModel()
        {
            Id=Guid.NewGuid().ToString();
        }
        public string Id { get; set; }  
        public string RoleName { get; set; }    

    }
}
