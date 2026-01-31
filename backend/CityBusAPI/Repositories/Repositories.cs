using CityBusAPI.Data;
using CityBusAPI.DTOs;
using CityBusAPI.Models;
using Microsoft.EntityFrameworkCore;
using Route = CityBusAPI.Models.Route;

namespace CityBusAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly CityBusDbContext _context;

        public AuthRepository(CityBusDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }

    public interface IAudioRepository
    {
        Task<IEnumerable<AudioFile>> GetAllAsync();
        Task<AudioFile?> GetByIdAsync(int id);
        Task<AudioFile> CreateAsync(AudioFile audioFile);
        Task<AudioFile> UpdateAsync(AudioFile audioFile);
        Task<bool> DeleteAsync(int id);
    }

    public class AudioRepository : IAudioRepository
    {
        private readonly CityBusDbContext _context;

        public AudioRepository(CityBusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AudioFile>> GetAllAsync()
        {
            return await _context.AudioFiles.ToListAsync();
        }

        public async Task<AudioFile?> GetByIdAsync(int id)
        {
            return await _context.AudioFiles.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AudioFile> CreateAsync(AudioFile audioFile)
        {
            _context.AudioFiles.Add(audioFile);
            await _context.SaveChangesAsync();
            return audioFile;
        }

        public async Task<AudioFile> UpdateAsync(AudioFile audioFile)
        {
            _context.AudioFiles.Update(audioFile);
            await _context.SaveChangesAsync();
            return audioFile;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var audioFile = await GetByIdAsync(id);
            if (audioFile == null) return false;

            _context.AudioFiles.Remove(audioFile);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IScheduleRepository
    {
        Task<IEnumerable<Schedule>> GetAllAsync();
        Task<Schedule?> GetByIdAsync(int id);
        Task<Schedule> CreateAsync(Schedule schedule);
        Task<Schedule> UpdateAsync(Schedule schedule);
        Task<bool> DeleteAsync(int id);
        Task<int> GetActiveSchedulesCountAsync();
    }

    public class ScheduleRepository : IScheduleRepository
    {
        private readonly CityBusDbContext _context;

        public ScheduleRepository(CityBusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _context.Schedules.Include(s => s.AudioFile).Include(s => s.Bus).ToListAsync();
        }

        public async Task<Schedule?> GetByIdAsync(int id)
        {
            return await _context.Schedules.Include(s => s.AudioFile).Include(s => s.Bus)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<Schedule> UpdateAsync(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var schedule = await GetByIdAsync(id);
            if (schedule == null) return false;

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetActiveSchedulesCountAsync()
        {
            return await _context.Schedules.Where(s => s.IsActive).CountAsync();
        }
    }

    public interface IBusRepository
    {
        Task<IEnumerable<Bus>> GetAllAsync();
        Task<Bus?> GetByIdAsync(int id);
        Task<Bus> CreateAsync(Bus bus);
        Task<Bus> UpdateAsync(Bus bus);
        Task<bool> DeleteAsync(int id);
    }

    public class BusRepository : IBusRepository
    {
        private readonly CityBusDbContext _context;

        public BusRepository(CityBusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bus>> GetAllAsync()
        {
            return await _context.Buses.Include(b => b.Route).ToListAsync();
        }

        public async Task<Bus?> GetByIdAsync(int id)
        {
            return await _context.Buses.Include(b => b.Route).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Bus> CreateAsync(Bus bus)
        {
            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();
            return bus;
        }

        public async Task<Bus> UpdateAsync(Bus bus)
        {
            _context.Buses.Update(bus);
            await _context.SaveChangesAsync();
            return bus;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bus = await GetByIdAsync(id);
            if (bus == null) return false;

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public interface IRouteRepository
    {
        Task<IEnumerable<Route>> GetAllAsync();
        Task<Route?> GetByIdAsync(int id);
        Task<Route> CreateAsync(Route route);
        Task<Route> UpdateAsync(Route route);
        Task<bool> DeleteAsync(int id);
    }

    public class RouteRepository : IRouteRepository
    {
        private readonly CityBusDbContext _context;

        public RouteRepository(CityBusDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Route>> GetAllAsync()
        {
            return await _context.Routes.ToListAsync();
        }

        public async Task<Route?> GetByIdAsync(int id)
        {
            return await _context.Routes.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Route> CreateAsync(Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<Route> UpdateAsync(Route route)
        {
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var route = await GetByIdAsync(id);
            if (route == null) return false;

            _context.Routes.Remove(route);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
