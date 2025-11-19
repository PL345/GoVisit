using GoVisit.Core.Models;

namespace GoVisit.Core.Interfaces;

public interface IOfficeService
{
    Task<IEnumerable<Office>> GetAllOfficesAsync();
    Task<Office?> GetOfficeByIdAsync(string id);
    Task<Office> CreateOfficeAsync(Office office);
    Task<bool> UpdateOfficeAsync(string id, Office office);
    Task<bool> DeleteOfficeAsync(string id);
    Task<IEnumerable<Office>> GetOfficesByServiceAsync(string serviceType);
}