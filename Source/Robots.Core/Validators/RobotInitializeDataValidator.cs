using FluentValidation;
using Robots.Core.Models;

namespace Robots.Core.Validators
{
    public class RobotInitializeDataValidator : AbstractValidator<RobotInitializeData>
    {
        public RobotInitializeDataValidator()
        {
            RuleFor(data => data.x)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(Coordinates.MAX_COORDINATE);

            RuleFor(data => data.y)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(Coordinates.MAX_COORDINATE);
        }
    }
}
