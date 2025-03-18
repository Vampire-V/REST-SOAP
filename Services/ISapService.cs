using System.ServiceModel;

namespace DemoSAP.Services
{
    [ServiceContract]
    public interface ISapService
    {
        [OperationContract]
        Task<string> GetItems();

        [OperationContract]
        Task<string> GetItem(int id);

        [OperationContract]
        Task<string> CreateItem(string requestBody);

        [OperationContract]
        Task<string> UpdateItem(int id, string requestBody);

        [OperationContract]
        Task<string> DeleteItem(int id);
    }
}
