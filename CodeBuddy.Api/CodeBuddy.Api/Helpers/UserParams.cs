namespace CodeBuddy.Api.Helpers
{
    public class UserParams
    {
        private const int maximumSize = 50;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maximumSize) ? maximumSize : value; }
        }

        public int UserId { get; set; }
        public string Gender { get; set; }
        public int PageNumber { get; set; } = 1;
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;


    }
}
