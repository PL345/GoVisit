using GoVisit.Core.Models;

namespace GoVisit.Core.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment?> GetAppointmentByIdAsync(string id);
    Task<Appointment> CreateAppointmentAsync(CreateAppointmentRequest request);
    Task<bool> UpdateAppointmentStatusAsync(string id, AppointmentStatus status);
    Task<IEnumerable<Appointment>> GetAppointmentsByOfficeAsync(string officeId);
    Task<IEnumerable<Appointment>> GetAppointmentsByDateAsync(DateTime date);
    Task<IEnumerable<Appointment>> GetAppointmentsByCitizenAsync(string citizenId);
    Task UpdateAppointmentAsync(Appointment appointment);
    Task DeleteAppointmentAsync(string id);
}