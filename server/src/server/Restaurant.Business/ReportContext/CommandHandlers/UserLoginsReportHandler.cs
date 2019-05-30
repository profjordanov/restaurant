using Optional;
using Restaurant.Business.ReportContext.Generators;
using Restaurant.Core._Base;
using Restaurant.Core.ReportContext.Commands;
using Restaurant.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Business.ReportContext.CommandHandlers
{
    public class UserLoginsReportHandler : ICommandHandler<UserLoginsReportRequest, HttpFile>
    {
        private readonly UserLoginsReportGenerator _generator;

        public UserLoginsReportHandler(UserLoginsReportGenerator generator)
        {
            _generator = generator;
        }

        public async Task<Option<HttpFile, Error>> Handle(UserLoginsReportRequest request, CancellationToken cancellationToken) =>
            _generator.GetReport(request).SomeNotNull(Error.NotFound("Nullable report!"));
    }
}