using Pawpal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pawpal.Services
{
    public interface IFeedingTimeService
    {
        Task<List<FeedingTimeEntry>> GetAllFeedingTimeAsync();
        Task AddFeedingTimeAsync(FeedingTimeEntry entry);
        Task DeleteFeedingTimeAsync(int id);
    }
}
