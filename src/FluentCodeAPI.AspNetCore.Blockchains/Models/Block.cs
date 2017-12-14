using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FluentCodeAPI.AspNetCore.Blockchains.Models
{
    /// <summary>
    /// Represents the <see cref="Block"/>.
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Gets or sets the <see cref="Index"/> as a <see cref="Int32"/>.
        /// </summary>
        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Include, PropertyName = "index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Timestamp"/> as a <see cref="Int32"/>.
        /// </summary>
        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Include, PropertyName = "timestamp")]
        public int Timestamp { get; set; } = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        /// <summary>
        /// Gets or sets the <see cref="Transactions"/> as a <see cref="IList{Models.Transaction}"/>.
        /// </summary>
        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Include, PropertyName = "transactions")]
        public IList<Transaction> Transactions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Proof"/> as a <see cref="Int32"/>.
        /// </summary>
        [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Include, PropertyName = "proof")]
        public int Proof { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="PreviousHash"/> as a <see cref="String"/>.
        /// </summary>
        [JsonProperty(Order = 5, NullValueHandling = NullValueHandling.Include, PropertyName = "previous_hash")]
        public string PreviousHash { get; set; }
    }
}