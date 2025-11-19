using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisit.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IRepository<Appointment> _appointmentRepository;
    private readonly IRepository<Office> _officeRepository;
    private readonly IRepository<OfficeSchedule> _scheduleRepository;

    public AppointmentService(
        IRepository<Appointment> appointmentRepository,
        IRepository<Office> officeRepository,
        IRepository<OfficeSchedule> scheduleRepository)
    {
        _appointmentRepository = appointmentRepository;
        _officeRepository = officeRepository;
        _scheduleRepository = scheduleRepository;
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
        // Validate office exists and is active
        var office = await ValidateOfficeAsync(request.OfficeId);
        
        // Validate service is available at office
        ValidateServiceAvailability(office, request.ServiceType);
        
        // Validate appointment slot is available
        await ValidateAppointmentSlotAsync(request.OfficeId, request.ServiceType, request.AppointmentDate, request.AppointmentTime);

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

    public async Task<IEnumerable<Appointment>> GetAppointmentsByCitizenAsync(string citizenId)
    {
        var appointments = await _appointmentRepository.GetAllAsync();
        return appointments.Where(a => a.CitizenId == citizenId);
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        await _appointmentRepository.UpdateAsync(appointment.Id!, appointment);
    }

    public async Task DeleteAppointmentAsync(string id)
    {
        await _appointmentRepository.DeleteAsync(id);
    }

    private async Task<Office> ValidateOfficeAsync(string officeId)
    {
        var offices = await _officeRepository.GetAllAsync();
        var office = offices.FirstOrDefault(o => o.OfficeId == officeId);
        
        if (office == null)
            throw new ArgumentException($"Office with ID {officeId} not found");
            
        if (!office.IsActive)
            throw new ArgumentException($"Office {office.Name} is currently inactive");
            
        return office;
    }

    private static void ValidateServiceAvailability(Office office, string serviceType)
    {
        if (!office.Services.Contains(serviceType))
            throw new ArgumentException($"Service '{serviceType}' is not available at {office.Name}");
    }

    private async Task ValidateAppointmentSlotAsync(string officeId, string serviceType, DateTime date, TimeSpan time)
    {
        // Get office schedule
        var schedules = await _scheduleRepository.GetAllAsync();
        var schedule = schedules.FirstOrDefault(s => s.OfficeId == officeId && s.ServiceType == serviceType);
        
        if (schedule == null)
            throw new ArgumentException($"No schedule found for service '{serviceType}' at office {officeId}");

        // Check if date is in the past
        if (date.Date < DateTime.Today)
            throw new ArgumentException("Cannot book appointments in the past");

        // Check date overrides first
        var dateOverride = schedule.DateOverrides.FirstOrDefault(d => d.Date.Date == date.Date);
        if (dateOverride != null)
        {
            if (dateOverride.IsClosed)
                throw new ArgumentException($"Office is closed on {date:yyyy-MM-dd}: {dateOverride.Reason}");
                
            if (dateOverride.CustomSchedule != null)
            {
                ValidateTimeSlot(dateOverride.CustomSchedule, time);
            }
        }
        else
        {
            // Check weekly schedule
            var dayOfWeek = date.DayOfWeek.ToString();
            if (!schedule.WeeklySchedule.TryGetValue(dayOfWeek, out var daySchedule))
                throw new ArgumentException($"No schedule available for {dayOfWeek}");
                
            ValidateTimeSlot(daySchedule, time);
        }

        // Check if slot is already booked
        await ValidateSlotNotBookedAsync(officeId, date, time);
    }

    private static void ValidateTimeSlot(DaySchedule daySchedule, TimeSpan time)
    {
        if (!daySchedule.IsOpen)
            throw new ArgumentException("Office is closed on this day");
            
        if (time < daySchedule.StartTime || time >= daySchedule.EndTime)
            throw new ArgumentException($"Appointment time must be between {daySchedule.StartTime} and {daySchedule.EndTime}");
            
        // Check if time falls during break
        if (daySchedule.BreakStart.HasValue && daySchedule.BreakEnd.HasValue)
        {
            if (time >= daySchedule.BreakStart && time < daySchedule.BreakEnd)
                throw new ArgumentException($"Appointment time falls during break period ({daySchedule.BreakStart} - {daySchedule.BreakEnd})");
        }
    }

    private async Task ValidateSlotNotBookedAsync(string officeId, DateTime date, TimeSpan time)
    {
        var existingAppointments = await GetAppointmentsByOfficeAsync(officeId);
        var conflictingAppointment = existingAppointments.FirstOrDefault(a => 
            a.AppointmentDate.Date == date.Date && 
            a.AppointmentTime == time &&
            a.Status != AppointmentStatus.Cancelled);
            
        if (conflictingAppointment != null)
            throw new ArgumentException($"Time slot {time} on {date:yyyy-MM-dd} is already booked");
    }
}