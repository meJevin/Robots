using FluentValidation;
using Robots.Core.Enums;
using Robots.Core.Exceptions;

namespace Robots.Core.Models
{
    public record MarsSurfaceInitializeData(int Width, int Height);

    public record ProhibitedCell(int X, int Y, Orientation Orientation);

    public interface IMarsSurface
    {
        int Width { get; }
        int Height { get; }

        IReadOnlyList<IRobot> Robots { get; }
        IReadOnlyList<ProhibitedCell> ProhibitedCells { get; } // Using a dictionary would be faster but whatever...

        void AddRobot(IRobot robot);
        void AddProhibitedCell(ProhibitedCell prohibitedCell);

        void Initialize(MarsSurfaceInitializeData initializeData);
    }

    public class MarsSurface : IMarsSurface
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<IRobot> _robots;
        public IReadOnlyList<IRobot> Robots => _robots;

        private List<ProhibitedCell> _prohibitedCells;
        public IReadOnlyList<ProhibitedCell> ProhibitedCells => _prohibitedCells;

        private readonly IValidator<MarsSurfaceInitializeData> _marsSurfaceInitializationValidator;
        private bool _isInitialized;

        public MarsSurface(IValidator<MarsSurfaceInitializeData> marsSurfaceInitializationValidator)
        {
            _marsSurfaceInitializationValidator = marsSurfaceInitializationValidator;

            _robots = new List<IRobot>();
            _prohibitedCells = new List<ProhibitedCell>();
        }

        public void AddRobot(IRobot robot)
        {
            if (!_isInitialized)
            {
                throw new MarsSurfaceException("Mars surface hasn't been initialized");
            }

            if (_robots.Contains(robot))
            {
                throw new MarsSurfaceException("Robot added already exists on the surface of mars");
            }

            _robots.Add(robot);
        }

        public void AddProhibitedCell(ProhibitedCell prohibitedCell)
        {
            var found = _prohibitedCells.FirstOrDefault(pC => pC == prohibitedCell);

            if (found is not null) return;

            _prohibitedCells.Add(prohibitedCell);
        }

        public void Initialize(MarsSurfaceInitializeData initializeData)
        {
            if (_isInitialized)
            {
                throw new MarsSurfaceException("Mars surface has already been initialized");
            }

            _marsSurfaceInitializationValidator.ValidateAndThrow(initializeData);

            Width = initializeData.Width;
            Height = initializeData.Height;

            _isInitialized = true;
        }
    }
}
