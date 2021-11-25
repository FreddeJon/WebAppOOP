using WebAppOOP.Core.ModelDTOS.Interfaces;

namespace WebAppOOP.Core.ModelDTOS
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
    }
}
