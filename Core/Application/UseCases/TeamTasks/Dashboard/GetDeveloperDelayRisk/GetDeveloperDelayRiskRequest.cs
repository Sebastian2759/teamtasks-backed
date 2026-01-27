using Application.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.TeamTasks.Dashboard.GetDeveloperDelayRisk
{
    public class GetDeveloperDelayRiskRequest : IRequest<ResponseBase<GetDeveloperDelayRiskResponse>>
    {
    }
}