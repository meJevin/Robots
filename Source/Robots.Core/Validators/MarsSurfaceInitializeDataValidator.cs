using FluentValidation;
using Robots.Core.Models;

namespace Robots.Core.Validators
{
    public class MarsSurfaceInitializeDataValidator 
        : AbstractValidator<MarsSurfaceInitializeData>
    {
        public MarsSurfaceInitializeDataValidator()
        {
            RuleFor(options => options.Width)
                .GreaterThan(0)
                .LessThanOrEqualTo(Coordinates.MAX_COORDINATE);

            RuleFor(options => options.Height)
                .GreaterThan(0)
                .LessThanOrEqualTo(Coordinates.MAX_COORDINATE);
        }
    }
}
