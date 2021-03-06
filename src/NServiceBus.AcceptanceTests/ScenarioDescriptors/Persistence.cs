﻿namespace NServiceBus.AcceptanceTests.ScenarioDescriptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AcceptanceTesting.Support;
    using NServiceBus.Persistence;

    public static class Persistence
    {
        public static IEnumerable<RunDescriptor> AllAvailable
        {
            get
            {
                if (availablePersisters == null)
                {
                    availablePersisters = GetAllAvailable().ToList();
                }

                return availablePersisters;
            }
        }


        public static RunDescriptor Default
        {
            get
            {
                var specificPersistence = Environment.GetEnvironmentVariable("Persistence.UseSpecific");

                if (!string.IsNullOrEmpty(specificPersistence))
                    return AllAvailable.Single(r => r.Key == specificPersistence);

                var persistenceOtherThanInMemory = AllAvailable.Where(t => t != InMemory).ToList();

                if (persistenceOtherThanInMemory.Count() == 1)
                    return persistenceOtherThanInMemory.First();

                return InMemory;
            }
        }

        static RunDescriptor InMemory
        {
            get { return AllAvailable.SingleOrDefault(r => r.Key == "InMemory"); }
        }

        static IEnumerable<RunDescriptor> GetAllAvailable()
        {
            var foundDefinitions = TypeScanner.GetAllTypesAssignableTo<PersistenceDefinition>();

            
            foreach (var definition in foundDefinitions)
            {
                var key = definition.Name;

                var runDescriptor = new RunDescriptor
                {
                    Key = key,
                    Settings =
                        new Dictionary<string, string>
                                {
                                    {"Persistence", definition.AssemblyQualifiedName}
                                }
                };

                yield return runDescriptor;
            }
        }

        static IList<RunDescriptor> availablePersisters;

    }
}