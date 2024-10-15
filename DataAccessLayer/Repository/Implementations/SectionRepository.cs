using Common.Models;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository.Implementations;

public class SectionRepository(TicketSpotDbContext context) : IRepository<Section>
{
    public new async Task<List<Section>> GetAllAsync() =>
        await context.Sections
        .AsNoTracking()
        .AsQueryable()
        .Include(e => e.Venue)
        .ToListAsync();

    public async Task<Section> GetAsync(int id) =>
        await context.Sections
        .AsNoTracking()
        .AsQueryable()
        .Where(e => e.Id == id)
        .Include(e => e.Venue)
        .SingleOrDefaultAsync();

    public async Task<Section> CreateAsync(Section sectionToCreate)
    {
        await context.Sections.AddAsync(sectionToCreate);
        await context.SaveChangesAsync();
        return sectionToCreate;
    }

    public async Task<Section> UpdateAsync(Section updatedSection)
    {
        var itemToUpdate = await GetAsync(updatedSection.Id)
            ?? throw new RecordNotFoundException("The section to be updated is not found in the database");

        context.Sections.Update(updatedSection);
        await context.SaveChangesAsync();
        return updatedSection;
    }

    public async Task DeleteAsync(int id)
    {
        var itemToDelete = new Section { Id = id };

        context.Attach(itemToDelete);
        context.Sections.Remove(itemToDelete);
        await context.SaveChangesAsync();
    }
}
