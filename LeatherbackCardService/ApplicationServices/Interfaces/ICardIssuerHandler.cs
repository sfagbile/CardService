using System.Threading.Tasks;
using ApplicationServices.CardIssuance.Command;
using Shared.BaseResponse;

namespace ApplicationServices.Interfaces
{
    public interface ICardIssuerHandler: ICardIssuerService
    {
        Task<Result<string>> IssuerCard(IssuePhysicalCardCommand model);
    }
}