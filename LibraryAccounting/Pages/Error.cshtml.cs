using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace LibraryAccounting.Pages
{
/// <summary>
/// Модель для обработки ошибок
/// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// Статус ошибки
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Показ статуса ошибкиы
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Сообщение об ошибки
        /// </summary>
        public string ExceptionMessage { get; set; }
        private readonly ILogger<ErrorModel> _logger;

        /// <inheritdoc></inheritdoc>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }
        /// <inheritdoc></inheritdoc>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            var exceptionHandlerPathFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                ExceptionMessage = "File error thrown";
                _logger.LogError(ExceptionMessage);
            }
            if (exceptionHandlerPathFeature?.Path == "/index")
            {
                ExceptionMessage += " from home page";
            }
        }
    }
}