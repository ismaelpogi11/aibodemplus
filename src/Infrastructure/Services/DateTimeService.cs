using Medobia.Application.Common.Interfaces;
using System;

namespace Medobia.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
