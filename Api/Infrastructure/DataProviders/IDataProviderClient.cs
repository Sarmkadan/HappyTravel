using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Infrastructure.DataProviders
{
    public interface IDataProviderClient
    {
        Task<Result<T, ProblemDetails>> Get<T>(Uri url, string languageCode = LocalizationHelper.DefaultLanguageCode,
            CancellationToken cancellationToken = default);


        Task<Result<TOut, ProblemDetails>> Post<T, TOut>(Uri url, T requestContent, string languageCode = LocalizationHelper.DefaultLanguageCode,
            CancellationToken cancellationToken = default);


        Task<Result<TOut, ProblemDetails>> Post<TOut>(Uri url, string languageCode = LocalizationHelper.DefaultLanguageCode,
            CancellationToken cancellationToken = default);
        
        
        Task<Result<VoidObject, ProblemDetails>> Post(Uri uri,
            string languageCode = LocalizationHelper.DefaultLanguageCode,
            CancellationToken cancellationToken = default);

        
        Task<Result<TOut, ProblemDetails>> Send<TOut>(HttpRequestMessage httpRequestMessage,
            string languageCode = LocalizationHelper.DefaultLanguageCode, CancellationToken cancellationToken = default);
    }
}