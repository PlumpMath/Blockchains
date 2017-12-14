using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FluentCodeAPI.AspNetCore.Blockchains.Internal
{
    /// <summary>
    /// Represents a <see cref="Web"/> client builded on top of <see cref="HttpWebRequest"/> for ease of use and modularity.
    /// </summary>
    public class Web
    {
        /// <summary>
        /// Get the response value from the requested uri string.
        /// </summary>
        /// <param name="requestUriString">The Uri as a <see cref="String"/></param>
        /// <returns>The response value as a <see cref="String"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if the argument is null or empty.</exception>
        /// <exception cref="WebException">Thrown if there was an error getting the response.</exception>
        public static async Task<string> GetAsync(string requestUriString)
        {
            if (string.IsNullOrEmpty(requestUriString))
            {
                throw new ArgumentNullException(nameof(requestUriString));
            }

            var request = CreateHttpWebRequest("GET", requestUriString);

            return await GetResponseAsync(request);
        }

        /// <summary>
        /// Create the <see cref="HttpWebRequest"/>.
        /// </summary>
        /// <param name="method">The HTTP Verb as a <see cref="String"/></param>
        /// <param name="requestUriString">The Uri as a <see cref="String"/></param>
        /// <returns>The <see cref="HttpWebRequest"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if one of the argument is null or empty.</exception>
        private static HttpWebRequest CreateHttpWebRequest(string method, string requestUriString)
        {
            if (string.IsNullOrEmpty(method))
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (string.IsNullOrEmpty(requestUriString))
            {
                throw new ArgumentNullException(nameof(requestUriString));
            }

            var request = (HttpWebRequest)WebRequest.Create(requestUriString);

            request.Method = method;

            return request;
        }

        /// <summary>
        /// Get the response from the request.
        /// </summary>
        /// <param name="request"><see cref="HttpWebRequest"/> object</param>
        /// <returns>The response value as a <see cref="String"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if the <see cref="HttpWebRequest"/> argument is null.</exception>
        /// <exception cref="WebException">Thrown if there was an error getting the response.</exception>
        private static async Task<string> GetResponseAsync(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}