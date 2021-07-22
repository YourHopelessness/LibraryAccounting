using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace LibraryAccounting.Pages
{
/// <summary>
/// ������ ��� ��������� ������
/// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// ������ ������
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// ����� ������� �������
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// ��������� �� ������
        /// </summary>
        public string ExceptionMessage { get; set; }
        private readonly ILogger<ErrorModel> _logger;

        /// <inheritdoc></inheritdoc>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }
        /// <inheritdoc></inheritdoc>
        public void OnGet([FromQuery] int error)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ExceptionMessage = $"������ ������: {error}";
            _logger.LogError(ExceptionMessage);
        }
    }
}