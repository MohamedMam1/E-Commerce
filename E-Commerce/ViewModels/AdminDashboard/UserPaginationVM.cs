namespace E_Commerce.ViewModels.AdminDashboard
{
    public class UserPaginationVM
    {
        public List<UserDashBoardVM> Users { get; set; } = new List<UserDashBoardVM>();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                {
                    return 0;
                }

                return (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}

