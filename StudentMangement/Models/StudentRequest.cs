namespace StudentMangement.Models
{
    public class StudentRequest
    {
        public  string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Dirth { get; set; }
        public IFormFile ImageFile { get; set; } 
    }
}
