﻿using Microsoft.Azure.Cosmos;

namespace PropVivo.Infrastructure.Interfaces
{
    public interface ICosmosDbContainer
    {
        /// <summary>
        /// Instance of Azure Cosmos DB Container class
        /// </summary>
        Container _container { get; }
    }
}