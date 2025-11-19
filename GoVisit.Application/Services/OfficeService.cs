using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisit.Application.Services;

public class OfficeService : IOfficeService
{
    private readonly IRepository<Office> _officeRepository;

    public OfficeService(IRepository<Office> officeRepository)
    {
        _officeRepository = officeRepository;
    }

    public async Task<IEnumerable<Office>> GetAllOfficesAsync()
    {
        return await _officeRepository.GetAllAsync();
    }

    public async Task<Office?> GetOfficeByIdAsync(string id)
    {
        return await _officeRepository.GetByIdAsync(id);
    }

    public async Task<Office> CreateOfficeAsync(Office office)
    {
        return await _officeRepository.CreateAsync(office);
    }

    public async Task<bool> UpdateOfficeAsync(string id, Office office)
    {
        return await _officeRepository.UpdateAsync(id, office);
    }

    public async Task<bool> DeleteOfficeAsync(string id)
    {
        return await _officeRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Office>> GetOfficesByServiceAsync(string serviceType)
    {
        var offices = await _officeRepository.GetAllAsync();
        return offices.Where(o => o.Services.Contains(serviceType) && o.IsActive);
    }
}