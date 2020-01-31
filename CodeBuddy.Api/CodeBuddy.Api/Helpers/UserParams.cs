using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBuddy.Api.Helpers
{
    public class UserParams
    {
        private const int maximumSize = 50;
        private int pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > maximumSize) ? maximumSize : value; }
        }

    }
}
