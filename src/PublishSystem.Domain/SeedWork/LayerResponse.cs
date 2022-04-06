using System.Runtime.CompilerServices;
using PublishSystem.Domain.Enums.Error;
using PublishSystem.Domain.Extensions;

namespace PublishSystem.Domain.SeedWork
{
    public class LayerResponse<T>
    {
        public LayerResponse(T content, ICollection<string> errorList)
        {
            Content = content;
            ErrorList = errorList;
        }

        public LayerResponse(ICollection<string> errorList)
        {
            ErrorList = errorList;
        }

        public LayerResponse(T content)
        {
            Content = content;
        }

        public LayerResponse(string error)
        {
            ErrorList = new List<string> { error };
        }

        public LayerResponse()
        {
        }

        public LayerResponse(T content, ErrorCode errorCode, string message = "", [CallerMemberName] string callerMemberName = "")
        {
            Content = content;
            ErrorList.Add($"{errorCode.GetDescription()} {message} from {callerMemberName}");
        }

        public ICollection<string> ErrorList { get; set; } = new List<string>();

        public T Content { get; set; }

        public bool HasError() { return ErrorList.Any(); }

        public void AddError(ErrorCode errorCode, string message = "", [CallerMemberName] string callerMemberName = "")
        {
            ErrorList.Add($"{errorCode.GetDescription()} {message} from {callerMemberName}");
        }
    }
}