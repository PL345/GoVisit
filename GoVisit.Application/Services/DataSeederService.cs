using GoVisit.Core.Models;
using GoVisit.Core.Interfaces;

namespace GoVisit.Application.Services;

public class DataSeederService
{
    private readonly IRepository<Office> _officeRepository;
    private readonly IRepository<Service> _serviceRepository;
    private readonly IRepository<OfficeSchedule> _scheduleRepository;
    private readonly IRepository<Appointment> _appointmentRepository;

    public DataSeederService(
        IRepository<Office> officeRepository,
        IRepository<Service> serviceRepository,
        IRepository<OfficeSchedule> scheduleRepository,
        IRepository<Appointment> appointmentRepository)
    {
        _officeRepository = officeRepository;
        _serviceRepository = serviceRepository;
        _scheduleRepository = scheduleRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task SeedDataAsync()
    {
        // Only seed if collections are empty
        var existingServices = await _serviceRepository.GetAllAsync();
        if (!existingServices.Any())
        {
            await SeedServicesAsync();
            await SeedOfficesAsync();
            await SeedSchedulesAsync();
            await SeedAppointmentsAsync();
        }
    }

    private async Task SeedServicesAsync()
    {
        var services = new List<Service>
        {
            new() { ServiceId = "PASSPORT", Name = "הנפקת דרכון", Description = "הנפקת דרכון ישראלי חדש", Category = "זהות ואזרחות", EstimatedDurationMinutes = 30, RequiredDocuments = new() { "תעודת זהות", "תמונות", "אישור תשלום" } },
            new() { ServiceId = "ID_CARD", Name = "הנפקת תעודת זהות", Description = "הנפקת תעודת זהות חדשה או חידוש", Category = "זהות ואזרחות", EstimatedDurationMinutes = 20, RequiredDocuments = new() { "תעודת זהות ישנה", "תמונות" } },
            new() { ServiceId = "DRIVING_LICENSE", Name = "רישיון נהיגה", Description = "הנפקת רישיון נהיגה חדש או חידוש", Category = "תחבורה", EstimatedDurationMinutes = 45, RequiredDocuments = new() { "תעודת זהות", "אישור רפואי", "תמונות" } },
            new() { ServiceId = "BUSINESS_LICENSE", Name = "רישיון עסק", Description = "הנפקת רישיון עסק חדש", Category = "עסקים", EstimatedDurationMinutes = 60, RequiredDocuments = new() { "תעודת זהות", "מסמכי התאגדות", "אישור זכויות חתימה" } },
            new() { ServiceId = "BIRTH_CERTIFICATE", Name = "תעודת לידה", Description = "הנפקת תעודת לידה", Category = "זהות ואזרחות", EstimatedDurationMinutes = 15, RequiredDocuments = new() { "תעודת זהות של ההורים", "אישור לידה מבית החולים" } },
            new() { ServiceId = "MARRIAGE_CERTIFICATE", Name = "תעודת נישואין", Description = "הנפקת תעודת נישואין", Category = "זהות ואזרחות", EstimatedDurationMinutes = 20, RequiredDocuments = new() { "תעודות זהות", "אישור רווקות" } }
        };

        foreach (var service in services)
        {
            await _serviceRepository.CreateAsync(service);
        }
    }

    private async Task SeedOfficesAsync()
    {
        var offices = new List<Office>
        {
            new() { OfficeId = "TEL_AVIV_CENTER", Name = "משרד הפנים תל אביב מרכז", Address = "שלמה המלך 125", City = "תל אביב", Phone = "03-7394444", Services = new() { "PASSPORT", "ID_CARD", "BIRTH_CERTIFICATE", "MARRIAGE_CERTIFICATE" } },
            new() { OfficeId = "JERUSALEM_CENTER", Name = "משרד הפנים ירושלים מרכז", Address = "הרצל 38", City = "ירושלים", Phone = "02-6294444", Services = new() { "PASSPORT", "ID_CARD", "BIRTH_CERTIFICATE", "MARRIAGE_CERTIFICATE" } },
            new() { OfficeId = "HAIFA_CENTER", Name = "משרד הפנים חיפה מרכז", Address = "פל ים 15", City = "חיפה", Phone = "04-8634444", Services = new() { "PASSPORT", "ID_CARD", "BIRTH_CERTIFICATE" } },
            new() { OfficeId = "BEER_SHEVA_CENTER", Name = "משרד הפנים באר שבע מרכז", Address = "הרצל 53", City = "באר שבע", Phone = "08-6294444", Services = new() { "PASSPORT", "ID_CARD", "BIRTH_CERTIFICATE" } },
            new() { OfficeId = "TEL_AVIV_LICENSING", Name = "משרד התחבורה תל אביב", Address = "דרך בגין 42", City = "תל אביב", Phone = "03-6754444", Services = new() { "DRIVING_LICENSE" } },
            new() { OfficeId = "JERUSALEM_BUSINESS", Name = "משרד הכלכלה ירושלים", Address = "אגרון 5", City = "ירושלים", Phone = "02-6664444", Services = new() { "BUSINESS_LICENSE" } }
        };

        foreach (var office in offices)
        {
            await _officeRepository.CreateAsync(office);
        }
    }

    private async Task SeedSchedulesAsync()
    {
        var schedules = new List<OfficeSchedule>
        {
            new()
            {
                OfficeId = "TEL_AVIV_CENTER",
                ServiceType = "PASSPORT",
                WeeklySchedule = new()
                {
                    ["Sunday"] = new() { IsOpen = true, StartTime = new(8, 0, 0), EndTime = new(16, 0, 0), SlotDurationMinutes = 30, BreakStart = new(12, 0, 0), BreakEnd = new(13, 0, 0) },
                    ["Monday"] = new() { IsOpen = true, StartTime = new(8, 0, 0), EndTime = new(16, 0, 0), SlotDurationMinutes = 30, BreakStart = new(12, 0, 0), BreakEnd = new(13, 0, 0) },
                    ["Tuesday"] = new() { IsOpen = true, StartTime = new(8, 0, 0), EndTime = new(16, 0, 0), SlotDurationMinutes = 30, BreakStart = new(12, 0, 0), BreakEnd = new(13, 0, 0) },
                    ["Wednesday"] = new() { IsOpen = true, StartTime = new(8, 0, 0), EndTime = new(16, 0, 0), SlotDurationMinutes = 30, BreakStart = new(12, 0, 0), BreakEnd = new(13, 0, 0) },
                    ["Thursday"] = new() { IsOpen = true, StartTime = new(8, 0, 0), EndTime = new(16, 0, 0), SlotDurationMinutes = 30, BreakStart = new(12, 0, 0), BreakEnd = new(13, 0, 0) },
                    ["Friday"] = new() { IsOpen = true, StartTime = new(8, 0, 0), EndTime = new(12, 0, 0), SlotDurationMinutes = 30 },
                    ["Saturday"] = new() { IsOpen = false }
                },
                DateOverrides = new()
                {
                    new() { Date = new DateTime(2024, 4, 23), IsClosed = true, Reason = "יום העצמאות" },
                    new() { Date = new DateTime(2024, 10, 7), IsClosed = true, Reason = "סוכות" }
                }
            }
        };

        foreach (var schedule in schedules)
        {
            await _scheduleRepository.CreateAsync(schedule);
        }
    }

    private async Task SeedAppointmentsAsync()
    {
        var appointments = new List<Appointment>
        {
            new()
            {
                CitizenId = "123456789",
                CitizenName = "יוסי כהן",
                CitizenPhone = "050-1234567",
                OfficeId = "TEL_AVIV_LICENSING",
                ServiceType = "DRIVING_LICENSE",
                AppointmentDate = DateTime.Today.AddDays(1),
                AppointmentTime = new TimeSpan(9, 0, 0),
                Status = AppointmentStatus.Scheduled
            },
            new()
            {
                CitizenId = "987654321",
                CitizenName = "שרה לוי",
                CitizenPhone = "052-9876543",
                OfficeId = "TEL_AVIV_CENTER",
                ServiceType = "PASSPORT",
                AppointmentDate = DateTime.Today.AddDays(2),
                AppointmentTime = new TimeSpan(10, 30, 0),
                Status = AppointmentStatus.Confirmed
            },
            new()
            {
                CitizenId = "555666777",
                CitizenName = "דוד אברהם",
                CitizenPhone = "054-5556677",
                OfficeId = "TEL_AVIV_LICENSING",
                ServiceType = "DRIVING_LICENSE",
                AppointmentDate = DateTime.Today.AddDays(3),
                AppointmentTime = new TimeSpan(14, 0, 0),
                Status = AppointmentStatus.Scheduled
            }
        };

        foreach (var appointment in appointments)
        {
            await _appointmentRepository.CreateAsync(appointment);
        }
    }
}