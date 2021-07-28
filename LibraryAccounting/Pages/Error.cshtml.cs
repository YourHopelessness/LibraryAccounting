using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

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
        /// Словарь статусов ошибок и ошибок
        /// </summary>
        private readonly Dictionary<int, string> _statusErrors = new()
            {{ 404 , "Запрашиваемый ресурс не найден" },
             { 400 , "Неверный запрос" } };

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
        public void OnGet([FromQuery] int error)
        {
            if (!Enum.IsDefined(typeof(HttpStatusCode), error)) error = 404;
            string message = "";
            if (_statusErrors.TryGetValue(error, out message))
            {
                ExceptionMessage = $"Статус ошибки: {error}\n{message}";
            }
            else
            {
                ExceptionMessage = $"Статус ошибки: {error}\n";
            }
            _logger.LogError(ExceptionMessage);
        }
    }
}