using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Bookings;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public interface IBookingsProcessingService
    {
        Task<Result<List<int>>> GetForCancellation(DateTime deadlineDate);

        Task<Result<ProcessResult>> Cancel(List<int> bookingIds);
    }
}