using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations
{
    public class SectionRepository(TicketSpotDbContext ticketSpotContext)
        : BaseRepository<Section>(ticketSpotContext), ISectionRepository
    {
        public new async Task<List<Section>> GetAllAsync() =>
            await _context.Sections
            .AsNoTracking()
            .AsQueryable()
            .Include(e => e.Venue)
            .ToListAsync();

        public async Task<Section> GetAsync(int id) =>
            await _context.Sections
            .AsNoTracking()
            .AsQueryable()
            .Where(e => e.Id == id)
            .Include(e => e.Venue)
            .SingleOrDefaultAsync();

        public async Task<Section> CreateAsync(Section sectionToCreate)
        {
            await _context.Sections.AddAsync(sectionToCreate);
            await _context.SaveChangesAsync();
            return sectionToCreate;
        }

        public async Task<Section> UpdateAsync(Section updatedSection)
        {
            var itemToUpdate = await Get(x => x.Id == updatedSection.Id)
                ?? throw new RecordNotFoundException("The section to be updated is not found in the database");

            _context.Sections.Update(updatedSection);
            await _context.SaveChangesAsync();
            return updatedSection;
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await GetAsync(id)
                ?? throw new RecordNotFoundException(string.Format("Section with id: {0} is not found in the database", id));

            _context.Sections.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
