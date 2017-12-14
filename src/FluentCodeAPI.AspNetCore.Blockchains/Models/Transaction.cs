using Newtonsoft.Json;
using System;

namespace FluentCodeAPI.AspNetCore.Blockchains.Models
{
    /// <summary>
    /// Represents the <see cref="Transaction"/>.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the <see cref="Sender"/> as a <see cref="String"/>.
        /// </summary>
        [JsonProperty(Order = 1, NullValueHandling = NullValueHandling.Include, PropertyName = "sender")]
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Recipient"/> as a <see cref="String"/>.
        /// </summary>
        [JsonProperty(Order = 2, NullValueHandling = NullValueHandling.Include, PropertyName = "recipient")]
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Amount"/> as a <see cref="Int32"/>.
        /// </summary>
        [JsonProperty(Order = 3, NullValueHandling = NullValueHandling.Include, PropertyName = "amount")]
        public int Amount { get; set; }
    }
}