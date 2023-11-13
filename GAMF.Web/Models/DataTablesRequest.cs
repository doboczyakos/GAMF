using Microsoft.AspNetCore.Mvc;

namespace GAMF.Web.Models
{
    public class DataTablesRequest
    {
        public int Draw { get; init; }

        [FromQuery(Name = "search[value]")]
        public string? SearchString { get; init; }

        public int Start { get; init; }
        public int Length { get; init; }

        [FromQuery(Name = "columns")]
        public required DataTablesColumn[] Columns { get; init; }

        [FromQuery(Name = "order")]
        public required DataTablesOrder[] Order { get; init; }
    }

    public class DataTablesColumn
    {
        [FromQuery(Name = "[data]")]
        public required string Data { get; init; }
    }

    public class DataTablesOrder
    {
        [FromQuery(Name = "[column]")]
        public int Column { get; init; }

        [FromQuery(Name = "[dir]")]
        public OrderDirection Direction { get; init; }
    }
}
