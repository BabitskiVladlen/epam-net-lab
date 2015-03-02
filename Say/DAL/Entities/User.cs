namespace DAL.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Image { get; set; }
        public byte[] CompressedImage { get; set; }
        public string ImageMimeType { get; set; }

    }
}
