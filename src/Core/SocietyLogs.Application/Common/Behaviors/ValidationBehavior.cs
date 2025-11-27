using FluentValidation;
using MediatR;

namespace SocietyLogs.Application.Common.Behaviors
{
    // IPipelineBehavior arayüzünü implemente eder.
    // TRequest: Gelen istek (Command/Query)
    // TResponse: Dönecek cevap
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // 1. ÖNCE (Handler çalışmadan önce):
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // İlgili command için yazılmış tüm validatorları çalıştır
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // Hata varsa Handler'a hiç gitme, direkt Exception fırlat!
                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            // 2. SONRA (Eğer hata yoksa):
            // next() diyerek bir sonraki adıma (yani Handler'a) geç.
            return await next();
        }
    }
}