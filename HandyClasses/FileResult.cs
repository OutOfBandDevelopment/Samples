using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleApplication1
{
    public static class FileResultEx
    {
        public static IHttpActionResult File(this ApiController controller, byte[] content, string contentType = null, string fileName = null, bool excludeContentDisposition = false)
        {
            Contract.Requires(content != null);
            Contract.Ensures(Contract.Result<IHttpActionResult>() != null);
            return new FileResult(content, contentType, fileName, excludeContentDisposition);
        }
        public static IHttpActionResult File(this ApiController controller, Stream content, string contentType = null, string fileName = null, bool excludeContentDisposition = false)
        {
            Contract.Requires(content != null);
            Contract.Ensures(Contract.Result<IHttpActionResult>() != null);
            return new FileResult(content, contentType, fileName, excludeContentDisposition);
        }
    }
    public class FileResult : IHttpActionResult
    {
        public FileResult(byte[] content, string contentType = null, string fileName = null, bool excludeContentDisposition = false)
            : this(new MemoryStream(content), contentType, fileName, excludeContentDisposition)
        {
            Contract.Requires(content != null);
            this.Content = new MemoryStream(content);
        }

        public FileResult(Stream content, string contentType = null, string fileName = null, bool excludeContentDisposition = false)
        {
            Contract.Requires(content != null);
            this.Content = content;
            this.ContentType = contentType;
            this.FileName = fileName;
            this.ExcludeContentDisposition = excludeContentDisposition;
        }

        private Stream Content { get; set; }
        private string ContentType { get; set; }
        private string FileName { get; set; }
        private bool ExcludeContentDisposition { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            Contract.Ensures(Contract.Result<Task<HttpResponseMessage>>() != null);
            var task = Task.Run(() =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(this.Content),
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(this.ContentType ?? "application/octet-stream");

                if (!this.ExcludeContentDisposition)
                {
                    var contentDisposition = response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    if (!string.IsNullOrWhiteSpace(this.FileName))
                        contentDisposition.FileName = this.FileName;
                }
                return response;
            }, cancellationToken);
            Contract.Assume(task != null);
            return task;
        }

    }
}
