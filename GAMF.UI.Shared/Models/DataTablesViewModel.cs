namespace GAMF.Web.Models
{
    public class DataTablesViewModel<TViewModel>
        where TViewModel : class
    {
        public int Draw { get; init; }

        public required int RecordsFiltered { get; init; }
        public required int RecordsTotal { get; init; }

        public required IEnumerable<TViewModel> Data { get; init; }
    }
}
