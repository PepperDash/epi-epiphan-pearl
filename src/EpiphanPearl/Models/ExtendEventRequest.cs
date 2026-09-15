using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Newtonsoft.Json;

namespace PepperDash.Essentials.Plugins.Models
{
    /// <summary>
    /// Body for <c>POST /schedule/events/{id}/control/extend</c>.
    /// </summary>
    /// <remarks>
    /// <para><c>finish</c> is the number of <b>seconds to add</b> to the event's current finish
    /// time — the API's own words, with an example of <c>300</c>. It is not an absolute time.</para>
    /// <para>This previously carried a <c>DateTime</c> serialised through
    /// <c>SecondEpochConverter</c>, which wrote a Unix epoch — roughly 1.79 billion — so every
    /// extend asked the Pearl to add about fifty-six years. The API rejects an extension that
    /// overlaps the next scheduled event, so extending has never worked.</para>
    /// </remarks>
    public class ExtendEventRequest
    {
        [JsonProperty("finish")]
        public int Finish { get; set; }
    }
}