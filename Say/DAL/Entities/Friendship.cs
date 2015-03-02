namespace DAL.Entities
{
    public class Friendship
    {
        public int FriendshipID { get; set; }
        public int Friend1 { get; set; }
        public int Friend2 { get; set; }
        public bool Waiting { get; set; }
    }
}
