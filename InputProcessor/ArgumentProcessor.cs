﻿using CliArgumentProcessor.ArgumentProcessors;
using CliArgumentProcessor.Commands;
using System.Collections.Generic;

namespace CliArgumentProcessor
{
    /// <summary>
    /// Gets the CLI arguments and converts them into internal commands.
    /// </summary>
    public class ArgumentProcessor
    {
        /// <summary>
        /// Collection of argument processors. The first that successfully resolves the arguments provides the internal command.
        /// </summary>
        private readonly List<ArgumentProcessorBase> _argumentProcessors;

        public IReadOnlyCollection<ArgumentProcessorBase> ArgumentProcessors { get => _argumentProcessors.AsReadOnly(); }

        /// <summary>
        /// Setup the basic processors (Copy folder, exit, help).
        /// </summary>
        public ArgumentProcessor()
        {
            _argumentProcessors = new List<ArgumentProcessorBase>
            {
                new CopyFolderArgumentProcessor(),
                new ExitArgumentProcessor(),
                new HelpArgumentProcessor()
            };
        }

        public ICommand ProcessArguments(string[] args)
        {
            foreach (var argumentProcessor in _argumentProcessors)
            {
                var command = argumentProcessor.GetCommand(args);
                if (!(command is NotCommand))
                {
                    return command;
                }
            }

            return new NotCommand();
        }
    }
}
