using Robots.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robots.Application.Services
{
    public interface IRobotInstructionFactory
    {
        IRobotInstruction FromName(string name);
    }

    public class RobotInstructionFactory : IRobotInstructionFactory
    {
        List<IRobotInstruction> _availableInstructions;

        public RobotInstructionFactory()
        {
            _availableInstructions = new List<IRobotInstruction>();

            foreach (var instructionType in typeof(IRobotInstruction).Assembly
                .GetTypes().Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(IRobotInstruction))))
            {
                _availableInstructions.Add((Activator.CreateInstance(instructionType) as IRobotInstruction)!);
            }
        }

        public IRobotInstruction FromName(string name)
        {
            var found = _availableInstructions.FirstOrDefault(x => x.Name == name);

            if (found is null)
            {
                throw new ArgumentException($"Could not find robot instruction with name {name}");
            }

            return found;
        }
    }
}
