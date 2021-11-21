using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Chat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinksController
    {
        private const string RepoName = "https://github.com/Zedronar/next-chat";

        public class LinksObject
        {
            public string github { get; set; }
        }

        private readonly LinksObject _links;

        public LinksController(IFileProvider fileProvider)
        {
            _links = new LinksObject() { github = RepoName };
        }

        /// <summary>
        /// This one outputs the url this demo is hosted at, for specifying the GitHub link url.
        /// </summary>
        [HttpGet]
        public LinksObject Get()
        {
            return _links;
        }
    }
}