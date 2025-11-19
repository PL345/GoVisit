using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisit.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;

    public AppointmentService(IRepository<Appointment> appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _appointmentRepository.GetAllAsync();
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(string id)
    {
        return await _appointmentRepository.GetByIdAsync(id);
    }

    public async Task<Appointment> CreateAppointmentAsync(CreateAppointmentRequest request)
    {
        var appointment = new Appointment
        {
            CitizenId = request.CitizenId,
            CitizenName = request.CitizenName,
            CitizenPhone = request.CitizenPhone,
            OfficeId = request.OfficeId,
            ServiceType = request.ServiceType,
            AppointmentDate = request.AppointmentDate,
            AppointmentTime = request.AppointmentTime
        };

        return await _appointmentRepository.CreateAsync(appointment);
    }

    public async Task<bool> UpdateAppointmentStatusAsync(string id, AppointmentStatus status)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null)
            return false;

        appointment.Status = status;
        return await _appointmentRepository.UpdateAsync(id, appointment);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByOfficeAsync(string officeId)
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return appointments.Where(a => a.OfficeId == officeId);
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date)
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return appointments.Where(a => a.AppointmentDate.Date == date.Date);
    }
}