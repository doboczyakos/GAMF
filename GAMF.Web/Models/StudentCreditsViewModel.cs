using System.ComponentModel.DataAnnotations;
using GAMF.Core;

namespace GAMF.Web.Models
{
    public class StudentCreditsViewModel
    {
        [Display(Name = nameof(Resources.Student), ResourceType = typeof(Resources))]
        public required string Name { get; init; }
        [Display(Name = nameof(Resources.Credits), ResourceType = typeof(Resources))]
        public required int Credits { get; init; }
    }
}
