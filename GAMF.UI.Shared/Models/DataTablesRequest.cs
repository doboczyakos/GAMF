#if MVC
using Microsoft.AspNetCore.Mvc;
#endif

namespace GAMF.Web.Models
{
    public class DataTablesRequest
    {
        public int Draw { get; init; }

#if MVC
        [FromQuery(Name = "search[value]")]
#endif
        public string? SearchString { get; set; }

        public int Start { get; set; }
        public int Length { get; set; }

#if MVC
        [FromQuery(Name = "columns")]
#endif
        public required DataTablesColumn[] Columns { get; init; }

#if MVC
        [FromQuery(Name = "order")]
#endif
        public required DataTablesOrder[] Order { get; set; }
    }

    public class DataTablesColumn
    {
#if MVC
        [FromQuery(Name = "[data]")]
#endif
        public required string Data { get; init; }
    }

    public class DataTablesOrder
    {
#if MVC
        [FromQuery(Name = "[column]")]
#endif
        public int Column { get; init; }

#if MVC
        [FromQuery(Name = "[dir]")]
#endif
        public OrderDirection Direction { get; init; }
    }

    public enum OrderDirection { Asc, Desc }
}
